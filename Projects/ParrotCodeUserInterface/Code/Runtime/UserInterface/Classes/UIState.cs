using System;
using UnityEngine;

namespace ParrotCode.UI
{
    [Serializable]
    public class UIState
    {
        [field: SerializeField]
        public string Name { get; private set; }

        [field: SerializeField, Space(5)]
        public Color Color { get; private set; } = Color.white;

        [field: SerializeField, Space(5)]
        public Color BackgroundColor { get; private set; } = Color.white;

        [field: SerializeField, Space(5)]
        public Sprite Image { get; private set; }

        [field: SerializeField, Space(5)]
        public UIStateType State { get; private set; }
    }
}
