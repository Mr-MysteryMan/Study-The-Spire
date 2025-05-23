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

namespace Combat
{
    // 战斗系统类，用于管理战斗中的所有角色和处理器
    // 该类负责注册和注销处理器和触发器，并处理命令的执行
    [RequireComponent(typeof(EventManager), typeof(Character))]
    public class CombatSystem : MonoBehaviour
    {
        public GameObject AdventurerPrefab; // 角色预制体
        public GameObject EnemyPrefab;

        public GameObject UI; // 战斗UI页面

        // 来源角色的处理器列表
        private Dictionary<(Type, Character), SortedList<(int, long), IProcessor>> sourceProcessors;

        // 目标角色的处理器列表
        private Dictionary<(Type, Character), SortedList<(int, long), IProcessor>> targetProcessors;

        private Dictionary<Type, SortedList<(int, long), ITrigger>> triggers;

        private EventManager eventManager; // 事件管理器

        public EventManager EventManager => eventManager; // 事件管理器

        private CardManager cardManager; // 卡片管理器

        // 系统角色，用于一些没有直接来源的命令，暂时没用
        private Character systemCharacter;

        // 角色列表，所有参与战斗的角色
        private Character playerCharacter;

        private List<Enemy> monsterCharacter;

        public Character PlayerCharacter => playerCharacter; // 玩家角色
        public List<Enemy> MonsterCharacter => monsterCharacter; // 怪物角色

        [SerializeField] private BasicRulesLibSO rulesLibSO;

        void Awake()
        {
            CreateCharacters(); // 获取角色

            var eventManager = GetComponent<EventManager>();
            var character = GetComponent<Character>();
            var cardManager = GetComponent<CardManager>();
            Initialize(eventManager, character, cardManager);

            cardManager.init(); // 初始化卡片管理器
            cardManager.drewCard(); // 抽卡
        }

        void Start()
        {
            this.eventManager.Subscribe<TurnEndEvent>((e) => { 
                if (e.Character == this.PlayerCharacter) {
                    cardManager.discardAll();
                    checkGameOver(); // 检查游戏结束条件
                }
            });
            this.eventManager.Subscribe<TurnStartEvent>((e) => { 
                if (e.Character == this.PlayerCharacter) {
                    cardManager.setEnergy(Setting.RoundEnergy); // 更新能量点
                    cardManager.drewCard(); Debug.Log("抽卡"); 
                } 
            });
        }


        // 游戏结束条件判断
        private void checkGameOver() {
            if (!(this.PlayerCharacter.CurHp > 0)) {
                // TODO : 游戏结束
                Debug.Log("游戏结束");
            } else if (this.MonsterCharacter.All(x => x.CurHp <= 0)) {
                // TODO : 胜利
                Debug.Log("胜利");
            }
        }

        public void Initialize(EventManager eventManager, Character systemCharacter, CardManager cardManager)
        {
            this.eventManager = eventManager;
            this.systemCharacter = systemCharacter;
            this.cardManager = cardManager;
            this.cardManager.addCharacter(this.playerCharacter, this.monsterCharacter); // 添加角色到卡片管理器
            sourceProcessors = new();
            targetProcessors = new();
            triggers = new();
            RegisterTrigger(new DamageDealtTrigger());
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
            playerCharacter.SetHP(Setting.PlayerHp, curHp);
            // 创建怪物角色
            // TODO: 接入关卡管理, 获取敌人数据
            monsterCharacter = new List<Enemy>();
            for (int i = 0; i < 1; i++)
            {
                var monster = CreateCharacter(monsterPosition, CharacterType.Enemy) as Enemy;
                monsterCharacter.Add(monster);
            }
        }

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

        private void RegisterProcessorForCharacter(Character character)
        {
            foreach (var rule in rulesLibSO.Rules)
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

        public void RegisterTrigger<T>(ITrigger<T> trigger) where T : ICommand
        {
            var type = typeof(T);
            if (!triggers.TryGetValue(type, out var list))
            {
                list = new();
                triggers[type] = list;
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