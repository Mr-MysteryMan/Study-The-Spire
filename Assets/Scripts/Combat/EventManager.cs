using System;
using System.Collections.Generic;
using UnityEngine;

// 单例模式的事件管理器类
public class EventManager : MonoBehaviour
{
    public static EventManager instance;

    private void Awake() {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 阻止销毁
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public delegate void EventDelegate<T>(T eventData);
    private Dictionary<Type, Delegate> eventTable = new ();

    public Dictionary<string, Delegate> stringEventTable = new ();

    // 订阅事件
    public void Subscribe<T>(EventDelegate<T> eventDelegate) {
        Type eventType = typeof(T);
        if (eventTable.ContainsKey(eventType))
        {
            eventTable[eventType] = Delegate.Combine(eventTable[eventType], eventDelegate);
        }
        else
        {
            eventTable.Add(eventType, eventDelegate);
        }
    }

    public void Subscribe<T>(string eventName, EventDelegate<T> eventDelegate) {
        if (stringEventTable.ContainsKey(eventName))
        {
            stringEventTable[eventName] = Delegate.Combine(stringEventTable[eventName], eventDelegate);
        }
        else
        {
            stringEventTable.Add(eventName, eventDelegate);
        }
    }

    // 取消订阅事件
    public void Unsubscribe<T>(EventDelegate<T> eventDelegate) {
        Type eventType = typeof(T);
        if (eventTable.ContainsKey(eventType))
        {
            eventTable[eventType] = Delegate.Remove(eventTable[eventType], eventDelegate);
            if (eventTable[eventType] == null)
            {
                eventTable.Remove(eventType);
            }
        }
    }

    public void Unsubscribe<T>(string eventName, EventDelegate<T> eventDelegate) {
        if (stringEventTable.ContainsKey(eventName))
        {
            stringEventTable[eventName] = Delegate.Remove(stringEventTable[eventName], eventDelegate);
            if (stringEventTable[eventName] == null)
            {
                stringEventTable.Remove(eventName);
            }
        }
    }

    // 发布事件
    public void Publish<T>(T eventData) {
        Type eventType = typeof(T);
        if (eventTable.TryGetValue(eventType, out Delegate eventDelegate))
        {
            EventDelegate<T> callback = eventDelegate as EventDelegate<T>;
            callback?.Invoke(eventData);
        }
    }

    public void Publish<T>(string eventName, T eventData) {
        if (stringEventTable.TryGetValue(eventName, out Delegate eventDelegate))
        {
            EventDelegate<T> callback = eventDelegate as EventDelegate<T>;
            callback?.Invoke(eventData);
        }
    }
}
