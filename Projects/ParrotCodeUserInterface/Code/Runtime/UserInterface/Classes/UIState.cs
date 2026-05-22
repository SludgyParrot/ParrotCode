using System;
using UnityEngine;

namespace ParrotCode.UI
{
    [Serializable]
    public abstract class UIState
    {
        [SerializeField]
        private string stateId;

        [SerializeField, Space(5)]
        private UIStateType state;

        [SerializeField, Space(5)]
        private Color color;

        [Header("Optionals"), SerializeField, Space(5)]
        private bool useOptionals;

        public string StateId
        {
            get
            {
                if (string.IsNullOrEmpty(stateId))
                    throw new NullReferenceException("StateId cannot be null for UIState.");
                return stateId;
            }
        }

        public UIStateType State => state;
        public Color Color => color;
        public bool UseOptionals => useOptionals;

        protected UIState() { }

        protected UIState(string stateId, UIStateType state, Color color, bool useOptionals = false)
        {
            this.stateId = stateId;
            this.state = state;
            this.color = color;
            this.useOptionals = useOptionals;
        }
    }
}
