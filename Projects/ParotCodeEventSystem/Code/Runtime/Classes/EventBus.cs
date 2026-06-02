using System;
using System.Collections.Generic;

namespace ParrotCode.EventSystem
{
    /// <summary>
    /// This class handles global events across the Sludgy Parrot Code framework.
    /// </summary>
    public static class EventBus
    {
        private static readonly Dictionary<Type, object> events = new Dictionary<Type, object>();

        /// <summary>
        /// Adds function callback to the event bus's events list.
        /// </summary>
        /// <typeparam name="T">Event data type.</typeparam>
        /// <param name="eventCallback">The function that listens to callbacks from the event.</param>
        public static void AddListener<T>(Action<T> eventCallback)
        {
            if (events.TryGetValue(typeof(T), out object existingEvent))
                events[typeof(T)] = (Action<T>)existingEvent + eventCallback;
            else
                events[typeof(T)] = eventCallback;
        }

        /// <summary>
        /// Sends action event to subscribed functions.
        /// </summary>
        /// <typeparam name="T">Event data type.</typeparam>
        /// <param name="eventData">The event data parameter to be sent to the listening functions.</param>
        public static void InvokeEvent<T>(T eventData)
        {
            if (events.TryGetValue(typeof(T), out object existingEvent))
                ((Action<T>)existingEvent).Invoke(eventData);
        }

        /// <summary>
        /// Removes function callback from the event bus's events list.
        /// </summary>
        /// <typeparam name="T">Event data type.</typeparam>
        /// <param name="eventCallback">The function that listens to callbacks from the event.</param>
        public static void RemoveListener<T>(Action<T> eventCallback)
        {
            if (events.TryGetValue(typeof(T), out object existingEvent))
            {
                Action<T> callback = (Action<T>)existingEvent - eventCallback;

                if (callback == null)
                    events.Remove(typeof(T));
                else
                    events[typeof(T)] = callback;
            }
            else
                UnityEngine.Debug.LogWarning($"Couldn't unregister event of type: {typeof(T)}. Event was not found in the registered events list.");
        }
    }
}
