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
using GlobalCardManager = CardManager;
using static Combat.Characters.EnemyLib;

namespace Combat
{
    // 战斗系统类，用于管理战斗中的所有角色和处理器
    // 该类负责注册和注销处理器和触发器，并处理命令的执行
    [RequireComponent(typeof(EventManager), typeof(Character))]
    public class CombatSystem : MonoBehaviour
    {
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

        public CardManager cardManager; // 卡片管理器

        public CardManager CardManager => cardManager; // 卡片管理器

        public CharacterManager characterManager;
        public Character systemCharacter; // 系统角色，用于一些没有直接来源的命令

        public TurnSystem turnSystem; // 回合系统

        [SerializeField] private BasicRulesLibSO rulesLibSO;

        private EventListener.BasicRuleLib eventRulesLib;

        private TriggerLib triggerLib;

        public EnemyType DefaultEnemyType;

        private float enemyMargin = 120; // 怪物的间隔

        public Character PlayerCharacter => characterManager.PlayerCharacter; // 玩家角色
        public List<Enemy> MonsterCharacters => characterManager.MonsterCharacters; // 怪物角色列表
        public List<Character> AllCharacters => characterManager.AllCharacters; // 所有角色列表

        void Awake()
        {
            Initialize();
            InitCharacterManager();
            turnSystem.InitTurnSystem();
            uiPanel.SetActive(false);

            eventRulesLib = new EventListener.BasicRuleLib(this);

            cardManager.init(this, PlayerCharacter); // 初始化卡片管理器

            InitBasicEvent();

            turnSystem.StartCombat(); // 开始战斗
        }

        private void InitCharacterManager()
        {
            var globalCardManagerObject = GameObject.Find("CardManager(Clone)");
            var globalCardManager = globalCardManagerObject == null ? null : globalCardManagerObject.GetComponent<GlobalCardManager>();
            List<CharacterInfo> characterInfos;
            int hp, maxHp;
            EnemyType enemyType;
            if (globalCardManager == null)
            {
                characterInfos = new List<CharacterInfo>() { CharacterInfo.Create(CharacterType.Mage), CharacterInfo.Create(CharacterType.Warrior) }; // 初始化角色信息
                hp = Setting.PlayerHp; // 设置玩家初始血量
                maxHp = Setting.PlayerHp; // 设置玩家最大血量
                enemyType = DefaultEnemyType;
            }
            else
            {
                characterInfos = globalCardManager.characterTypes; // 从全局卡片管理器获取角色类型
                enemyType = GameObject.Find("SceneLoadManager").GetComponent<SceneLoadManager>().currentRoom.roomData.roomType switch
                {
                    RoomType.MinorEnemy => EnemyType.Minor,
                    RoomType.EliteEnemy => EnemyType.Elite,
                    RoomType.BossRoom => EnemyType.Boss,
                    _ => DefaultEnemyType // 默认怪物类型
                };
            }

            characterManager.Init(this.systemCharacter, characterInfos, enemyType);
        }


        /// <summary>
        /// 延后至这一帧结束后摧毁各种gameobject，否则会导致正在遍历敌人的时候，游戏结束被销毁，导致报错。
        /// 临时的解决方法，后续需要优化
        /// </summary>
        /// <returns></returns>
        private IEnumerator DestoryAfterDelay(int a)
        {
            yield return new WaitForEndOfFrame();
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
            treasure.GetComponent<Treasure>().init(characterManager.GetPlayerCharacterInfos(), () => backToMenuEvent.RaiseEvent(null, this)); // 设置宝物的生命值
        }

        private void Fail()
        {
            uiPanel.SetActive(true);
        }

        private void InitBasicEvent()
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

        public void Initialize()
        {
            this.triggerLib = new TriggerLib();
            sourceProcessors = new();
            targetProcessors = new();

            this.eventManager = GetComponent<EventManager>();
            this.eventManager.Initialize();

            triggers = new();
            foreach (var trigger in this.triggerLib.GetTriggers())
            {
                RegisterTrigger(trigger);
            }
        }

        public void KillCharacter(Character character)
        {
            this.characterManager.KillCharacter(character);
        }

        public void LockDead()
        {
            this.characterManager.LockDead();
        }

        public void ReleaseDead()
        {
            this.characterManager.ReleaseDead();
        }

        /* 命令处理管道和触发器部分 */
        public void RegisterProcessorForCharacter(Character character)
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
            ProcessOnly(ref command);

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

        public void ProcessCommand<T>(ref T command) where T : ICommand
        {
            ProcessOnly(ref command);

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

        /// <summary>
        /// 处理命令的管道，遍历所有处理器并执行，可以用于尝试获取处理后的结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="command"></param>
        public void ProcessOnly<T>(ref T command) where T : ICommand
        {
            foreach (var processor in GetProcessors<T>(command.Source, ProcessorEffectSideType.Source))
            {
                processor.Process(ref command);
            }

            foreach (var processor in GetProcessors<T>(command.Target, ProcessorEffectSideType.Target))
            {
                processor.Process(ref command);
            }
        }
    }
}