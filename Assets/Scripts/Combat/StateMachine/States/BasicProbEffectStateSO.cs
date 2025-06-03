using System.Collections.Generic;
using UnityEngine;

namespace Combat.StateMachine.States
{
    [CreateAssetMenu(fileName = "BasicProbEffectState", menuName = "Combat/StateMachine/EffectStates/BasicProbEffectState")]
    public class BasicProbEffectStateSO : EffectStateBaseSO
    {
        [System.Serializable]
        private class Transition
        {
            public EffectStateBaseSO targetState;  // 目标状态
            public float probabilityWeight; // 转移权重（非归一化）
        }

        [SerializeField] private List<Transition> transitions; // 转移列表

        private Basic.BasicProbEffectState state = new Basic.BasicProbEffectState();
        protected override IEffectState EffectState => state;
        public override void Init()
        {
            this.transitions.ForEach(t => t.targetState.Init());
            this.state.Init(transitions.ConvertAll(t => new Basic.BasicProbEffectState.Transition
            {
                targetState = t.targetState,
                probabilityWeight = t.probabilityWeight
            }));
        }
    }
}