using System;
using System.Collections.Generic;

namespace ParrotCode.UI
{
    public static class UIEventBus
    {
        private static readonly Dictionary<Type, Delegate> events = new Dictionary<Type, Delegate>();

        public static void Subscribe<T>(Action<T> func)
        {
            if (events.TryGetValue(typeof(T), out var value))
                events[typeof(T)] = Delegate.Combine(value, func);
            else
                events[typeof(T)] = func;
        }

        public static void Unsubscribe<T>(Action<T> func)
        {
            if (events.TryGetValue(typeof(T), out var value))
                events[typeof(T)] = Delegate.Remove(value, func);
        }

        public static void Publish<T>(T func, params object[] objects)
        {
            if (events.TryGetValue(typeof(T), out var value))
                value.Method.Invoke(value, objects);
        }
    }
}
