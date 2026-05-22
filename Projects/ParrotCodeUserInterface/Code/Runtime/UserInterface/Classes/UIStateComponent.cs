using System;
using System.Collections.Generic;
using UnityEngine;
using ParrotCode.Native.Common;

namespace ParrotCode.UI
{
    public abstract class UIStateComponent<TTYpe, TState>: UIComponent<TTYpe> where TTYpe : class where TState : UIState
    {
        [field: SerializeField, Space(5)]
        protected virtual List<TState> States {  get; set; }

        public virtual void SetUIState(UIStateType stateType, Action<(TState state, string errorMessage)> actionCallback = null)
        {
            var eventStateResults = GetUIStateOfType(stateType);

            if (!string.IsNullOrEmpty(eventStateResults.errorMessage))
            {
                Log(eventStateResults.errorMessage, LogVerbosity.Error, LogChannel.InputSystem);
                actionCallback?.Invoke((null, eventStateResults.errorMessage));
            }
            else
            {
                SetColor(eventStateResults.state.Color);
                actionCallback?.Invoke((eventStateResults.state, eventStateResults.errorMessage));
            }
        }

        protected (TState state, string errorMessage) GetUIStateOfType(UIStateType stateType)
        {
            string errorMessage = string.Empty;
            TState state = null;

            try
            {
                state = States.Find(x => x.State == stateType);
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
            }

            return (state, errorMessage);
        }
    }
}
