using Combat.Command;

namespace Combat.Trigger {
    public class DamageDealtTrigger : ITrigger<AttackCommand>
    {
        public System.Type CommandType => typeof(AttackCommand);

        public int Priority => 0;

        public int TimeStamp => 0;

        public void PostCheck(EventManager manager, AttackCommand Command)
        {
            // 不做任何事情
        }

        public void PreCheck(EventManager manager, AttackCommand Command)
        {
            if (Command.HPDamage > 0 || Command.AmmorDamage > 0) {
                var Event = new Events.DamageDealtEvent(Command.AmmorDamage, Command.HPDamage, Command.Type, Command.Source, Command.Target);
                manager.Publish(Event);
            }
        }
    }
}