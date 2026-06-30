using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using Combat.StateMachine.States;

namespace EnemyStateMachine.Editor
{
    public sealed class EnemyStateGraphView : GraphView
    {
        /* -------------------------------- private fields -------------------------------- */
        readonly Dictionary<EffectStateBaseSO, Node> nodeLookup = new();
        readonly EnemyStateSearchWindow searchWindow;

        public EnemyStateGraphSO CurrentGraph { get; private set; }

        private IStateNode curEntryNode;    // 记录当前高亮

        public void SetCurrentGraph(EnemyStateGraphSO graph)
        {
            if (graph == null) return;
            CurrentGraph = graph;
        }

        /* -------------------------------- ctor -------------------------------- */
        public EnemyStateGraphView(EditorWindow host)
        {
            styleSheets.Add(Resources.Load<StyleSheet>("EnemyGraphStyle"));
            CurrentGraph = ((EnemyStateGraphWindow)host).CurrentAsset;

            /* 基本交互操作 */
            this.AddManipulator(new ContentZoomer());
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            Insert(0, new GridBackground());

            /* Search Window（Shift + Space）*/
            searchWindow = ScriptableObject.CreateInstance<EnemyStateSearchWindow>();
            searchWindow.Init(this, host);
            nodeCreationRequest = ctx =>
                SearchWindow.Open(new SearchWindowContext(ctx.screenMousePosition), searchWindow);

            graphViewChanged = OnGraphViewChanged;
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter _)
        {
            var list = new List<Port>();
            ports.ForEach(p =>
            {
                if (p == startPort) return;                 // 同一个端口
                if (p.direction == startPort.direction) return; // 同方向
                list.Add(p);
            });
            return list;
        }

        /* ======================================================================
         *  I.   PUBLIC  ——  资产 <=> 视图
         * ==================================================================== */
        #region Populate / Serialize

        public void Populate(EnemyStateGraphSO graph)
        {
            DeleteElements(graphElements.ToList());
            nodeLookup.Clear();

            var posDict = graph.nodeMetas.ToDictionary(m => m.node, m => m.position);

            /* ---------- 1. 生成节点 ---------- */
            foreach (var so in graph.states)
                CreateNodeView(so, posDict);

            if (graph.entry && nodeLookup.TryGetValue(graph.entry, out var entryNode))
            {
                entryNode.AddToClassList("entryNode");
                curEntryNode = (IStateNode)entryNode;
            }

            /* ---------- 2. 补边：普通 Next / 条件等 ---------- */
            foreach (var so in graph.states)
            {
                switch (so)
                {
                    /* ---------------- BasicProbEffectStateSO ---------------- */
                    case BasicProbEffectStateSO probSO:
                        var probNode = (ProbEffectStateNode)nodeLookup[probSO];
                        var transitions = probSO.Transitions;
                        for (int i = 0; i < transitions.Count; ++i)
                        {
                            var elem = transitions[i];
                            var targetState = elem.targetState;
                            if (targetState && nodeLookup.TryGetValue(targetState, out var dstNode))
                            {
                                var edge = ConnectPort(probNode.GetPortByIndex(i), ((IStateNode)dstNode).InputPort);
                                AddElement(edge);
                            }
                        }
                        break;

                    /* ---------------- 其他 State（Basic / Condition 等） ---------------- */
                    case BasicConditionEffectStateSO condSO:
                        // true / false 两条边
                        ConnectIf(condSO, condSO.trueState, "True");
                        ConnectIf(condSO, condSO.falseState, "False");
                        break;

                    case BasicEffectStateSO basicSO:
                        ConnectIf(basicSO, basicSO.NextState, "Next");
                        break;
                    default:
                        break;
                }
            }

            void ConnectIf(EffectStateBaseSO from, EffectStateBaseSO to, string portName)
            {
                if (!to) return;
                var fromNode = (IStateNode)nodeLookup[from];
                var toNode = (IStateNode)nodeLookup[to];
                var outPort = fromNode.GetPort(portName);
                var edge = ConnectPort(outPort, toNode.InputPort);
                AddElement(edge);
            }
        }

        private Edge ConnectPort(Port output, Port input)
        {
            return output.ConnectTo(input);
        }

