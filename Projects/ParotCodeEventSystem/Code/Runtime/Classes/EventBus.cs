using System;
using System.Collections.Generic;

namespace ParotCode.EventSystem
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
        public static void Register<T>(Action<T> eventCallback)
        {
            try
            {
                if (events.TryGetValue(typeof(T), out object existingEvent))
                    events[typeof(T)] = (Action<T>)existingEvent + eventCallback;
                else
                    events[typeof(T)] = eventCallback;
            }
            catch (Exception exception)
            {
                UnityEngine.Debug.LogError($"Register event on type: {typeof(T)} failed with exception: {exception.Message}");
            }
        }

        /// <summary>
        /// Sends action event to subscribed functions.
        /// </summary>
        /// <typeparam name="T">Event data type.</typeparam>
        /// <param name="eventData">The event data parameter to be sent to the listening functions.</param>
        public static void Publish<T>(T eventData)
        {
            try
            {
                if(events.TryGetValue(typeof(T), out object existingEvent))
                    ((Action<T>)existingEvent).Invoke(eventData);
                else
                    UnityEngine.Debug.LogWarning($"Couldn't publish event of type: {typeof(T)}. Event was not found in the registered events list.");
            }
            catch (Exception exception)
            {
                UnityEngine.Debug.LogError($"Publish event on type: {typeof(T)} failed with exception: {exception.Message}");
            }
        }

        /// <summary>
        /// Removes function callback from the event bus's events list.
        /// </summary>
        /// <typeparam name="T">Event data type.</typeparam>
        /// <param name="eventCallback">The function that listens to callbacks from the event.</param>
        public static void Unregister<T>(Action<T> eventCallback)
        {
            try
            {
                if (events.TryGetValue(typeof(T), out object existingEvent))
                    events[typeof(T)] = (Action<T>)existingEvent - eventCallback;
                else
                    UnityEngine.Debug.LogWarning($"Couldn't unregister event of type: {typeof(T)}. Event was not found in the registered events list.");
            }
            catch (Exception exception)
            {
                UnityEngine.Debug.LogError($"Unregister event on type: {typeof(T)} failed with exception: {exception.Message}");
            }
        }
    }
}
