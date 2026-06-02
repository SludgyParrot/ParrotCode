using UnityEngine;
using UnityEngine.Localization;
using ParrotCode.Native.Common;
using ParrotCode.EventSystem;
using ParrotCode.Audio;

namespace ParrotCode.UI
{
    [RequireComponent(typeof(SoundPlayer))]
    [RequireComponent(typeof(ImageView))]
    [RequireComponent(typeof(UIInputHandler))]
    [RequireComponent (typeof(UIButtonConfigEventHandler))] 
    [DisallowMultipleComponent]
    public sealed class UIButton : BaseMonoBehaviour, IUIButton, ISelectable
    {
        [SerializeField, Space(5)]
        private TextView title;

        [SerializeField, Space(5)]
        private UIStateType entryState;

        [SerializeField, Space(5)]
        private UITheme fallbackTheme;

        private ImageView imageViewer;
        private UIInputHandler inputHandler;
        public UIStateMachine stateMachine;
        private SoundPlayer soundPlayer;
        private UIButtonConfigEventHandler configEventHandler;

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

        public UIButtonConfigEventHandler ConfigEventHandler
        {
            get
            {
                if(configEventHandler == null)
                    configEventHandler = GetComponent<UIButtonConfigEventHandler>();
                return configEventHandler;
            }
        }

        private void OnEnable()
        {
            if(stateMachine == null && fallbackTheme == null)
            {
                Log($"[{gameObject.name}] Button initialization failed. There is no fallback theme '{nameof(fallbackTheme)}' assigned.", LogVerbosity.Error, LogChannel.UI);
                return;
            }

            stateMachine ??= new UIStateMachine(fallbackTheme);
            stateMachine.AddListener(OnStateChanged);
            InputHandler.AddListener(stateMachine.SetState);
            EventBus.AddListener<UITheme>(OnThemeChangedEvent);
        }

        private void OnDisable()
        {
            if (stateMachine == null)
            {
                Log($"[{gameObject.name}] Unregister UI button state machine on disable failed. State machine component '{nameof(stateMachine)}' is null.", LogVerbosity.Error, LogChannel.UI);
                return;
            }
            stateMachine.RemoveListener(OnStateChanged);
            InputHandler.RemoveListener(stateMachine.SetState);
            EventBus.RemoveListener<UITheme>(OnThemeChangedEvent);
        }

        private void OnThemeChangedEvent(UITheme theme)
        {
            if(theme == null || stateMachine == null)
            {
                Log($"UI Button: OnThemeChangedEvent failed. Theme event parameter value '{nameof(theme)}' or state machine cannot be null for '{gameObject.name}'.", LogVerbosity.Error, LogChannel.UI);
                return;
            }

            stateMachine.ChangeTheme(theme);
            stateMachine.SetState(entryState);
        }

        private void OnStateChanged(UIState state)
        {
            if(state == null)
            {
                Log($"[{gameObject.name}] OnStateChanged failed, state '{nameof(state)}' argument s null.", LogVerbosity.Error, LogChannel.UI);
                return;
            }

            SetColor(state.TextColor);
            SetBackgroundColor(state.BackgroundColor);
            SetBackgroundImage(state.Image);
            PlaySoundFx(state.SoundFx);

            if (state.State == UIStateType.Pressed)
                ConfigEventHandler.Config();
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
            //if(text == null)
            //{

            //    return;
            //}

            //title?.SetText(text.GetLocalizedString());
        }

        public void PlaySoundFx(AudioClip clip)
            => SoundPlayer.PlayOnce(clip);

        public void Focus()
        {
           
        }

        public void Select()
        {
        
        }
    }
}
