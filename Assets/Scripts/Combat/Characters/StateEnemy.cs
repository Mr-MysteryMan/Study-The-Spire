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
        public EnemyStateGraphSO stateMachine;
        public TypeToIconLib IconLib;

        [SerializeField] private EnemyIndentController enemyIndentController;

        private IEffectState curState;

        public override Sprite Indent => IconLib.GetIcon(Effect.EffectType);
        public override IEnemyEffect Effect => curState.Effect;

        public override void OnCombatStart()
        {
            base.OnCombatStart();
            curState = stateMachine.entry;
            curState.OnEnter();
            enemyIndentController.Indent = Indent;
        }

        public override void OnTurnEnd()
        {
            base.OnTurnEnd();
            var state = curState.MoveToNextState();
            if (state is not IEffectState effectState)
            {
                Debug.LogError($"状态 {curState} 的下一个状态不是 IEffectState 类型，无法继续移动到下一个状态。");
                return;
            }
            curState = effectState;
            enemyIndentController.Indent = Indent;
        }
    }
}