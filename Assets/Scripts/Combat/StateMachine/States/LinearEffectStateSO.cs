using System.Collections.Generic;
using UnityEngine;

namespace Combat.StateMachine.States
{
    [CreateAssetMenu(fileName = "LinearEffectState", menuName = "Combat/StateMachine/EffectStates/LinearEffectState")]
    public class LinearEffectStateSO : EffectStateBaseSO
    {
        [SerializeField] private List<EffectStateBaseSO> states;
        private Basic.LinearEffectState state = new Basic.LinearEffectState();
        protected override IEffectState EffectState => state;

        protected override void _Init()
        {
            this.states.ForEach(s => s.Init());
            this.state.Init(states.ConvertAll(s => s as IEffectState));
        }
    }
}