using System;
using System.Collections.Generic;
using Combat.Processor;
using Combat.Trigger;
using Combat.Command;
using System.Linq;
using UnityEngine;

using ITrigger = Combat.Trigger.ITrigger;
using Combat.Processor.Rules;
using Combat.Characters;
using Combat.Events.Turn;
using Combat.Events;
using System.Collections;
using UnityEngine.Assertions;

namespace Combat
{
    // 战斗系统类，用于管理战斗中的所有角色和处理器
    // 该类负责注册和注销处理器和触发器，并处理命令的执行
    [RequireComponent(typeof(EventManager), typeof(Character))]
    public class CombatSystem : MonoBehaviour
    {
        public GameObject AdventurerPrefab; // 角色预制体
        public GameObject EnemyPrefab;
        public GameObject TreasurePrefab; // 宝物预制体
        public ObjectEventSO backToMenuEvent;

        public GameObject DamageTextPrefab; // 伤害文本预制体

        public Camera combatCamera; // 战斗摄像机

        public GameObject UI; // 战斗UI页面
        public GameObject uiPanel;

        // 来源角色的处理器列表
        private Dictionary<(Type, Character), SortedList<(int, long), IProcessor>> sourceProcessors;

        // 目标角色的处理器列表
        private Dictionary<(Type, Character), SortedList<(int, long), IProcessor>> targetProcessors;

        private Dictionary<Type, SortedList<(int, long), ITrigger>> triggers;

        private EventManager eventManager; // 事件管理器

        public EventManager EventManager => eventManager; // 事件管理器

        private CardManager cardManager; // 卡片管理器

        public CardManager CardManager => cardManager; // 卡片管理器

        // 系统角色，用于一些没有直接来源的命令，暂时没用
        private Character systemCharacter;

        // 角色列表，所有参与战斗的角色
        private Character playerCharacter;

        private List<Enemy> monsterCharacter;

        public Character PlayerCharacter => playerCharacter; // 玩家角色
        public List<Enemy> MonsterCharacter => monsterCharacter; // 怪物角色

        [SerializeField] private BasicRulesLibSO rulesLibSO;

        private EventListener.BasicRuleLib eventRulesLib;

        private TriggerLib triggerLib;

        void Awake()
        {
            CreateCharacters(); // 获取角色
            uiPanel.SetActive(false);
            var eventManager = GetComponent<EventManager>();
            var character = GetComponent<Character>();
            var cardManager = GetComponent<CardManager>();
            Initialize(eventManager, character, cardManager);

            eventRulesLib = new EventListener.BasicRuleLib(this);

            cardManager.init(this); // 初始化卡片管理器
        }


        /// <summary>
        /// 延后至这一帧结束后摧毁各种gameobject，否则会导致正在遍历敌人的时候，游戏结束被销毁，导致报错。
        /// 临时的解决方法，后续需要优化
        /// </summary>
        /// <returns></returns>
        private IEnumerator DestoryAfterDelay(int a)
        {
            yield return new WaitForEndOfFrame();
            Destroy(this.playerCharacter.gameObject);
            foreach (var monster in this.monsterCharacter)
            {
                Destroy(monster.gameObject); // 销毁怪物角色
            }
            this.monsterCharacter.Clear(); // 清空怪物角色列表
            Destroy(this.gameObject); // 销毁战斗系统
            if (a == 1)
            {
                Success();
            }
            else
            {
                Fail();
            }
        }

        private void Success()
        {
            // 弹出宝藏窗口
            var treasure = Instantiate(TreasurePrefab);
            treasure.GetComponent<Treasure>().init(this.playerCharacter.CurHp); // 设置宝物的生命值

            backToMenuEvent.RaiseEvent(null, this);
        }

        private void Fail()
        {
            uiPanel.SetActive(true);
        }

        void Start()
        {
            eventRulesLib.AddListen();
            this.eventManager.Subscribe<CombatLoseEvent>((e) =>
            {
                Debug.Log("游戏结束"); // TODO: 游戏结束
                StartCoroutine(DestoryAfterDelay(0));
            });
            this.eventManager.Subscribe<CombatWinningEvent>((e) =>
            {
                Debug.Log("游戏胜利"); // TODO: 游戏胜利
                StartCoroutine(DestoryAfterDelay(1));
            });
            this.eventManager.Subscribe<TurnEndEvent>((e) =>
            {
                if (e.Character == this.PlayerCharacter)
                {
                    cardManager.discardAll();
                }
            });
            this.eventManager.Subscribe<TurnStartEvent>((e) =>
            {
                if (e.Character == this.PlayerCharacter)
                {
                    cardManager.setEnergy(Setting.RoundEnergy); // 更新能量点
                    cardManager.drewCard();
                    Debug.Log("抽卡");
                }
            });
        }