        private void SerializeProbNode(BasicProbEffectStateSO probSO, ProbEffectStateNode probNode)
        {
            var soSer = new SerializedObject(probSO);
            var listProp = soSer.FindProperty("transitions");

            for (int i = 0; i < listProp.arraySize; ++i)
            {
                var elem = listProp.GetArrayElementAtIndex(i);
                var tgtProp = elem.FindPropertyRelative("targetState");

                /* 找端口对应的 edge（若断开则清空引用） */
                var port = probNode.GetPortByIndex(i);
                var edge = port.connections.FirstOrDefault();
                tgtProp.objectReferenceValue =
                    edge != null
                    ? ((IStateNode)edge.input.node).TargetSO
                    : null;
            }
            soSer.ApplyModifiedProperties();
        }

        private void SerializeBasicNode(BasicEffectStateSO basicSO, EffectStateNode basicNode)
        {
            var soSer = new SerializedObject(basicSO);
            var nextProp = soSer.FindProperty("NextState");

            /* 找端口对应的 edge（若断开则清空引用） */
            var outPort = basicNode.GetPort("Next");
            var edge = outPort.connections.FirstOrDefault();
            nextProp.objectReferenceValue =
                edge != null
                ? ((IStateNode)edge.input.node).TargetSO
                : null;

            soSer.ApplyModifiedProperties();
        }

        private void SerializeConditionNode(BasicConditionEffectStateSO conditionSO, EffectStateNode conditionNode)
        {
            var soSer = new SerializedObject(conditionSO);
            var trueProp = soSer.FindProperty("trueState");
            var falseProp = soSer.FindProperty("falseState");

            var truePort = conditionNode.GetPort("True");
            var falsePort = conditionNode.GetPort("False");

            var trueEdge = truePort.connections.FirstOrDefault();
            var falseEdge = falsePort.connections.FirstOrDefault();

            trueProp.objectReferenceValue =
                trueEdge != null
                ? ((IStateNode)trueEdge.input.node).TargetSO
                : null;
            falseProp.objectReferenceValue =
                falseEdge != null
                ? ((IStateNode)falseEdge.input.node).TargetSO
                : null;
            
            soSer.ApplyModifiedProperties();
        }

        public void Serialize(EnemyStateGraphSO graph)
        {
            /* ---------- 0. 保存位置 & 记录 Undo ---------- */
            Undo.RecordObject(graph, "Enemy Graph Modified");
            foreach (var node in nodes)
            {
                var so = ((IStateNode)node).TargetSO;
                var rect = node.GetPosition();
                var guid = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(so));
                NodePosPrefs.Save(guid, rect);
            }

            /* ---------- 1. 写回 states 列表（去重） ---------- */
            graph.states = nodeLookup.Keys.ToList();

            /* ---------- 2. 写回 Prob 节点 Transition 的连接 ---------- */
            foreach (var kv in nodeLookup)
            {
                var so = kv.Key;
                var node = kv.Value;

                switch (so)
                {
                    case BasicProbEffectStateSO probSO:
                        SerializeProbNode(probSO, (ProbEffectStateNode)node);
                        break;

                    case BasicEffectStateSO basicSO:
                        SerializeBasicNode(basicSO, (EffectStateNode)node);
                        break;

                    case BasicConditionEffectStateSO condSO:
                        SerializeConditionNode(condSO, (EffectStateNode)node);
                        break;

                    default:
                        continue; // 其他类型不处理
                }
            }

            /* ---------- 3. 保存节点位置 ---------- */
            graph.nodeMetas.Clear();
            foreach (var n in nodes)
            {
                var sn = (IStateNode)n;
                graph.nodeMetas.Add(new EnemyStateGraphSO.NodeMeta
                {
                    node = sn.TargetSO,
                    position = n.GetPosition().position
                });
            }
            EditorUtility.SetDirty(graph);

            /* ---------- 4. Asset 保存 ---------- */
            AssetDatabase.SaveAssets();
        }
        #endregion

        /* ======================================================================
         *  II.  PRIVATE —— 事件回调 / 工具
         * ==================================================================== */
        #region GraphView helpers

