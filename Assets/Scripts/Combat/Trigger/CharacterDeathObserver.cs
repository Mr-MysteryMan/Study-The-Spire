using Type = System.Type;
using Combat.Command;

namespace Combat.Trigger {
    public class CharacterDeathTrigger : ITrigger<AttackCommand> {
        public Type CommandType => typeof(AttackCommand);
        public int Priority => 100;
        public int TimeStamp => 0;

        public void PreCheck(EventManager manager, AttackCommand command) {
            // Do nothing
        }

        public void PostCheck(EventManager manager, AttackCommand command) {
            if (command.Target.CurHp <= 0) {
                var eventArgs = new Events.CharacterDeathEvent(command.Target, command.Source);
                manager.Publish(eventArgs);
            }
        }
    }
}