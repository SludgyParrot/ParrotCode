using System;
using UnityEngine;
using UnityEngine.EventSystems;
using ParrotCode.Native.Common;

namespace ParrotCode.UI
{ 
    [DisallowMultipleComponent]
    public sealed class UIDraggableInputHandler: BaseMonoBehaviour, 
        IBeginDragHandler, 
        IDragHandler,
        IDropHandler, 
        IEndDragHandler
    {

        public Action<PointerEventData> OnActionPerformed {  get; private set; }

        public void OnBeginDrag(PointerEventData eventData)
            => OnActionPerformed?.Invoke(eventData);

        public void OnDrag(PointerEventData eventData)
            => OnActionPerformed?.Invoke(eventData);

        public void OnDrop(PointerEventData eventData)
            => OnActionPerformed?.Invoke(eventData);

        public void OnEndDrag(PointerEventData eventData)
            => OnActionPerformed?.Invoke(eventData);
    }
}
