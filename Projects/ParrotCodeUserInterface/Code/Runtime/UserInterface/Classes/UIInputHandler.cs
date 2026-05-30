using System;
using UnityEngine;
using UnityEngine.EventSystems;
using ParrotCode.Native.Common;

namespace ParrotCode.UI
{
    [DisallowMultipleComponent]
    public sealed class UIInputHandler : BaseMonoBehaviour,
        IPointerEnterHandler,
        IPointerExitHandler,
        IPointerDownHandler,
        IPointerUpHandler
    {
        public event Action<UIStateType> OnInput;

        public void OnPointerEnter(PointerEventData eventData)
            => OnInput?.Invoke(UIStateType.Hovered);

        public void OnPointerExit(PointerEventData eventData)
            => OnInput?.Invoke(UIStateType.Normal);

        public void OnPointerDown(PointerEventData eventData)
            => OnInput?.Invoke(UIStateType.Pressed);

        public void OnPointerUp(PointerEventData eventData)
            => OnInput?.Invoke(UIStateType.Selected);
    }
}
