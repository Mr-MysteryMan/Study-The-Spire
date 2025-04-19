using System;

namespace Combat.EventVarible
{
    public class ValueChangedEvent<T>
    {
        public T OldValue { get; } // 旧值
        public T NewValue { get; } // 新值

        public string ValueName { get; } // 事件名称

        public object Parent { get; } // 所在对象

        public ValueChangedEvent(string name, object parent, T oldValue, T newValue)
        {
            ValueName = name;
            Parent = parent;
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}