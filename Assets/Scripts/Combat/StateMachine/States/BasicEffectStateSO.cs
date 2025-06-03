using Combat.Characters.EnemyEffect;
using UnityEngine;

namespace Combat.StateMachine.States
{
    [CreateAssetMenu(fileName = "BasicEffectState", menuName = "Combat/StateMachine/EffectStates/BasicEffectState")]
    public class BasicEffectStateSO : EffectStateBaseSO
    {
        [SerializeField] private TypedEffectInfo typedEffect;

        public EffectStateBaseSO NextState;

        public override IEnemyEffect Effect => typedEffect.GetEnemyEffect();

        public override IState GetNextState()
        {
            return NextState;
        }

        public override void OnEnter() { }

        public override void OnExit() { }
    }
}