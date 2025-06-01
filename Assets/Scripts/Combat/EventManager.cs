using System;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    // 单例模式的事件管理器类
    public class EventManager : MonoBehaviour
    {
        // 事件委托类型，用于定义事件的回调方法
        public delegate void EventDelegate<T>(T eventData);

        // 事件表，存储不同类型的事件和对应的回调方法
        private Dictionary<Type, Delegate> eventTable;

        // string版本的事件表，使用名称索引事件
        public Dictionary<string, Delegate> stringEventTable;

        public void Initialize()
        {
            eventTable = new Dictionary<Type, Delegate>();
            stringEventTable = new Dictionary<string, Delegate>();
        }

        // 订阅事件
        public void Subscribe<T>(EventDelegate<T> eventDelegate)
        {
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

        // 订阅事件，string版本的是用于类型相同的不同事件，请保证相同名称类型一致
        public void Subscribe<T>(string eventName, EventDelegate<T> eventDelegate)
        {
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
        public void Unsubscribe<T>(EventDelegate<T> eventDelegate)
        {
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

        // 取消订阅事件
        public void Unsubscribe<T>(string eventName, EventDelegate<T> eventDelegate)
        {
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
        public void Publish<T>(T eventData)
        {
            Type eventType = typeof(T);
            if (eventTable.TryGetValue(eventType, out Delegate eventDelegate))
            {
                EventDelegate<T> callback = eventDelegate as EventDelegate<T>;
                callback?.Invoke(eventData);
            }
        }

        // 发布事件，string版本的是用于类型相同的不同事件
        public void Publish<T>(string eventName, T eventData)
        {
            if (stringEventTable.TryGetValue(eventName, out Delegate eventDelegate))
            {
                EventDelegate<T> callback = eventDelegate as EventDelegate<T>;
                callback?.Invoke(eventData);
            }
        }
    }
}