        public void Initialize(EventManager eventManager, Character systemCharacter, CardManager cardManager)
        {
            this.eventManager = eventManager;
            this.systemCharacter = systemCharacter;
            this.cardManager = cardManager;
            this.cardManager.addCharacter(this.playerCharacter, this.monsterCharacter); // 添加角色到卡片管理器
            this.triggerLib = new TriggerLib();
            sourceProcessors = new();
            targetProcessors = new();

            triggers = new();
            foreach (var trigger in this.triggerLib.GetTriggers())
            {
                RegisterTrigger(trigger);
            }

            RegisterProcessorForCharacter(this.playerCharacter); // 注册玩家角色的处理器
            foreach (var character in this.monsterCharacter) // 注册怪物角色的处理器
            {
                RegisterProcessorForCharacter(character);
            }
        }

        private void CreateCharacters()
        {
            // 指定角色位置
            Vector3 playerPosition = new Vector3(-300, 20, 0); // 玩家位置
            Vector3 monsterPosition = new Vector3(300, 20, 0); // 敌人位置
            // 暂且1v1

            int curHp = 100; // TODO: 接入背包系统, 获取当前血量
            // 创建玩家角色
            playerCharacter = CreateCharacter(playerPosition);
            playerCharacter.SetInitHP(Setting.PlayerHp, curHp);
            Assert.IsTrue(playerCharacter != null && playerCharacter is Adventurer, "玩家角色创建失败！"); // 确保玩家角色创建成功
            (playerCharacter as Adventurer).SetInitMana(Setting.RoundEnergy); // 设置玩家角色的初始法力值
            // 创建怪物角色
            // TODO: 接入关卡管理, 获取敌人数据
            monsterCharacter = new List<Enemy>();
            for (int i = 0; i < 1; i++)
            {
                var monster = CreateCharacter(monsterPosition, CharacterType.Enemy) as Enemy;
                monsterCharacter.Add(monster);
            }
        }

        /* 处理角色的部分函数 */
        private enum CharacterType
        {
            Player,
            Enemy
        }

        private Character CreateCharacter(Vector3 position, CharacterType type = CharacterType.Player)
        {
            var Prefab = type == CharacterType.Player ? AdventurerPrefab : EnemyPrefab;
            // 根据位置坐标在UI上创建角色
            var Obj = Instantiate(Prefab, UI.transform);
            Obj.transform.localPosition = position;
            var character = Obj.GetComponent<Character>();
            character.combatSystem = this; // 设置战斗系统为自身
            return character;
        }

        public void KillCharacter(Character character)
        {
            if (character is Adventurer adventurer) // 如果是玩家角色
            {
                playerCharacter = null; // 玩家角色为空
            }
            else if (character is Enemy enemy) // 如果是怪物角色
            {
                monsterCharacter.Remove(enemy); // 从怪物列表中移除
            }
            Destroy(character.gameObject); // 销毁角色对象
        }

        /* 命令处理管道和触发器部分 */
        private void RegisterProcessorForCharacter(Character character)
        {
            foreach (var rule in rulesLibSO.GetRules())
            {
                if (rule is IProcessor processor)
                {
                    RegisterProcessor(processor, character);
                }
            }
        }

