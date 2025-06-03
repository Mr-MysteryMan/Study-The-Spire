using System.Collections.Generic;
using UnityEngine;

namespace Combat.StateMachine.States
{
    [CreateAssetMenu(fileName = "LoopEffectState", menuName = "Combat/StateMachine/EffectStates/LoopEffectState")]
    public class LoopEffectStateSO : EffectStateBaseSO
    {
        [SerializeField] private List<EffectStateBaseSO> states;
        private Basic.LoopEffectState state = new Basic.LoopEffectState();
        protected override IEffectState EffectState => state;

        public override void Init()
        {
            this.states.ForEach(s => s.Init());
            this.state.Init(states.ConvertAll(s => s as IEffectState));
        }
    }
}