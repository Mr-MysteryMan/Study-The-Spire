using Combat.Events;
using Combat.EventVariable;

namespace Combat.EventListener
{
    public class DeathListener : IEventListener
    {
        private CombatSystem combatSystem;

        public DeathListener(CombatSystem combatSystem) {
            this.combatSystem = combatSystem;
        }

        public void AddListen(EventManager eventManager)
        {
            eventManager.Subscribe<ValueChangedEvent<int>>(OnHpChanged);
        }

        public void RemoveListen(EventManager eventManager)
        {
            eventManager.Unsubscribe<ValueChangedEvent<int>>(OnHpChanged);
        }

        protected void OnHpChanged(ValueChangedEvent<int> eventData)
        {
            if (eventData.ValueName != "CurHp") return; // 只监听CurHp的变化
            if (eventData.Parent is Character character && character.CurHp <= 0) {
                character.BeforeDeath();
                combatSystem.KillCharacter(character); 
                combatSystem.EventManager.Publish(new CharacterDeathEvent(eventData.Parent as Character)); // 发布角色死亡事件
            }
        }
    }
}