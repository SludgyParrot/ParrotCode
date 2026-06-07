/*

Parrot Code
Copyright (c) 2026 Sludgy Parrot (Pty) Ltd. All Rights Reserved.

This source code is proprietary and confidential software owned by
Sludgy Parrot (Pty) Ltd.

Parrot Code is a commercial software product developed and distributed
by Sludgy Parrot (Pty) Ltd.

Unauthorized copying, modification, distribution, sublicensing,
reverse engineering, decompilation, disclosure, or use of this
software, in whole or in part, is strictly prohibited without
prior written permission from Sludgy Parrot (Pty) Ltd.

This software is provided under the terms of a separate license
agreement. Possession of this source code does not grant any rights
to use, modify, distribute, or create derivative works unless
explicitly authorized by a valid written license.

THE SOFTWARE IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, EXCEPT AS REQUIRED BY APPLICABLE LAW.

For licensing inquiries:
licensing@sludgyparrot.com

*/

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
