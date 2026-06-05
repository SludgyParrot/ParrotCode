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
