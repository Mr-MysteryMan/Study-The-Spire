using Cards.CardEffect;
using Combat.Characters.EnemyEffect;
using Combat.Characters.EnemyEffect.UIController;
using Combat.StateMachine;
using Combat.StateMachine.States;
using UnityEngine;

namespace Combat.Characters
{
    public class StateEnemy : Enemy
    {
        public EffectStateBaseSO stateMachine;
        public TypeToIconLib IconLib;

        [SerializeField] private EnemyIndentController enemyIndentController;

        public override Sprite Indent => IconLib.GetIcon(Effect.EffectType);
        public override IEnemyEffect Effect => stateMachine.Effect;

        public override void OnCombatStart()
        {
            base.OnCombatStart();
            stateMachine.Init();
            stateMachine.OnEnter();
            enemyIndentController.Indent = Indent;
        }

        public override void OnTurnEnd()
        {
            base.OnTurnEnd();
            stateMachine.MoveToNextState();
            enemyIndentController.Indent = Indent;
        }
    }
}