        public Node CreateNodeView(EffectStateBaseSO so, Dictionary<EffectStateBaseSO, Vector2> posDict)
        {
            /* 若已有则直接返回 */
            if (nodeLookup.TryGetValue(so, out var existed)) return existed;

            Node node = so switch
            {
                BasicProbEffectStateSO probSO => new ProbEffectStateNode(probSO),
                _ => new EffectStateNode(so),// 复用旧的万能 Node
            };

            /* 还原位置 */
            if (posDict.TryGetValue(so, out var pos))
                node.SetPosition(new Rect(pos, Vector2.zero));
            else
                node.SetPosition(new Rect(Vector2.zero, Vector2.zero));
            AddElement(node);
            nodeLookup[so] = node;
            return node;
        }

        public Node CreateNodeView(EffectStateBaseSO so, Vector2 pos)
        {
            Node node = so switch
            {
                BasicProbEffectStateSO prob => new ProbEffectStateNode(prob),
                _ => new EffectStateNode(so),
            };

            node.SetPosition(new Rect(pos, Vector2.zero));
            AddElement(node);
            nodeLookup[so] = node;
            return node;
        }

        /* —— 端口连接 / 断开时，实时同步 Prob 节点 —— */
        GraphViewChange OnGraphViewChanged(GraphViewChange change)
        {
            /* ----------- 新建 Edge 时写回 targetState ----------- */
            if (change.edgesToCreate != null)
                foreach (var e in change.edgesToCreate)
                    SyncEdge(e);

            /* ----------- 删除 Edge 时同样清空引用 ----------- */
            if (change.elementsToRemove != null)
            {
                foreach (var el in change.elementsToRemove.OfType<Edge>())
                    SyncEdge(el, isDelete: true);
            }
            return change;

            /* 内部局部函数 */
            void SyncEdge(Edge edge, bool isDelete = false)
            {
                if (edge.output.node is ProbEffectStateNode probNode)
                    SyncEdgeProb(edge, isDelete);
                else
                    SyncEdgeBasic(edge, isDelete);
            }
        }

        private void SyncEdgeProb(Edge edge, bool isDelete = false)
        {
            if (edge.output.node is not ProbEffectStateNode probNode) return;
            var so = (BasicProbEffectStateSO)probNode.TargetSO;
            int idx = (int)edge.output.userData;

            so.Transitions[idx].targetState = isDelete
                ? null
                : ((IStateNode)edge.input.node).TargetSO;
        }

        private void SyncEdgeBasic(Edge edge, bool isDelete = false)
        {
            if (edge.output.node is not EffectStateNode basicNode) return;
            var so = basicNode.TargetSO;
            switch (so)
            {
                case BasicEffectStateSO basicSO:
                    SyncEdgeBasic(basicSO, edge, isDelete);
                    break;
                case BasicConditionEffectStateSO condSO:
                    SyncEdgeBasic(condSO, edge, isDelete);
                    break;
            }
        }

        private void SyncEdgeBasic(BasicEffectStateSO basicSO, Edge edge, bool isDelete)
        {
            basicSO.NextState = isDelete
                ? null
                : ((IStateNode)edge.input.node).TargetSO;
        }

        private void SyncEdgeBasic(BasicConditionEffectStateSO condSO, Edge edge, bool isDelete)
        {
            if (edge.output.portName == "True")
            {
                condSO.trueState = isDelete
                    ? null
                    : ((IStateNode)edge.input.node).TargetSO;
            }
            else if (edge.output.portName == "False")
            {
                condSO.falseState = isDelete
                    ? null
                    : ((IStateNode)edge.input.node).TargetSO;
            }
        }

        internal void SetEntryNode(IStateNode node)
        {
            var graph = CurrentGraph;                  // 给 GraphView 暴露只读 CurrentGraph
            if (graph == null) return;

            // 1) 数据
            graph.entry = node.TargetSO;
            EditorUtility.SetDirty(graph);

            // 2) 视觉：给老入口移除样式，新入口加样式
            if (curEntryNode is VisualElement veOld)
                veOld.RemoveFromClassList("entryNode");

            ((VisualElement)node).AddToClassList("entryNode");
            curEntryNode = node;
        }
        #endregion

        public bool Validate()
        {
            foreach (var node in nodes)
            {
                if (node.Query<Port>().ToList().Any(p => p.direction == Direction.Output && !p.connected))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
