using System;
using System.Collections.Generic;
using ParrotCode.Native.Inspector;
using UnityEngine;

namespace ParrotCode.UI
{
    public sealed class UIButton : SelectableUIComponent
    {
        [SerializeField, Space(5)]
        private bool requireButtonTitle;

        [SerializeField, Space(5)]
        private UIText titleTextDisplayer;

        public UIText TitleTextDisplayer
        {
            get
            {
                if (titleTextDisplayer == null)
                    throw new NullReferenceException($"TitleTextDisplayer cannot be null for: {Identifier}");
                return titleTextDisplayer;
            }
        }

        [Button]
        public void Create()
        {
            if(states == null || states.Count == 0)
            {
                states = new List<UIImageState>
                { 
                    new UIImageState(null, "Normal State", UIStateType.Normal, Color.white),
                    new UIImageState(null, "Hovered State", UIStateType.Hovered, Color.black),
                    new UIImageState(null, "Selected State", UIStateType.Selected, Color.gray),
                    new UIImageState(null, "Pressed State", UIStateType.Pressed, Color.black),
                    new UIImageState(null, "Disabled State", UIStateType.Disabled, Color.grey),
                };
            }

            if(requireButtonTitle)
            {
                titleTextDisplayer = GetComponentInChildren<UIText>();

                if (titleTextDisplayer != null)
                    return;

                titleTextDisplayer = new GameObject("Title Displayer").AddComponent<UIText>();
                titleTextDisplayer.SetText("[None]");
                titleTextDisplayer.SetColor(Color.black);
                titleTextDisplayer.SetAlignment(TMPro.TextAlignmentOptions.Center);

                Add(titleTextDisplayer);
            }
        }

        [Button]
        private void Invoke()
        {

        }

        protected override void Init()
        {
            base.Init();

            if (!requireButtonTitle)
                return;

            TitleTextDisplayer.SetUIState(initialInteractableState);
        }

        public override void SetUIState(UIStateType stateType, Action<(UIImageState state, string errorMessage)> actionCallback = null)
        {
            base.SetUIState(stateType, actionCallback);

            if (!requireButtonTitle)
                return;

            TitleTextDisplayer.SetUIState(stateType);
        }

        public override void SetColor(Color color) { }
    }
}
