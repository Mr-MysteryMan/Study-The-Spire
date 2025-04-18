using System;
using System.Collections.Generic;
using Combat.Processor;
using Combat.Observer;
using Combat.Events;
using System.Collections;
using Combat.Command;


namespace Combat
{
    public class CombatSystem
    {
        private Dictionary<Type, List<IProcessor>> processors = new Dictionary<Type, List<IProcessor>>();
        private Dictionary<Type, List<Observer.IObserver<ICommand>>> observers = new Dictionary<Type, List<Observer.IObserver<ICommand>>>();

        public void ProcessCommand<T>(T command) where T: ICommand  {
            foreach (var processor in GetProcessors<T>(command.Source)) {
                processor.Process(ref command);
            }

            foreach (var processor in GetProcessors<T>(command.Target)) {
                processor.Process(ref command);
            }

            foreach (var observer in GetObserver<T>()) {
                observer.PreCheck(eventManager, command);
            }

            command.Execute();

            foreach (var observer in GetObserver<T>()) {
                observer.PostCheck(eventManager, command);
            }
        }
    }
}