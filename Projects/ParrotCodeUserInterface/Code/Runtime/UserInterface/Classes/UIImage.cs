using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ParrotCode.UI
{
    [RequireComponent (typeof (Image))]
    public class UIImage : UIStateComponent<Image, UIImageState>
    {
        protected override List<UIImageState> States { get; set; } = new List<UIImageState>()
        {
            new UIImageState(null, "Normal State", UIStateType.Normal, Color.white),
            new UIImageState(null, "Hovered State", UIStateType.Hovered, Color.white)
        };

        public override void SetColor(Color color)
            => Value.color = color;

        public override void SetUIState(UIStateType stateType, Action<(UIState state, string errorMessage)> actionCallback = null)
        {
           
        }

        protected void SetBackgroundImage(Sprite image)
            => Value.sprite = image;
    }
}
