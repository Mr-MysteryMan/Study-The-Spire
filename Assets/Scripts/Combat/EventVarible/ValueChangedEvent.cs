using System;

public class ValueChangedEvent<T>
{
    public T OldValue { get; } // 旧值
    public T NewValue { get; } // 新值

    public string ValueName { get; } // 事件名称

    public ValueChangedEvent(string name, T oldValue, T newValue)
    {
        ValueName = name;
        OldValue = oldValue;
        NewValue = newValue;
    }
}