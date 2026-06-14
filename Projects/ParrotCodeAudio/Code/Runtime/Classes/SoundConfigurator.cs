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

using System.Collections.Generic;
using UnityEngine;
using ParrotCode.Native.Common;
using System.Linq;
using ParrotCode.Native.Inspector;

namespace ParrotCode.Audio
{
    [DisallowMultipleComponent]
    public sealed class SoundConfigurator: BaseMonoBehaviour
    {
        [SerializeField, Space(5)]
        private SoundSettings settings;

        [SerializeField, Space(5)]
        private List<SoundPlayer> audioPlayers;

        [SerializeField, Space(5)]
        private FindObjectsInactive includeInactiveSoundPlayers;

        [SerializeField, Space(5)]
        private FindObjectsSortMode findSoundPlayersSortMode;

        private IReadOnlyList<SoundPlayer> AudioPlayers => audioPlayers;

        protected override void Init()
        {
            CacheLocalAudioPlayers();
            ApplySettings(settings);
        }

        [Button]
        private void FetchAllSceneSoundPlayers()
        {
            audioPlayers = FindObjectsByType<SoundPlayer>(includeInactiveSoundPlayers, findSoundPlayersSortMode).ToList();

            if (audioPlayers?.Count > 0)
                Log($"Found and assigned {AudioPlayers.Count} sound players to sound configurator '{gameObject.name}'.", LogVerbosity.Debug, LogChannel.Audio);
            else
                Log($"There are no sound players found in the scene for '{gameObject.name}'.", LogVerbosity.Warning, LogChannel.Audio);
        }

        private void CacheLocalAudioPlayers()
        {
            audioPlayers ??= new List<SoundPlayer>();
            var players = GetComponentsInChildren<SoundPlayer>().Distinct();

            if (audioPlayers.Count == 0)
                audioPlayers = players.ToList();
            else
                audioPlayers.AddRange(players);
        }

        public void ApplySettings(SoundSettings settings)
        {
            if(settings == null)
            {
                Log($"Couldn't apply settings for '{gameObject.name}'. The audio settings parameter value is null.", LogVerbosity.Error, LogChannel.Audio);
                return;
            }

            for (int i = 0; i < AudioPlayers.Count; i++)
            {
                SoundPlayer audioPlayer = AudioPlayers[i];
                if (audioPlayer == null)
                {
                    Log($"Couldn't apply settings for sound player at index: {i} for '{gameObject.name}'. The value of the audio player is null.", LogVerbosity.Error, LogChannel.Audio);
                    continue;
                }
                audioPlayer.ApplySettings(settings);
            }
        }
    }
}
