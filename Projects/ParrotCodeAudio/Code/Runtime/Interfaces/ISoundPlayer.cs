using UnityEngine;

namespace ParrotCode.Audio
{
    public interface ISoundPlayer
    {
        void LoadSound(AudioClip clip);
        void Play();
        void Play(AudioClip audio);
        void PlayOnce(AudioClip clip);
        void Stop();
    }
}
