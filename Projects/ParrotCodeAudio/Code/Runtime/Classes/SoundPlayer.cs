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
