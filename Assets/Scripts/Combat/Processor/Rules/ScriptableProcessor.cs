using System;
using UnityEngine;

using Combat.Command;

namespace Combat.Processor.Rules
{
    // SO 版本的处理器接口，仅用于包装一个基类，不要直接继承该接口（用class是为了能在编辑器中显示）
    public abstract class ScriptableProcessor : ScriptableObject, IProcessor
    {
        // 处理器的优先级，数值越小优先级越高
        public abstract int Priority { get; }

        // 处理器的时间戳，表示相同类型的处理器的执行顺序
        public abstract int TimeStamp { get; }

        // 处理器的命令类型，表示该处理器能处理的命令类型
        public abstract Type CommandType { get; }

        // 处理器的生效方向，选定当Target或Source匹配时，才能处理该命令
        public abstract ProcessorEffectSideType EffectSide { get; }
    }

    // SO 版本的处理器接口，泛型版本，用于处理对应的命令
    public abstract class ScriptableProcessor<T> : ScriptableProcessor, IProcessor<T> where T : ICommand
    {
        public abstract void Process(ref T command);
    }
}