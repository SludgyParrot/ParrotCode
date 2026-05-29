using System;
using System.Collections.Generic;
using UnityEngine;

namespace ParrotCode.UI
{
    [CreateAssetMenu(fileName = "UITheme", menuName = "Parrot Code/UI/Theme")]
    public sealed class UITheme: ScriptableObject
    {
        [SerializeField]
        private List<UIState> states = new List<UIState>();

        public List<UIState> States
        {
            get
            {
                if(states.Count == 0)
                    throw new NullReferenceException($"States for: {name} cannot be null.");
                return states;
            }
        }

        public (UIState state, string errorMessage) GetState(UIStateType type)
        {
            try
            {
                UIState state = States.Find(s => s.State.Equals(type));
                string errorMessage = state == null? $"Get state failed. Couldn't find UI state of type: {type} for theme: {name}." : string.Empty;
                return (state, errorMessage);
            }
            catch(Exception exception)
            {
                throw exception;
            }
        }
    }
}
