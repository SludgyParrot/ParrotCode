using System;
using UnityEngine;

namespace ParrotCode.UI
{
    public interface IUIComponent<TType> where TType: class
    {
        public TType Value { get; }

        void SetColor(Color color);
        void SetUIState(UIStateType stateType, Action<(UIState state, string errorMessage)> actionCallback = null);
        void Add<T>(UIComponent<T> component) where T: class;
    }
}
