using System;
using UnityEngine;

namespace ParrotCode.UI
{
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
    }
}
