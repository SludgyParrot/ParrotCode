using UnityEngine;

namespace ParrotCode.UI
{
    public interface IUIView
    {
        UINavigationType NavigationType { get; set; }

        void SetRenderMode(RenderMode renderMode);
    }
}
