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
            if (Command.Source == null) {
                Debug.Log("[DamageDealtTrigger PostCheck] " + Command.Target.name + " 受到了" + Command.HPDamage + "点伤害，" + Command.AmmorDamage + "点护甲伤害。(无来源)");
            } else {
                Debug.Log("[DamageDealtTrigger PostCheck] " + Command.Source.name + " 对 " + Command.Target.name + " 造成了" + Command.HPDamage + "点伤害，" + Command.AmmorDamage + "点护甲伤害。");
            }
            if (Command.HPDamage > 0 || Command.AmmorDamage > 0) {
                var Event = new Events.DamageDealtEvent(Command.AmmorDamage, Command.HPDamage, Command.Type, Command.Source, Command.Target);
                manager.Publish(Event);
            }
        }

        public void PreCheck(EventManager manager, AttackCommand Command)
        {

        }
    }
}