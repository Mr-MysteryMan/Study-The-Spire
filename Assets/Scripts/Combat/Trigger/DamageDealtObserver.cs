using Combat.Command;
using UnityEngine;

namespace Combat.Trigger {
    // 造成伤害的处理器，用于处理造成伤害的事件
    // 该处理器会在造成伤害后触发，并发送一个DamageDealtEvent事件
    public class DamageDealtTrigger : ITrigger<AttackCommand>
    {
        public System.Type CommandType => typeof(AttackCommand);

        public int Priority => 0;

        public int TimeStamp => 0;

        public void PostCheck(EventManager manager, AttackCommand Command)
        {
            Debug.Log("DamageDealtTrigger PreCheck: " + Command.Source.name + " dealt " + Command.HPDamage + " HP damage and " + Command.AmmorDamage + " Ammor damage to " + Command.Target.name);
            if (Command.HPDamage > 0 || Command.AmmorDamage > 0) {
                Debug.Log("DamageDealtTrigger PreCheck: " + Command.Source.name + " dealt " + Command.HPDamage + " HP damage and " + Command.AmmorDamage + " Ammor damage to " + Command.Target.name);
                var Event = new Events.DamageDealtEvent(Command.AmmorDamage, Command.HPDamage, Command.Type, Command.Source, Command.Target);
                manager.Publish(Event);
            }
        }

        public void PreCheck(EventManager manager, AttackCommand Command)
        {

        }
    }
}