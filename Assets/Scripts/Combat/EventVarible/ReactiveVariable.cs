namespace Combat.EventVarible
{
    public class ReactiveVariable<T>
    {
        private T _value;
        public T Value
        {
            get => _value;
            set
            {
                var oldValue = _value;
                _value = value;
                if (EventManager.instance != null && oldValue != null && !oldValue.Equals(value))
                {
                    EventManager.instance.Publish(Name, new ValueChangedEvent<T>(_eventName, oldValue, value));
                }
            }
        }

        public string Name;

        private string _eventName;

        public object Source { get; }

        public ReactiveVariable(string name, string eventName, T value, object source = null)
        {
            Name = name;
            _eventName = eventName;
            _value = value;
            Source = source;
        }
    }
}