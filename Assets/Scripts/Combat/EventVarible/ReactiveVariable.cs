using UnityEngine;
namespace Combat.EventVarible
{
    public class ReactiveVariable<T> : ScriptableObject
    {
        [SerializeField] private T _value;
        [SerializeField] private EventManager _eventManager;

        public T Value
        {
            get => _value;
            set
            {
                var oldValue = _value;
                _value = value;
                if (_eventManager != null && oldValue != null && !oldValue.Equals(value))
                {
                    _eventManager.Publish(_name, new ValueChangedEvent<T>(_eventName, Parent, oldValue, value));
                }
            }
        }

        [SerializeField] private string _name;

        [SerializeField] private string _eventName;

        public GameObject Parent;

        public void SetParent(GameObject parent)
        {
            Parent = parent;
        }
    }
}