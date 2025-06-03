using Combat.Characters.EnemyEffect;
using UnityEngine;

namespace Combat.StateMachine.States
{
    [CreateAssetMenu(fileName = "BasicEffectState", menuName = "Combat/StateMachine/EffectStates/BasicEffectState")]
    public class BasicEffectStateSO : EffectStateBaseSO
    {
        [SerializeField] private TypedEffectInfo typedEffect;
        private Basic.BasicEffectState state = new Basic.BasicEffectState();
        protected override IEffectState EffectState => state;

        public override void Init()
        {
            this.state.Init(Effect);
        }
    }
}