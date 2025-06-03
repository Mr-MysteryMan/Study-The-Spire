using System;
using UnityEngine;

namespace Combat.StateMachine.States
{
    [CreateAssetMenu(fileName = "BasicConditionEffectState", menuName = "Combat/StateMachine/EffectStates/BasicConditionEffectState")]
    public class BasicConditionEffectStateSO : EffectStateBaseSO
    {
        [SerializeField] private EffectStateBaseSO trueState;
        [SerializeField] private EffectStateBaseSO falseState;
        [SerializeField] private Func<bool> condition;
        private Basic.BasicConditionEffectState state = new Basic.BasicConditionEffectState();
        protected override IEffectState EffectState => state;

        public override void Init()
        {
            trueState.Init();
            falseState.Init();
            this.state.Init(trueState, falseState, condition);
        }
    }
}