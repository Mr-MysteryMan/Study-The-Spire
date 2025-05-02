using Combat.CardEffect;
using Combat.Characters.EnemyEffect;
using Combat.Characters.EnemyEffect.UIController;
using Combat.StateMachine.States;
using UnityEngine;

namespace Combat.Characters {
    public class StateEnemy : Enemy {
        public StateMachine.StateMachine stateMachine;
        public EffectStateMakerSO effectStateMakerSO;

        [SerializeField] private EnemyIndentController enemyIndentController;

        public override Sprite Indent => ((EffectState)stateMachine.CurrentState).effect.Icon;
        public override ITypedEffect Effect => ((EffectState)stateMachine.CurrentState).effect;

        public override void OnCombatStart()
        {
            base.OnCombatStart();
            stateMachine.Init(effectStateMakerSO.State);
            enemyIndentController.Indent = Indent;
        }

        void Awake()
        {
            stateMachine = GetComponent<StateMachine.StateMachine>();
        }

        public override void OnTurnEnd()
        {
            base.OnTurnEnd();
            if (stateMachine.CurrentState is EffectState)
            {
                stateMachine.TransitionToNextState();
                enemyIndentController.Indent = Indent;
            }
        }
    }
}