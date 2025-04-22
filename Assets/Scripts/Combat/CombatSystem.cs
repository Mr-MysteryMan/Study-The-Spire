using System;
using System.Collections.Generic;
using Combat.Processor;
using Combat.Trigger;
using Combat.Events;
using System.Collections;
using Combat.Command;
using System.Linq;
using UnityEngine;

using ITrigger = Combat.Trigger.ITrigger;
using Combat.Processor.Rules;

namespace Combat
{
    // 战斗系统类，用于管理战斗中的所有角色和处理器
    // 该类负责注册和注销处理器和触发器，并处理命令的执行
    [RequireComponent(typeof(EventManager), typeof(Character))]
    public class CombatSystem : MonoBehaviour
    {

        // 来源角色的处理器列表
        private Dictionary<(Type, Character), SortedList<(int, long), IProcessor>> sourceProcessors;

        // 目标角色的处理器列表
        private Dictionary<(Type, Character), SortedList<(int, long), IProcessor>> targetProcessors;

        private Dictionary<Type, SortedList<(int, long), ITrigger>> triggers;

        private EventManager eventManager;

        // 系统角色，用于一些没有直接来源的命令，暂时没用
        private Character systemCharacter;

        // 角色列表，所有参与战斗的角色
        public List<Character> characters;

        [SerializeField] private BasicRulesLibSO rulesLibSO;

        public void Initialize(EventManager eventManager, Character systemCharacter)
        {
            this.eventManager = eventManager;
            this.systemCharacter = systemCharacter;
            sourceProcessors = new();
            targetProcessors = new();
            triggers = new();
            RegisterTrigger(new DamageDealtTrigger());

            foreach (var character in characters)
            {
                foreach (var rule in rulesLibSO.Rules)
                {
                    if (rule is IProcessor processor)
                    {
                        RegisterProcessor(processor, character);
                    }
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

        void Awake()
        {
            var eventManager = GetComponent<EventManager>();
            var character = GetComponent<Character>();
            Initialize(eventManager, character);
        }

        public IEnumerable<IProcessor<T>> GetProcessors<T>(Character character, ProcessorEffectSideType effectSide) where T : ICommand
        {
            var type = typeof(T);
            var processors = GetProcessorsByEffectSide(effectSide);
            if (!processors.TryGetValue((type, character), out var list))
            {
                return Enumerable.Empty<IProcessor<T>>();
            }
            return list.Values.OfType<IProcessor<T>>();
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