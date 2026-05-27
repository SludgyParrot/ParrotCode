using System;
using UnityEngine;

namespace ParrotCode.UI
{
    [RequireComponent(typeof(Canvas))]
    public sealed class UICanvas : UIComponent<Canvas>
    {
        public override void SetColor(Color color)
        {
           
        }

        public override void SetUIState(UIStateType stateType, Action<(UIState state, string errorMessage)> actionCallback = null)
        {
            
        }
    }
}
