using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ParrotCode.Native.Common;

namespace ParrotCode.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(CanvasScaler))]
    [RequireComponent(typeof(GraphicRaycaster))]
    [RequireComponent(typeof(CanvasGroup))]
    public sealed class UIView : BaseMonoBehaviour, IUIView
    {
        [HideInInspector]
        public List<Object> selectables = new List<Object>();

        public IEnumerable<ISelectable> Selectables
        {
            get
            {
                foreach(Object selectableObject in selectables)
                    if (selectableObject is ISelectable)
                        yield return selectableObject as ISelectable;
            }
        }

        private Canvas uiViewCanvas;
        public Canvas UIViewCanvas
        {
            get
            {
                if (uiViewCanvas == null)
                    uiViewCanvas = GetComponent<Canvas>();
                return uiViewCanvas;
            }
        }

        public void SetRenderMode(RenderMode renderMode)
            => UIViewCanvas.renderMode = renderMode;

        public void OnFocus()
        {
            
        }

        public void OnBlur()
        {
            
        }
    }
}
