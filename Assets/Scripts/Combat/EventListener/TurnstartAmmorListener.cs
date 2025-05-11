using Combat.Command;
using Combat.Events.Turn;

namespace Combat.EventListener
{
    public class TurnstartAmmorListener : IEventListener
    {
        private CombatSystem combatSystem;

        public TurnstartAmmorListener(CombatSystem combatSystem)
        {
            this.combatSystem = combatSystem;
        }

        public void AddListen(EventManager eventManager)
        {
            eventManager.Subscribe<TurnStartEvent>(OnTurnStart);
        }

        public void RemoveListen(EventManager eventManager)
        {
            eventManager.Unsubscribe<TurnStartEvent>(OnTurnStart);
        }

        private void OnTurnStart(TurnStartEvent e)
        {
            combatSystem.ProcessCommand(new RemoveAmmorCommand(null, e.Character));
        }
    }
}