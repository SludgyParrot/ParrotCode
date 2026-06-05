using System.Collections.Generic;
using UnityEngine;

namespace ParrotCode.UI
{
    public interface IUIView
    {
        IEnumerable<ISelectable> Selectables { get; }

        void SetRenderMode(RenderMode renderMode);
        void OnFocus();
        void OnBlur();
    }
}
