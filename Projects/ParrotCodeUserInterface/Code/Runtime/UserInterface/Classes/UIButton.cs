using System;
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
