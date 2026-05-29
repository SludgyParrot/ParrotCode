using System;
using System.Collections.Generic;
using System.Linq;

namespace ParrotCode.UI
{
    public sealed class UIStateMachine
    {
        private readonly Dictionary<UIStateType, UIState> states = new Dictionary<UIStateType, UIState>();

        public UIState CurrentState { get; private set; }
        public delegate void UIStateDelegate(UIState state);

        public UIStateDelegate OnStateChanged;

        public UIStateMachine(UITheme theme)
        {
            try
            {
                states = theme.States.ToDictionary(state => state.State);
            }
            catch(Exception exception)
            {
                throw exception;
            }
        }

        public void SetState(UIStateType stateType)
        {
            if (states.TryGetValue(stateType, out var state))
            {
                CurrentState = state;
                OnStateChanged?.Invoke(state);
            }
            else
                throw new NullReferenceException($"Set state: {stateType} failed. Couldn't find state of type on the assigned theme.");
        }
    }
}
