using System;
using Combat.Command;
using UnityEngine;

namespace Combat.Trigger
{
    public class HealDealtTrigger : ITrigger<HealCommand>
    {
        public Type CommandType => typeof(HealCommand);

        public int Priority => 0;

        public int TimeStamp => 0;

        public void PostCheck(EventManager manager, HealCommand Command)
        {
            if (Command.Source == null)
            {
                Debug.Log("[HealDealtTrigger PostCheck] " + Command.Target.name + " 回复了" + Command.HealAmount + "点生命值。(无来源)");
            }
            else
            {
                Debug.Log("[HealDealtTrigger PostCheck] " + Command.Source.name + " 对 " + Command.Target.name + " 回复了" + Command.HealAmount + "点生命值。");
            }

            if (Command.HealAmount > 0)
            {
                var Event = new Events.HealDealtEvent(Command.HealAmount, Command.Source, Command.Target);
                manager.Publish(Event);
            }
        }

        public void PreCheck(EventManager manager, HealCommand Command)
        {
        }
    }
}