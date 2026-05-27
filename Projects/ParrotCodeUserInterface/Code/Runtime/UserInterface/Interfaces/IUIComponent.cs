using System;
using UnityEngine;

namespace ParrotCode.UI
{
    public interface IUIComponent<TType> where TType: class
    {
        public TType Value { get; }

        void SetColor(Color color);
        void SetUIState(UIStateType stateType, Action<(UIState state, string errorMessage)> actionCallback = null);
        void Create<T>(string label = "UI Component", Action<T> createdInstanceCallback = null) where T : UIComponent<T>;
        T Create<T>(string label = "UI Component") where T : UIComponent<T>;
        void Add<T>(UIComponent<T> component, bool keepComponentPosition = false) where T: class;
    }
}
