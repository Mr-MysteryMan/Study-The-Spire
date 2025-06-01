using UnityEngine;
using Combat.Events.Turn;
using System.Collections;
using System.Collections.Generic;
using Combat.Characters;
using System.Linq;
using System;

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

        public void InitTurnSystem()
        {
            turnNum = 0;
            whoseTurn = WhoseTurn.Player; // 默认玩家先手
            turnState = TurnState.Started; // 默认回合开始
            Debug.Log("回合系统初始化完成");
        }

        public void StartCombat()
        {
            // 开始战斗
            Debug.Log("战斗开始");
            combatSystem.EventManager.Publish(new CombatStartEvent());
            combatSystem.PlayerCharacter.OnCombatStart(); // 玩家角色开始战斗
            combatSystem.LockDead();
            foreach (var monster in combatSystem.MonsterCharacters)
            {
                if (monster.IsDead) continue; // 如果怪物已经死亡，则跳过
                monster.OnCombatStart(); // 敌人角色开始战斗
            }
            combatSystem.ReleaseDead();
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
            StartCoroutine(StartEnemyTurn()); // 开始敌人回合
        }

        private IEnumerator StartEnemyTurn()
        {
            whoseTurn = WhoseTurn.Enemy; // 敌人回合
            Debug.Log($"开始第 {turnNum} 回合, 敌人回合");
            combatSystem.LockDead();
            foreach (var monster in combatSystem.MonsterCharacters)
            {
                if (monster.IsDead) continue; // 如果怪物已经死亡，则跳过
                combatSystem.EventManager.Publish(new TurnStartEvent(monster, turnNum));
            }
            combatSystem.ReleaseDead();

            combatSystem.LockDead();
            foreach (var monster in combatSystem.MonsterCharacters)
            {
                if (monster.IsDead) continue;
                monster.OnTurnStart();
            }
            combatSystem.ReleaseDead();

            combatSystem.LockDead();
            foreach (var monster in combatSystem.MonsterCharacters)
            {
                if (monster.IsDead) continue;
                yield return monster.Effect.Work(monster, GetTargets(monster.Effect.TargetType, monster)); // 触发敌人效果
            }
            combatSystem.ReleaseDead();

            EndEnemyTurn(); // 结束敌人回合
        }

        private List<Character> GetTargets(CardEffectTarget effectTarget, Character character)
        {
            return effectTarget switch
            {
                CardEffectTarget.None => new List<Character>(),
                CardEffectTarget.AdventurerOne => new List<Character> { combatSystem.PlayerCharacter },
                CardEffectTarget.AdventurerAll => new List<Character> { combatSystem.PlayerCharacter },
                CardEffectTarget.AdventurerSelf => new List<Character> { combatSystem.PlayerCharacter },

                CardEffectTarget.EnemyOne => new List<Character> { character },
                CardEffectTarget.EnemyAll => combatSystem.MonsterCharacters.Select(m => m as Character).ToList(),
                CardEffectTarget.NotPlayable => new List<Character>(),

                CardEffectTarget.CharacterOne => new List<Character> { character },
                CardEffectTarget.CharacterAll => combatSystem.AllCharacters,
                _ => throw new ArgumentOutOfRangeException(nameof(effectTarget), effectTarget, null)
            };
        }

        private void EndEnemyTurn()
        {
            // 结束敌人回合
            Debug.Log("敌人回合结束");
            turnState = TurnState.Ended; // 回合结束
            combatSystem.LockDead();
            foreach (var monster in combatSystem.MonsterCharacters)
            {
                if (monster.IsDead) continue; 
                combatSystem.EventManager.Publish(new TurnEndEvent(monster, turnNum));
            }
            combatSystem.ReleaseDead();

            combatSystem.LockDead();
            foreach (var monster in combatSystem.MonsterCharacters)
            {
                if (monster.IsDead) continue;
                monster.OnTurnEnd();
            }
            combatSystem.ReleaseDead();

            StartPlayerTurn(); // 开始玩家回合
        }

        internal void EndCombat()
        {
            // 结束战斗
            Debug.Log("战斗结束");
        }
    }
}