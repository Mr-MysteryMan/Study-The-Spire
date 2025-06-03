using System.Collections.Generic;
using UnityEngine;

namespace Combat.StateMachine.States
{
    [CreateAssetMenu(fileName = "LinkEffectState", menuName = "Combat/StateMachine/EffectStates/LinkEffectState")]
    public class LinkEffectStateSO : EffectStateBaseSO
    {
        [SerializeField] private EffectStateBaseSO curState;
        [SerializeField] private EffectStateBaseSO nextState;
        protected override IEffectState EffectState => curState;

        protected override void _Init()
        {
            curState.Init();
            nextState.Init();
            curState.SetNextState(nextState);
        }
    }
}