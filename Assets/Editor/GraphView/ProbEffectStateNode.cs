using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using Combat.StateMachine.States;
using System.Linq;      // BasicProbEffectStateSO 所在命名空间

namespace EnemyStateMachine.Editor
{
    /// <summary>
    ///   专门给 BasicProbEffectStateSO 用的 Node。  
    ///   每条 Transition ⇨ 一个独立输出端口（单容量），端口名显示权重。
    ///   port.userData = Transition 在 list 中的索引，方便 GraphView 写回。
    /// </summary>
    public sealed class ProbEffectStateNode : Node, IStateNode
    {
        private BasicProbEffectStateSO target;
        private Port inputPort;   // 统一输入
        public Port InputPort => inputPort;
        public EffectStateBaseSO TargetSO => target;
        public Port GetPort(string name) => null;
        public Port GetPortByIndex(int i) =>
            i < outputContainer.childCount ? (Port)outputContainer.ElementAt(i) : null;

        private bool _portsDirty = false;

        public ProbEffectStateNode(BasicProbEffectStateSO so)
        {
            target = so;
            title = "概率";

            /* -------- 输入端口 -------- */
            inputPort = InstantiatePort(Orientation.Horizontal, Direction.Input,
                                        Port.Capacity.Multi, typeof(bool));
            InputPort.portName = "";
            inputContainer.Add(InputPort);

            /* -------- Inspector -------- */
            var inspector = new IMGUIContainer(DrawInspector);
            extensionContainer.Add(inspector);

            /* -------- 添加转移按钮 -------- */
            var addBtn = new Button(() =>
            {
                Undo.RecordObject(target, "Add Transition");
                target.Transitions.Add(new BasicProbEffectStateSO.Transition { probabilityWeight = 1f });
                EditorUtility.SetDirty(target);
                _portsDirty = true;
            })
            { text = "+ Transition" };
            titleButtonContainer.Add(addBtn);

            /* 初始端口 */
            RebuildTransitionPorts();
            RefreshPorts();
            RefreshExpandedState();
        }

        /* ========== inspector 用 IMGUI 画序列化列表 ========== */
        void DrawInspector()
        {
            if (target == null) return;

            EditorGUI.BeginChangeCheck();

            /* 遍历并绘制每条 Transition 行 */
            for (int i = 0; i < target.Transitions.Count; ++i)
            {
                var t = target.Transitions[i];
                EditorGUILayout.BeginHorizontal();
                t.probabilityWeight = EditorGUILayout.FloatField($"W{i}", t.probabilityWeight);
                t.targetState = (EffectStateBaseSO)EditorGUILayout.ObjectField(
                                    t.targetState, typeof(EffectStateBaseSO), false);

                if (GUILayout.Button("-", GUILayout.Width(18)))
                {
                    Undo.RecordObject(target, "Remove Transition");
                    target.Transitions.RemoveAt(i);
                    _portsDirty = true;
                    break;
                }
                EditorGUILayout.EndHorizontal();
            }

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Edit Transition");
                EditorUtility.SetDirty(target);
                _portsDirty = true;              // 可能改了权重，需要刷新端口名
            }

            /* 若标记脏，则刷新一次端口，然后复位标记 */
            if (_portsDirty)
            {
                RebuildTransitionPorts();
                _portsDirty = false;
            }
        }

        void RebuildTransitionPorts()
        {
            /* ---------- 1. 数量对齐 ---------- */
            int need = target.Transitions.Count;
            int cur = outputContainer.childCount;

            // 多于需要 → 从末尾开始删（同时断开 edge）
            for (int i = cur - 1; i >= need; --i)
            {
                var p = (Port)outputContainer.ElementAt(i);
                /* 断开所有连线 */
                foreach (var e in p.connections.ToArray())
                    e.input.Disconnect(e);   // GraphView 2022+
                p.DisconnectAll();           // 保险
                outputContainer.RemoveAt(i);
            }

            // 少于需要 → 追加端口
            for (int i = cur; i < need; ++i)
                outputContainer.Add(CreateOnePort(i));

            /* ---------- 2. 更新端口名字 ---------- */
            for (int i = 0; i < need; ++i)
            {
                var port = (Port)outputContainer.ElementAt(i);
                float weight = Mathf.Max(0, target.Transitions[i].probabilityWeight);
                port.portName = weight.ToString("0.###");
            }

            RefreshPorts();
        }

        /* ========== 列表增添一条空 Transition ========== */
        void AddTransition()
        {
            Undo.RecordObject(target, "Add Prob Transition");
            target.AddTransition(null, 1);       // 默认权重 1
            EditorUtility.SetDirty(target);
            RebuildTransitionPorts();
            RefreshPorts();
        }

        Port CreateOnePort(int idx)
        {
            var p = InstantiatePort(Orientation.Horizontal, Direction.Output,
                                    Port.Capacity.Single, typeof(bool));
            p.userData = idx;
            return p;
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            base.BuildContextualMenu(evt);
            evt.menu.AppendAction("Set As Entry", _ =>
            {
                var gv = GetFirstAncestorOfType<EnemyStateGraphView>();
                gv?.SetEntryNode(this);          // 新增 API，见下
            });
        }
    }
}
