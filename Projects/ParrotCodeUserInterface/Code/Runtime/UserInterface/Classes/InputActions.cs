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
using UnityEngine;
using UnityEngine.EventSystems;
using ParrotCode.Native.Common;

namespace ParrotCode.UI
{
    [DisallowMultipleComponent]
    public sealed class InputActions : BaseMonoBehaviour, 
        IUIInputHandler,
        IPointerEnterHandler,
        IPointerExitHandler,
        IPointerDownHandler,
        IPointerUpHandler
    {
        private event Action<State> onInputActionEventTrigger;

        public void AddListener(params Action<State>[] listeners)
        {
            if(listeners == null || listeners.Length == 0)
            {
                Log($"[{gameObject.name}] AddEventListeners failed for register '{nameof(listeners)}'. There are no listener(s) assigned in the arguments.", LogVerbosity.Error, LogChannel.UI);
                return;
            }

            for(int i = 0; i < listeners.Length; i++)
            {
                if (listeners[i] == null)
                {
                    Log($"[{gameObject.name}] AddEventListeners failed to register a listener at '{i}'", LogVerbosity.Error, LogChannel.UI);
                    continue;
                }
                onInputActionEventTrigger += listeners[i];
            }
        }

        public void RemoveListener(params Action<State>[] listeners)
        {
            if (listeners == null || listeners.Length == 0)
            {
                Log($"[{gameObject.name}] RemoveEventListener failed for unregister '{nameof(listeners)}'. There are no listener(s) assigned in the arguments.", LogVerbosity.Error, LogChannel.UI);
                return;
            }

            for (int i = 0; i < listeners.Length; i++)
            {
                if (listeners[i] == null)
                {
                    Log($"[{gameObject.name}] RemoveEventListener failed to unregister a listener at '{i}'", LogVerbosity.Error, LogChannel.UI);
                    continue;
                }
                onInputActionEventTrigger -= listeners[i];
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
            => onInputActionEventTrigger?.Invoke(State.Hovered);

        public void OnPointerExit(PointerEventData eventData)
            => onInputActionEventTrigger?.Invoke(State.Normal);

        public void OnPointerDown(PointerEventData eventData)
            => onInputActionEventTrigger?.Invoke(State.Pressed);

        public void OnPointerUp(PointerEventData eventData)
            => onInputActionEventTrigger?.Invoke(State.Selected);
    }
}
