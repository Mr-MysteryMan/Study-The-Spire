using System.Collections.Generic;
using UnityEngine;

namespace Combat.StateMachine.States
{
    [CreateAssetMenu(fileName = "BasicProbEffectState", menuName = "Combat/StateMachine/EffectStates/BasicProbEffectState")]
    public class BasicProbEffectStateSO : EffectStateBaseSO
    {
        [System.Serializable]
        public class Transition
        {
            public EffectStateBaseSO targetState;  // 目标状态
            public float probabilityWeight; // 转移权重（非归一化）
        }

        [SerializeField] private List<Transition> transitions = new List<Transition>(); // 转移列表

        private Basic.BasicProbEffectState state = new Basic.BasicProbEffectState();
        protected override IEffectState EffectState => state;
        public int TransitionCount => transitions?.Count ?? 0;
        public List<Transition> Transitions => transitions;
        protected override void _Init()
        {
            this.transitions.ForEach(t => t.targetState.Init());
            this.state.Init(transitions.ConvertAll(t => new Basic.BasicProbEffectState.Transition
            {
                targetState = t.targetState,
                probabilityWeight = t.probabilityWeight
            }));
        }

        public void AddTransition(EffectStateBaseSO targetState, float probabilityWeight)
        {
            transitions.Add(new Transition { targetState = targetState, probabilityWeight = probabilityWeight });
        }
    }
}