using System;
using ParrotCode.Native.Common;
using UnityEngine;
using UnityEngine.Localization;

namespace ParrotCode.UI
{
    [RequireComponent(typeof(ImageView))]
    [RequireComponent(typeof(UIInputHandler))]
    [DisallowMultipleComponent]
    public sealed class UIButton : BaseMonoBehaviour, IUIButton
    {
        [SerializeField, Space(5)]
        private TextView title;

        [SerializeField, Space(5)]
        private UITheme theme;

        [SerializeField, Space(5)]
        private UIStateType entryState;

        private ImageView imageViewer;
        private UIInputHandler inputHandler;

        public UITheme Theme
        {
            get
            {
                if (theme == null)
                    throw new NullReferenceException($"Theme is not assigned in the inspector for: {gameObject.name}");
                return theme;
            }
        }

        public ImageView ImageViewer
        {
            get
            { 
                if(imageViewer == null) 
                    imageViewer = GetComponent<ImageView>();
                return imageViewer;
            }
        }

        public UIInputHandler InputHandler
        {
            get
            { 
                if (inputHandler == null)
                    inputHandler = GetComponent<UIInputHandler>();
                return inputHandler;
            }
        }

        public void Config()
        {
            UIStateMachine stateMachine = new UIStateMachine(Theme);
            stateMachine.OnStateChanged += OnStateChanged;
            InputHandler.OnInput += stateMachine.SetState;
            stateMachine.SetState(entryState);
        }

        private void OnStateChanged(UIState state)
        {
            SetBackgroundColor(state.Color);
            SetBackgroundImage(state.Image);
        }

        public void SetBackgroundColor(Color color)
            => ImageViewer.SetColor(color);

        public void SetBackgroundImage(Sprite image)
            => ImageViewer.SetImage(image);

        public void SetTextColor(Color color)
            => title?.SetColor(color);

        public void SetTitleText(string text)
            => title?.SetText(text);

        public void SetTitleText(LocalizedString text)
        {

        }
    }
}
