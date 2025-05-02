using UnityEngine;
using Combat.Events.Turn;

namespace Combat
{
    public enum WhoseTurn
    {
        Player, // 玩家回合
        Enemy, // 敌人回合
    }


    [RequireComponent(typeof(CombatSystem))]
    public class TurnSystem : MonoBehaviour
    {
        public int turnNum = 0; // 当前回合数

        [SerializeField] private CombatSystem combatSystem; // 战斗系统

        private WhoseTurn whoseTurn = WhoseTurn.Player; // 当前回合的操作者

        private enum TurnState
        {
            Started, // 回合开始
            Ended, // 回合结束
        }

        private TurnState turnState = TurnState.Started; // 回合状态

        private void Awake()
        {
            combatSystem = GetComponent<CombatSystem>();
        }

        private void Start()
        {
            // 初始化回合系统
            InitTurnSystem();
        }

        private void InitTurnSystem()
        {
            turnNum = 0;
            whoseTurn = WhoseTurn.Player; // 默认玩家先手
            turnState = TurnState.Started; // 默认回合开始
            Debug.Log("回合系统初始化完成");
            StartCombat();
        }

        public void StartCombat()
        {
            // 开始战斗
            Debug.Log("战斗开始");
            combatSystem.EventManager.Publish(new CombatStartEvent());
            combatSystem.PlayerCharacter.OnCombatStart(); // 玩家角色开始战斗
            foreach (var monster in combatSystem.MonsterCharacter)
            {
                monster.OnCombatStart(); // 敌人角色开始战斗
            }

            StartPlayerTurn();
        }

        public void StartPlayerTurn()
        {
            whoseTurn = WhoseTurn.Player; // 玩家回合
            turnNum++;
            Debug.Log($"开始第 {turnNum} 回合, 玩家回合");
            combatSystem.EventManager.Publish(new TurnStartEvent(combatSystem.PlayerCharacter, turnNum));
            combatSystem.PlayerCharacter.OnTurnStart();
        }

        public void EndPlayerTurn()
        {
            // 结束玩家回合
            Debug.Log("玩家回合结束");
            turnState = TurnState.Ended; // 回合结束
            combatSystem.EventManager.Publish(new TurnEndEvent(combatSystem.PlayerCharacter, turnNum));
            combatSystem.PlayerCharacter.OnTurnEnd();
            StartEnemyTurn(); // 开始敌人回合
        }

        private void StartEnemyTurn()
        {
            whoseTurn = WhoseTurn.Enemy; // 敌人回合
            Debug.Log($"开始第 {turnNum} 回合, 敌人回合");
            foreach (var monster in combatSystem.MonsterCharacter)
            {
                combatSystem.EventManager.Publish(new TurnStartEvent(monster, turnNum));
            }

            foreach (var monster in combatSystem.MonsterCharacter)
            {
                monster.OnTurnStart();
            }

            foreach (var monster in combatSystem.MonsterCharacter)
            {
                monster.Effect.Work(monster, monster.Effect.EffectType == Characters.EnemyEffect.EnemyEffectType.Attack ? combatSystem.PlayerCharacter : monster); // 触发敌人效果
            }

            EndEnemyTurn(); // 结束敌人回合
        }

        private void EndEnemyTurn()
        {
            // 结束敌人回合
            Debug.Log("敌人回合结束");
            turnState = TurnState.Ended; // 回合结束
            foreach (var monster in combatSystem.MonsterCharacter)
            {
                combatSystem.EventManager.Publish(new TurnEndEvent(monster, turnNum));
            }

            foreach (var monster in combatSystem.MonsterCharacter)
            {
                monster.OnTurnEnd();
            }

            StartPlayerTurn(); // 开始玩家回合
        }

        internal void EndCombat()
        {
            // 结束战斗
            Debug.Log("战斗结束");
        }
    }
}