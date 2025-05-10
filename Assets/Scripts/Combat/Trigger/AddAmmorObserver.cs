using Combat.Command;
using UnityEngine;

namespace Combat.Trigger
{
    public class AddAmmorTrigger : ITrigger<AddAmmorCommand>
    {
        public System.Type CommandType => typeof(AddAmmorCommand);

        public int Priority => 0;

        public int TimeStamp => 0;

        public void PostCheck(EventManager manager, AddAmmorCommand Command)
        {
            if (Command.Source == null)
            {
                Debug.Log("[AddAmmorTrigger PostCheck] " + Command.Target.name + " 获得了" + Command.AmmorAmount + "点护甲。(无来源)");
            }
            else
            {
                Debug.Log("[AddAmmorTrigger PostCheck] " + Command.Source.name + " 给 " + Command.Target.name + " 增加了" + Command.AmmorAmount + "点护甲。");
            }

            if (Command.AmmorAmount > 0)
            {
                var Event = new Events.AddAmmorEvent(Command.AmmorAmount, Command.Source, Command.Target);
                manager.Publish(Event);
            }
        }

        public void PreCheck(EventManager manager, AddAmmorCommand Command)
        {
        }
    }
}