using UnityEngine;

namespace Combat.EventVariable
{
    public class ReactiveVariableSO<T> : ScriptableObject, IReactiveVariable<T>
    {
        [SerializeField] private T _value;
        public T Value
        {
            get => _value; set
            {
                var oldValue = _value;
                _value = value;
                if (eventManager != null && oldValue != null && !oldValue.Equals(value))
                {
                    eventManager.Publish(new ValueChangedEvent<T>(name, parent, oldValue, value));
                }
            }
        }

        [SerializeField] private EventManager eventManager;


        [SerializeField] private string variableName;
        public string Name => variableName;

        private object parent;
        public object Parent => parent;

        public void SetParent(object parent)
        {
            this.parent = parent;
        }
    }
}