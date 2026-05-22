using System;
using UnityEngine;

namespace ParrotCode.UI
{ 
    [Serializable]
    public class UIImageState: UIState
    {
        [SerializeField, Space(5)]
        private Sprite stateImage;

        public Sprite StateImage
        {
            get
            {
                if (stateImage == null)
                    throw new NullReferenceException($"StateImage for state id: {StateId} cannot be null for UIState.");
                return stateImage;
            }
        }

        public UIImageState(): base() { }

        public UIImageState(Sprite stateImage, string stateId, UIStateType state, Color color, bool useOptionals = false) : base(stateId, state, color, useOptionals)
            => this.stateImage = stateImage;
    }
}
