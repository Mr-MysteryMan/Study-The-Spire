using System.Linq;
using Combat.Characters;
using Combat.Events;

namespace Combat.EventListener
{
    public class GameoverListener : IEventListener
    {
        private CombatSystem combatSystem;

        public GameoverListener(CombatSystem combatSystem) {
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

        public void OnCharacterDeath(CharacterDeathEvent deathEvent) {
            if (deathEvent.Victim is Adventurer){
                if (combatSystem.PlayerCharacter == deathEvent.Victim) {
                    // 玩家角色死亡，游戏结束
                    combatSystem.EventManager.Publish(new CombatLoseEvent());
                }
            } else if (deathEvent.Victim is Enemy) {
                if (combatSystem.MonsterCharacter.Count == 0) {
                    combatSystem.EventManager.Publish(new CombatWinningEvent());
                }
            }
        }
    }
}