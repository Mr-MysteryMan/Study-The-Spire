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

namespace Combat
{
    [RequireComponent(typeof(EventManager))]
    public class CombatSystem : MonoBehaviour
    {

        private Dictionary<(Type, Character), SortedList<(int, long), IProcessor>> processors;

        private Dictionary<Type, SortedList<(int, long), ITrigger>> triggers;

        private EventManager eventManager;

        public void Initialize(EventManager eventManager) {
            this.eventManager = eventManager;
            processors = new Dictionary<(Type, Character), SortedList<(int, long), IProcessor>>();
            triggers = new Dictionary<Type, SortedList<(int, long), ITrigger>>();
        }

        void Awake()
        {
            var eventManager = GetComponent<EventManager>();
            Initialize(eventManager);
        }

        public IEnumerable<IProcessor<T>> GetProcessors<T>(Character character) where T: ICommand {
            var type = typeof(T);
            if (!processors.TryGetValue((type, character), out var list)) {
                return Enumerable.Empty<IProcessor<T>>();
            }
            return list.Values.OfType<IProcessor<T>>();
        }

        public IEnumerable<ITrigger<T>> GetTrigger<T>() where T: ICommand {
            var type = typeof(T);
            if (!triggers.TryGetValue(type, out var list)) {
                return Enumerable.Empty<ITrigger<T>>();
            }
            return list.OfType<ITrigger<T>>();
        }

        public void RegisterProcessor<T>(IProcessor<T> processor, Character character) where T: ICommand {
            var type = typeof(T);
            if (!processors.TryGetValue((type, character), out var list)) {
                list = new SortedList<(int, long), IProcessor>();
                processors[(type, character)] = list;
            }
            list.Add((processor.Priority, processor.TimeStamp), processor);
        }

        public void UnregisterProcessor<T>(IProcessor<T> processor, Character character) where T: ICommand {
            var type = typeof(T);
            if (!processors.TryGetValue((type, character), out var list)) {
                return;
            }
            list.Remove((processor.Priority, processor.TimeStamp));
        }

        public void RegisterTrigger<T>(ITrigger<T> trigger) where T: ICommand {
            var type = typeof(T);
            if (!triggers.TryGetValue(type, out var list)) {
                list = new ();
                triggers[type] = list;
            }
            list.Add((trigger.Priority, trigger.TimeStamp), trigger);
        }

        public void UnregisterTrigger<T>(ITrigger<T> trigger) where T: ICommand {
            var type = typeof(T);
            if (!triggers.TryGetValue(type, out var list)) {
                return;
            }
            list.Remove((trigger.Priority, trigger.TimeStamp));
        }

        public void ProcessCommand<T>(T command) where T: ICommand  {
            foreach (var processor in GetProcessors<T>(command.Source)) {
                processor.Process(ref command);
            }

            foreach (var processor in GetProcessors<T>(command.Target)) {
                processor.Process(ref command);
            }

            foreach (var trigger in GetTrigger<T>()) {
                trigger.PreCheck(eventManager, command);
            }

            command.Execute();

            foreach (var trigger in GetTrigger<T>()) {
                trigger.PostCheck(eventManager, command);
            }
        }
    }
}