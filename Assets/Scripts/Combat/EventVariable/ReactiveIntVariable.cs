namespace Combat.EventVariable
{
    public class ReactiveIntVariable : ReactiveVariable<int>
    {
        public ReactiveIntVariable(string name, int initialValue, EventManager eventManager) : base(name, initialValue, eventManager)
        {
        }
    }
}