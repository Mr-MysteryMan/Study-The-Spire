using System;
using Combat.Characters.EnemyEffect;
using UnityEngine;

namespace Combat.StateMachine.States
{
    [CreateAssetMenu(fileName = "BasicConditionEffectState", menuName = "Combat/StateMachine/EffectStates/BasicConditionEffectState")]
    public class BasicConditionEffectStateSO : EffectStateBaseSO
    {
        [SerializeField] public EffectStateBaseSO trueState;
        [SerializeField] public EffectStateBaseSO falseState;
        [SerializeField] private Func<bool> condition;
        private IEffectState state;

        public override IEnemyEffect Effect => state.Effect;

        public override IState GetNextState()
        {
            return state.GetNextState();
        }

        public override void OnEnter()
        {
            if (condition())
            {
                state = trueState;
                trueState.OnEnter();
            }
            else
            {
                state = falseState;
                falseState.OnEnter();
            }
        }

        public override void OnExit() { }
    }
}