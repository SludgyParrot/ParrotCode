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
        [field: SerializeField, Space(5)]
        public UINavigationType NavigationType { get; set; }

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
    }
}
