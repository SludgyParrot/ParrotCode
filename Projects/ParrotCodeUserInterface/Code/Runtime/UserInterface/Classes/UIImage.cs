using System;
using UnityEngine;
using UnityEngine.UI;

namespace ParrotCode.UI
{
    [RequireComponent (typeof (Image))]
    public class UIImage : UIStateComponent<Image, UIImageState>
    {
        public override void SetColor(Color color)
            => Value.color = color;

        public override void SetUIState(UIStateType stateType, Action<(UIState state, string errorMessage)> actionCallback = null)
        {
           
        }

        protected void SetBackgroundImage(Sprite image)
            => Value.sprite = image;
    }
}
