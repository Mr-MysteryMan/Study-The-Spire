using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using Combat.StateMachine.States;

namespace EnemyStateMachine.Editor
{
    public class EffectStateNode : Node, IStateNode
    {
        public readonly EffectStateBaseSO target;
        public readonly Port inputPort;
        readonly VisualElement inspector;

        public Port InputPort => inputPort;
        public EffectStateBaseSO TargetSO => target;

        public EffectStateNode(EffectStateBaseSO so)
        {
            target = so;

            inputPort = InstantiatePort(Orientation.Horizontal,
                                        Direction.Input,
                                        Port.Capacity.Multi,
                                        typeof(bool));
            inputPort.portName = "";
            inputContainer.Add(inputPort);

            // 根据类型创建不同出口
            void AddOut(string name)
            {
                var p = InstantiatePort(Orientation.Horizontal, Direction.Output,
                                        Port.Capacity.Single, typeof(bool));
                p.portName = name;
                outputContainer.Add(p);
            }

            if (so is BasicEffectStateSO)
            {
                title = "效果";
                AddOut("Next");
            }
            else if (so is BasicConditionEffectStateSO)
            {
                title = "条件";
                AddOut("True");
                AddOut("False");
            }

            // Inspector 嵌入
            inspector = new IMGUIContainer(() =>
            {
                if (target == null) return;
                using (var scope = new EditorGUI.ChangeCheckScope())
                {
                    var soSer = new SerializedObject(target);
                    soSer.Update();
                    var prop = soSer.GetIterator();
                    bool enter = true;
                    while (prop.NextVisible(enter))
                    {
                        if (prop.name == "m_Script") continue;
                        EditorGUILayout.PropertyField(prop, true);
                        enter = false;
                    }
                    soSer.ApplyModifiedProperties();
                }
            });
            extensionContainer.Add(inspector);

            RefreshPorts();
            RefreshExpandedState();
        }

        public Port GetPort(string name) =>
            outputContainer.Query<Port>().Where(p => p.portName == name).First();
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
