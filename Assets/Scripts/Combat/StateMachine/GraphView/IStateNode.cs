using Combat.StateMachine.States;
using UnityEditor.Experimental.GraphView;

namespace EnemyStateMachine.Editor
{
    /// <summary>所有自定义节点都实现，方便 GraphView 不用 dynamic。</summary>
    internal interface IStateNode
    {
        Port InputPort { get; }
        Port GetPort(string name);        // 可返回 null
        EffectStateBaseSO TargetSO { get; }
    }
}