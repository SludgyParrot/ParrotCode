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

#region Included System Assemblies
using System;
using System.Collections.Generic;
#endregion

#region Included Unity Assemblies
using UnityEngine;
#endregion

namespace ParrotCode.UI
{
    public sealed class UIStateMachine
    {
        private readonly Dictionary<State, UIState> states = new Dictionary<State, UIState>();

        public UIState CurrentState { get; private set; }
        private event Action<UIState> onStateChanged;

        public UIStateMachine(UITheme theme)
            => BuildUITheme(theme);

        public void ChangeTheme(UITheme theme)
            => BuildUITheme(theme);

        private void BuildUITheme(UITheme theme)
        {
            if (theme == null || theme.States == null || theme.States.Count == 0)
                throw new ArgumentNullException($"UI state machine initialization failed. Either the theme is null or the states were not created/assigned for theme '{nameof(theme)}'.");
 
            states.Clear();

            for (int i = 0; i < theme.States.Count; i++)
            {
                UIState state = theme.States[i];

                if (state == null)
                {
                    Debug.LogWarning($"Couldn't add UI theme state at index({i}) for state machine.");
                    continue;
                }

                if (!states.ContainsKey(state.State))
                    states.Add(state.State, state);
                else
                    Debug.LogWarning($"Couldn't add UI theme state '{nameof(state)}' at index({i}), because an object with the same key: {state.State} already exists.");
            }
        }

        public void SetState(State stateType)
        {
            if (states.TryGetValue(stateType, out var state))
            {
                CurrentState = state;
                onStateChanged?.Invoke(state);
            }
            else
                Debug.Log($"Set state: {stateType} failed. Couldn't find state of type on the assigned theme.");
        }

        public void AddListener(params Action<UIState>[] listeners)
        {
            if (listeners == null || listeners.Length == 0)
            {
                Debug.LogError($"AddEventListeners failed for register '{nameof(listeners)}'. There are no listener(s) assigned in the arguments.");
                return;
            }

            for (int i = 0; i < listeners.Length; i++)
            {
                if (listeners[i] == null)
                {
                    Debug.LogError($"AddEventListeners failed to register a listener at '{i}'.");
                    continue;
                }
                onStateChanged += listeners[i];
            }
        }

        public void RemoveListener(params Action<UIState>[] listeners)
        {
            if (listeners == null || listeners.Length == 0)
            {
                Debug.LogError($"RemoveEventListener failed for unregister '{nameof(listeners)}'. There are no listener(s) assigned in the arguments.");
                return;
            }

            for (int i = 0; i < listeners.Length; i++)
            {
                if (listeners[i] == null)
                {
                    Debug.LogError($"RemoveEventListener failed to unregister a listener at '{i}'.");
                    continue;
                }
                onStateChanged -= listeners[i];
            }
        }

    }
}