        private Dictionary<(Type, Character), SortedList<(int, long), IProcessor>> GetProcessorsByEffectSide(ProcessorEffectSideType type)
        {
            return type switch
            {
                ProcessorEffectSideType.Source => sourceProcessors,
                ProcessorEffectSideType.Target => targetProcessors,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }

        public IEnumerable<IProcessor<T>> GetProcessors<T>(Character character, ProcessorEffectSideType effectSide) where T : ICommand
        {
            var type = typeof(T);
            var processors = GetProcessorsByEffectSide(effectSide);
            if (!processors.TryGetValue((type, character), out var list))
            {
                return Enumerable.Empty<IProcessor<T>>();
            }
            return list.Values.OfType<IProcessor<T>>().ToList();
        }

        public IEnumerable<ITrigger<T>> GetTrigger<T>() where T : ICommand
        {
            var type = typeof(T);
            if (!triggers.TryGetValue(type, out var list))
            {
                return Enumerable.Empty<ITrigger<T>>();
            }
            return list.Values.OfType<ITrigger<T>>();
        }

        public void RegisterProcessor<T>(IProcessor<T> processor, Character character) where T : ICommand
        {
            var type = typeof(T);
            var processors = GetProcessorsByEffectSide(processor.EffectSide);
            if (!processors.TryGetValue((type, character), out var list))
            {
                list = new SortedList<(int, long), IProcessor>();
                processors[(type, character)] = list;
            }
            if (list.ContainsKey((processor.Priority, processor.TimeStamp)))
            {
                Debug.LogError($"Processor {processor} already registered for {type} on {character}.");
                return;
            }
            list.Add((processor.Priority, processor.TimeStamp), processor);
        }

        public void RegisterProcessor(IProcessor processor, Character character)
        {
            var type = processor.CommandType;
            var processors = GetProcessorsByEffectSide(processor.EffectSide);
            if (!processors.TryGetValue((type, character), out var list))
            {
                list = new SortedList<(int, long), IProcessor>();
                processors[(type, character)] = list;
            }
            if (list.ContainsKey((processor.Priority, processor.TimeStamp)))
            {
                Debug.LogError($"Processor {processor} already registered for {type} on {character}.");
                return;
            }
            list.Add((processor.Priority, processor.TimeStamp), processor);
        }

        public void UnregisterProcessor<T>(IProcessor<T> processor, Character character, ProcessorEffectSideType effectSide) where T : ICommand
        {
            var type = typeof(T);
            var processors = GetProcessorsByEffectSide(effectSide);
            if (!processors.TryGetValue((type, character), out var list))
            {
                return;
            }
            list.Remove((processor.Priority, processor.TimeStamp));
        }

        public void UnregisterProcessor(IProcessor processor, Character character, ProcessorEffectSideType effectSide)
        {
            var type = processor.CommandType;
            var processors = GetProcessorsByEffectSide(effectSide);
            if (!processors.TryGetValue((type, character), out var list))
            {
                return;
            }
            list.Remove((processor.Priority, processor.TimeStamp));
        }

        public void RegisterTrigger(ITrigger trigger)
        {
            var type = trigger.CommandType;
            if (!triggers.TryGetValue(type, out var list))
            {
                list = new SortedList<(int, long), ITrigger>();
                triggers[type] = list;
            }
            if (list.ContainsKey((trigger.Priority, trigger.TimeStamp)))
            {
                Debug.LogError($"Trigger {trigger} already registered for {type}.");
                return;
            }
            list.Add((trigger.Priority, trigger.TimeStamp), trigger);
        }

        public void RegisterTrigger<T>(ITrigger<T> trigger) where T : ICommand
        {
            var type = typeof(T);
            if (!triggers.TryGetValue(type, out var list))
            {
                list = new();
                triggers[type] = list;
            }
            if (list.ContainsKey((trigger.Priority, trigger.TimeStamp)))
            {
                Debug.LogError($"Trigger {trigger} already registered for {type}.");
                return;
            }
            list.Add((trigger.Priority, trigger.TimeStamp), trigger);
        }

        public void UnregisterTrigger<T>(ITrigger<T> trigger) where T : ICommand
        {
            var type = typeof(T);
            if (!triggers.TryGetValue(type, out var list))
            {
                return;
            }
            list.Remove((trigger.Priority, trigger.TimeStamp));
        }

        public void ProcessCommand<T>(T command) where T : ICommand
        {
            foreach (var processor in GetProcessors<T>(command.Source, ProcessorEffectSideType.Source))
            {
                processor.Process(ref command);
            }

            foreach (var processor in GetProcessors<T>(command.Target, ProcessorEffectSideType.Target))
            {
                processor.Process(ref command);
            }

            foreach (var trigger in GetTrigger<T>())
            {
                trigger.PreCheck(eventManager, command);
            }

            command.Execute();

            foreach (var trigger in GetTrigger<T>())
            {
                trigger.PostCheck(eventManager, command);
            }
        }
    }
}