namespace Combat.EventVarible
{
    public class ReactiveIntVariable : ReactiveVariable<int>
    {
        public ReactiveIntVariable(string name, string eventName, int value, object source = null) : base(name, eventName, value, source)
        {
        }
    }
}