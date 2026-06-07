/*

Parrot Code
Copyright (c) 2026 Sludgy Parrot (Pty) Ltd. All Rights Reserved.

This source code is proprietary and confidential software owned by
Sludgy Parrot (Pty) Ltd.

Parrot Code is a commercial software product developed and distributed
by Sludgy Parrot (Pty) Ltd.

Unauthorized copying, modification, distribution, sublicensing,
reverse engineering, decompilation, disclosure, or use of this
software, in whole or in part, is strictly prohibited without
prior written permission from Sludgy Parrot (Pty) Ltd.

This software is provided under the terms of a separate license
agreement. Possession of this source code does not grant any rights
to use, modify, distribute, or create derivative works unless
explicitly authorized by a valid written license.

THE SOFTWARE IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, EXCEPT AS REQUIRED BY APPLICABLE LAW.

For licensing inquiries:
licensing@sludgyparrot.com

*/

using UnityEngine;
using UnityEngine.Localization;
using ParrotCode.Native.Common;
using ParrotCode.EventSystem;
using ParrotCode.Audio;

namespace ParrotCode.UI
{
    [RequireComponent(typeof(SoundPlayer))]
    [RequireComponent(typeof(ImageView))]
    [RequireComponent(typeof(InputActions))]
    [RequireComponent (typeof(UIButtonConfigEvent))]
    public sealed class UIButton : Selectable, IUIButton
    {
        [SerializeField, Space(5)]
        private TextView title;

        [SerializeField, Space(5)]
        private State entryState;

        [SerializeField, Space(5)]
        private UITheme fallbackTheme;

        private ImageView imageViewer;
        private InputActions inputHandler;
        public UIStateMachine stateMachine;
        private SoundPlayer soundPlayer;
        private UIButtonConfigEvent configEventHandler;

        public ImageView ImageViewer
        {
            get
            { 
                if(imageViewer == null) 
                    imageViewer = GetComponent<ImageView>();
                return imageViewer;
            }
        }

        public InputActions InputHandler
        {
            get
            { 
                if (inputHandler == null)
                    inputHandler = GetComponent<InputActions>();
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

        public UIButtonConfigEvent ConfigEventHandler
        {
            get
            {
                if(configEventHandler == null)
                    configEventHandler = GetComponent<UIButtonConfigEvent>();
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

            if (state.State == State.Pressed)
                ConfigEventHandler.Config();
        }

        public void OverrideTitleDisplayer(TextView title)
            => this.title = title;

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

        public override void Select()
            => stateMachine.SetState(entryState);
    }
}
