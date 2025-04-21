using System;
using Combat.Command;
namespace Combat.Processor
{

    // 处理器接口，仅用于包装一个基类，不要直接实现该接口
    public interface IProcessor
    {
        public int Priority { get; }
        public int TimeStamp { get; }
        public Type CommandType { get; }

        public ProcessorEffectSideType EffectSide { get; }
    }

    // 处理器接口，泛型版本，用于处理对应的命令
    public interface IProcessor<T> : IProcessor where T : ICommand
    {
        void Process(ref T command);
    }

    // 表示命令的处理方向
    public enum ProcessorEffectSideType
    {
        Source, // 发起者匹配则处理
        Target, // 目标者匹配则处理
    }
}