using UnityEngine;
public class ReactiveIntVariable : ReactiveVariable<int>
{
    public ReactiveIntVariable(string name, string eventName, int value, Object source = null) : base(name, eventName, value, source)
    {
    }
}