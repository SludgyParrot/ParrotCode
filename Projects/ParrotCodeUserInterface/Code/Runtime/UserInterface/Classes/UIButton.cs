using UnityEngine;
using UnityEngine.Localization;
using ParrotCode.Native.Common;
using ParotCode.EventSystem;
using ParrotCode.Audio;

namespace ParrotCode.UI
{
    [RequireComponent(typeof(SoundPlayer))]
    [RequireComponent(typeof(ImageView))]
    [RequireComponent(typeof(UIInputHandler))]
    [DisallowMultipleComponent]
    public sealed class UIButton : BaseMonoBehaviour, IUIButton
    {
        [SerializeField, Space(5)]
        private TextView title;

    

        [SerializeField, Space(5)]
        private UIStateType entryState;

        private ImageView imageViewer;
        private UIInputHandler inputHandler;
        private UIStateMachine stateMachine;
        private SoundPlayer soundPlayer;

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

        public SoundPlayer SoundPlayer
        {
            get
            {
                if (soundPlayer == null)
                    soundPlayer = GetComponent<SoundPlayer>();
                return soundPlayer;
            }
        }

        protected override void Init()
            => EventBus.Register<UITheme>(OnThemeChangedEvent);

        private void OnThemeChangedEvent(UITheme theme)
        {
            stateMachine = new UIStateMachine(theme);
            stateMachine.OnStateChanged += OnStateChanged;
            InputHandler.OnInput += stateMachine.SetState;
            stateMachine.SetState(entryState);
        }

        private void OnStateChanged(UIState state)
        {
            SetColor(state.TextColor);
            SetBackgroundColor(state.BackgroundColor);
            SetBackgroundImage(state.Image);
            PlaySoundFx(state.SoundFx);
        }

        public void SetColor(Color color)
            => title?.SetColor(color);

        public void SetBackgroundColor(Color color)
            => ImageViewer.SetColor(color);

        public void SetBackgroundImage(Sprite image)
            => ImageViewer.SetImage(image);

        public void SetTitleText(string text)
            => title?.SetText(text);

        public void SetTitleText(LocalizedString text)
        {

        }

        public void PlaySoundFx(AudioClip clip)
            => SoundPlayer.PlayOnce(clip);

        public void OnDestroy()
        {
            stateMachine.OnStateChanged -= OnStateChanged;
            InputHandler.OnInput -= stateMachine.SetState;
        }
    }
}
