using System;
using Combat.Command;
namespace Combat.Trigger {
    // 触发器接口，仅用于包装一个基类，不要直接实现该接口
    public interface ITrigger {
        public Type CommandType {get;}

        // 触发器的优先级，数值越小，优先级越高
        public int Priority { get; }

        // 触发器的时间戳，数值越小，优先级越高
        public int TimeStamp { get; }
    }

    // 触发器接口，泛型版本，用于处理对应的命令
    public interface ITrigger<T> : ITrigger where T : ICommand
    {
        // 命令执行前的检查，如需要发送事件，可以通过manager发送
        void PreCheck(EventManager manager, T Command);

        // 命令执行后的检查，如需要发送事件，可以通过manager发送
        void PostCheck(EventManager manager, T Command);
    }
}