using UnityEngine;
namespace Combat.EventVariable
{
    public class ReactiveVariable<T> : IReactiveVariable<T>
    {
        private T _value;
        private string name;
        private EventManager eventManager;
        private object parent;

        public T Value
        {
            get => _value;
            set
            {
                var oldValue = _value;
                _value = value;
                if (eventManager != null && oldValue != null && !oldValue.Equals(value))
                {
                    eventManager.Publish(new ValueChangedEvent<T>(name, parent, oldValue, value));
                }
            }
        }

        public string Name => name;

        public object Parent => parent;


        public void SetParent(object parent)
        {
            this.parent = parent;
        }

        public ReactiveVariable(string name, T initialValue, EventManager eventManager)
        {
            this.name = name;
            _value = initialValue;
            this.eventManager = eventManager;
        }
    }
}