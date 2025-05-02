namespace Combat.EventVariable
{
    public class ReactiveIntVariable : ReactiveVariable<int>
    {
        public ReactiveIntVariable(string name, int initialValue, EventManager eventManager, object parent) : base(name, initialValue, eventManager, parent)
        {
        }
    }
}