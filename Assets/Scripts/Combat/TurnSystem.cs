using UnityEngine;
using Combat.Events.Turn;
using System.Collections;
using System.Collections.Generic;
using Combat.Characters;
using System.Linq;
using System;
using Combat.Characters.EnemyEffect;

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

        public WhoseTurn WhoseTurn => whoseTurn; // 当前回合的操作者

        public TurnEndButtonController button;

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

            var MonsterCharacters = combatSystem.MonsterCharacters.Where(m => !m.IsDead).ToList();
            foreach (var monster in combatSystem.MonsterCharacters)
            {
                if (monster.IsDead) continue; // 如果怪物已经死亡，则跳过
                monster.OnCombatStart(); // 敌人角色开始战斗
            }
            StartPlayerTurn();
        }


        public void StartPlayerTurn()
        {
            whoseTurn = WhoseTurn.Player; // 玩家回合
            turnState = TurnState.Started;
            button.UpdateButtonState(); // 更新按钮状态
            turnNum++;
            Debug.Log($"开始第 {turnNum} 回合, 玩家回合");
            combatSystem.EventManager.Publish(new TurnStartEvent(combatSystem.PlayerCharacter, turnNum));
            combatSystem.PlayerCharacter.OnTurnStart();
        }

        /// <summary>
        /// 结束玩家回合并再开始一个新的玩家回合
        /// 即跳过敌人回合，获得一个新的回合
        /// </summary>
        public void EndAndStartNewPlayerTurn()
        {
            _EndPlayerTurn();
            StartPlayerTurn();
        }

        // 仅结束玩家回合
        private void _EndPlayerTurn()
        {
            if (turnState != TurnState.Started || whoseTurn != WhoseTurn.Player)
            {
                Debug.LogWarning("当前回合未开始或已结束，无法结束玩家回合");
                return;
            }
            // 结束玩家回合
            Debug.Log("玩家回合结束");
            turnState = TurnState.Ended; // 回合结束
            combatSystem.EventManager.Publish(new TurnEndEvent(combatSystem.PlayerCharacter, turnNum));
            combatSystem.PlayerCharacter.OnTurnEnd();
        }

        public void EndPlayerTurn()
        {
            _EndPlayerTurn(); // 结束玩家回合
            StartCoroutine(StartEnemyTurn()); // 开始敌人回合
        }

        private IEnumerator StartEnemyTurn()
        {
            whoseTurn = WhoseTurn.Enemy; // 敌人回合
            turnState = TurnState.Started; // 回合开始
            Debug.Log($"开始第 {turnNum} 回合, 敌人回合");

            // 复制怪物列表，避免在迭代时修改集合导致异常
            var Monsters = combatSystem.MonsterCharacters.Where(m => !m.IsDead).ToList();
            foreach (var monster in Monsters)
            {
                if (monster.IsDead) continue; // 如果怪物已经死亡，则跳过
                combatSystem.EventManager.Publish(new TurnStartEvent(monster, turnNum));
            }

            foreach (var monster in Monsters)
            {
                if (monster.IsDead) continue;
                monster.OnTurnStart();
            }

            foreach (var monster in Monsters)
            {
                if (monster.IsDead) continue;
                yield return monster.Effect.Work(monster, GetTargets(monster.Effect.TargetType, monster)); // 触发敌人效果
            }

            EndEnemyTurn(); // 结束敌人回合
        }

        /// <summary>
        /// 根据效果目标类型获取敌人效果的目标列表
        /// </summary>
        /// <param name="effectTarget">卡牌生效目标类型</param>
        /// <param name="characterSelf">效果发起者</param>
        /// <returns>返回生效的目标列表</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private List<Character> GetTargets(CardEffectTarget effectTarget, Character characterSelf)
        {
            return effectTarget switch
            {
                CardEffectTarget.None => new List<Character>(),
                CardEffectTarget.AllyOne => new List<Character> { characterSelf },
                CardEffectTarget.AllyAll => combatSystem.MonsterCharacters.Select(m => m as Character).ToList(),
                CardEffectTarget.AllySelf => new List<Character> { characterSelf },

                CardEffectTarget.EnemyOne => new List<Character> { combatSystem.PlayerCharacter },
                CardEffectTarget.EnemyAll => new List<Character> { combatSystem.PlayerCharacter },
                CardEffectTarget.NotPlayable => new List<Character>(),

                CardEffectTarget.CharacterOne => new List<Character> { characterSelf },
                CardEffectTarget.CharacterAll => combatSystem.AllCharacters,
                _ => throw new ArgumentOutOfRangeException(nameof(effectTarget), effectTarget, null)
            };
        }

        private void EndEnemyTurn()
        {
            // 结束敌人回合
            Debug.Log("敌人回合结束");
            turnState = TurnState.Ended; // 回合结束

            var Monsters = combatSystem.MonsterCharacters.Where(m => !m.IsDead).ToList();
            foreach (var monster in Monsters)
            {
                if (monster.IsDead) continue;
                combatSystem.EventManager.Publish(new TurnEndEvent(monster, turnNum));
            }

            foreach (var monster in Monsters)
            {
                if (monster.IsDead) continue;
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