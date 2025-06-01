using System.Linq;
using Combat.Characters;
using Combat.Events;

namespace Combat.EventListener
{
    public class GameoverListener : IEventListener
    {
        private CombatSystem combatSystem;

        public GameoverListener(CombatSystem combatSystem)
        {
            this.combatSystem = combatSystem;
        }

        public void AddListen(EventManager eventManager)
        {
            eventManager.Subscribe<CharacterDeathEvent>(OnCharacterDeath);
        }

        public void RemoveListen(EventManager eventManager)
        {
            eventManager.Unsubscribe<CharacterDeathEvent>(OnCharacterDeath);
        }

        public void OnCharacterDeath(CharacterDeathEvent deathEvent)
        {
            if (combatSystem.characterManager.IsLose())
            {
                combatSystem.EventManager.Publish(new CombatLoseEvent());
            }
            else if (combatSystem.characterManager.IsWin())
            {
                combatSystem.EventManager.Publish(new CombatWinningEvent());
            }
        }
    }
}