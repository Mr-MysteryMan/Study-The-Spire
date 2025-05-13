using System;
using Combat.Command.Buff;
using Combat.Events;
using UnityEngine;

namespace Combat.Trigger
{
    public class UpdateBuffTrigger : ITrigger<UpdateBuffCountCommand>
    {
        public Type CommandType => typeof(UpdateBuffCountCommand);

        public int Priority => 100;

        public int TimeStamp => 0;

        public void PostCheck(EventManager manager, UpdateBuffCountCommand Command)
        {
            if (Command.Source == null)
            {
                Debug.Log("[UpdateBuffTrigger PostCheck] " + Command.Target.name + " 更新了" + Command.Count + "层" + Command.BuffType.Name);
            }
            else
            {
                Debug.Log("[UpdateBuffTrigger PostCheck] " + Command.Source.name + " 对 " + Command.Target.name + " 更新了" + Command.Count + "层" + Command.BuffType.Name);
            }

            if (Command.Target.GetBuff(Command.BuffType) != null)
            {
                var Event = new BuffUpdatedEvent(Command.Source, Command.Target, Command.Target.GetBuff(Command.BuffType));
                manager.Publish(Event);
            }
        }

        public void PreCheck(EventManager manager, UpdateBuffCountCommand Command)
        {
        }
    }

    public class ApplyBuffTrigger : ITrigger<ApplyBuffCommand>
    {
        public Type CommandType => typeof(ApplyBuffCommand);

        public int Priority => 100;

        public int TimeStamp => 0;

        public void PostCheck(EventManager manager, ApplyBuffCommand Command)
        {
            if (Command.Source == null)
            {
                Debug.Log("[ApplyBuffTrigger PostCheck] " + Command.Target.name + " 施加了" + +Command.Count + "层" + Command.Buff.GetType().Name);
            }
            else
            {
                Debug.Log("[ApplyBuffTrigger PostCheck] " + Command.Source.name + " 对 " + Command.Target.name + " 施加了" + Command.Count + "层" + Command.Buff.GetType().Name);
            }
            if (Command.Target.GetBuff(Command.Buff.GetType()) != null)
            {
                var Event = new BuffUpdatedEvent(Command.Source, Command.Target, Command.Target.GetBuff(Command.Buff.GetType()));
                manager.Publish(Event);
            }
        }

        public void PreCheck(EventManager manager, ApplyBuffCommand Command)
        {
        }
    }
}

namespace Combat.EventListener
{
    public class BuffUpdatedRemoveListener : IEventListener
    {
        public void AddListen(EventManager eventManager)
        {
            eventManager.Subscribe<BuffUpdatedEvent>(OnBuffUpdated);
        }

        public void RemoveListen(EventManager eventManager)
        {
            eventManager.Unsubscribe<BuffUpdatedEvent>(OnBuffUpdated);
        }

        private void OnBuffUpdated(BuffUpdatedEvent e)
        {
            if (!e.Buff.IsAvaliable())
            {
                Debug.Log("[BuffUpdatedRemoveListener] " + e.Target.name + " 的 " + e.Buff.Name + " 不再可用");
                e.Target.combatSystem.ProcessCommand(new RemoveBuffCommand(null, e.Target, e.Buff.GetType()));
            }
        }
    }
}