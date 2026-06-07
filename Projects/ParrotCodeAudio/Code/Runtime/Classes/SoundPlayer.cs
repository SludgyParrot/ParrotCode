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
using ParrotCode.Native.Common;

namespace ParrotCode.Audio
{
    [RequireComponent(typeof(AudioSource))]
    [DisallowMultipleComponent]
    public sealed class SoundPlayer: BaseMonoBehaviour, ISoundPlayer
    {
        private AudioSource player;

        private AudioSource Player
        {
            get
            {
                if (player == null) 
                    player = GetComponent<AudioSource>();
                return player;
            }
        }

        public void ApplySettings(SoundSettings settings)
        {
            if (settings == null)
            {
                Log($"Apply audio settings has failed for '{gameObject.name}'. Sound setting's parameter value is null.", LogVerbosity.Warning, LogChannel.Audio);
                return;
            }

            Player.volume = settings.Volume;
            Player.pitch = settings.Pitch;
            Player.loop = settings.Loop;
            Player.playOnAwake = settings.PlayOnAwake;
        }

        public void LoadSound(AudioClip clip)
        {
            if (clip == null)
            {
                Log($"Load sound clip failed for '{gameObject.name}'. The clip parameter value is null.", LogVerbosity.Error, LogChannel.Audio);
                return;
            }
            Player.clip = clip;
        }

        public void Play()
        {
            if(Player.clip == null)
            {
                Log($"Play audio clip failed for '{gameObject.name}'. There is no clip assigned to the audio player component.", LogVerbosity.Error, LogChannel.Audio);
                return;
            }

            Player.Play();
        }

        public void Play(AudioClip clip)
        {
            if (clip == null)
            {
                Log($"Play audio clip failed for '{gameObject.name}'. The clip parameter value is null.", LogVerbosity.Error, LogChannel.Audio);
                return;
            }

            Player.clip = clip;
            Player.Play();
        }

        public void PlayOnce(AudioClip clip)
        { 
            if(clip == null)
            {
                Log($"Play audio clip once failed for '{gameObject.name}'. The clip parameter value is null.", LogVerbosity.Error, LogChannel.Audio);
                return;
            }
            Player.PlayOneShot(clip);
        } 
        
        public void Stop()
        {
            if (!Player.isPlaying)
            {
                Log($"There is currently no audio playback to stop for '{gameObject.name}'.", LogVerbosity.Warning, LogChannel.Audio);
                return;
            }

            Player.Stop();
        }
    }
}
