using System.Collections.Generic;

namespace Combat.EventListener {
    public class BasicRuleLib {
        public List<IEventListener> EventListeners { get; } = new List<IEventListener>();

        private EventManager eventManager;

        public BasicRuleLib(CombatSystem combatSystem) {
            this.eventManager = combatSystem.EventManager;
            EventListeners.Add(new DeathListener(combatSystem));
            EventListeners.Add(new GameoverListener(combatSystem));
        } 

        public void AddListen() {
            foreach (var listener in EventListeners) {
                listener.AddListen(eventManager);
            }
        } 

        public void RemoveListen() {
            foreach (var listener in EventListeners) {
                listener.RemoveListen(eventManager);
            }
        }
    }
}