using System;
using Combat.Command;
namespace Combat.Trigger {
    public interface ITrigger {
        public Type CommandType {get;}
        public int Priority { get; }
        public int TimeStamp { get; }
    }

    public interface ITrigger<T> : ITrigger where T : ICommand
    {

        // 命令执行前的检查，如需要发送事件，可以通过manager发送
        void PreCheck(EventManager manager, T Command);

        // 命令执行后的检查，如需要发送事件，可以通过manager发送
        void PostCheck(EventManager manager, T Command);
    }
}