using UnityEngine;

namespace ParrotCode.Audio
{
    [CreateAssetMenu(fileName = "SoundSettings", menuName = "Parrot Code/Sound/Settings")]
    public sealed class SoundSettings: ScriptableObject
    {
        [field: SerializeField]
        public float Volume { get; private set; }

        [field: SerializeField, Space(5)]
        public float Pitch { get; private set; }

        [field: SerializeField, Space(5)]
        public bool Loop { get; private set; }

        [field: SerializeField, Space(5)]
        public bool PlayOnAwake { get; private set; }
    }
}
