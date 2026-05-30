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
