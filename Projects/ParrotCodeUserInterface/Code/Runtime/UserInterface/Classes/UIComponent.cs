using System;
using UnityEngine;
using ParrotCode.Native.Common;

namespace ParrotCode.UI
{
    [DisallowMultipleComponent]
    public abstract class UIComponent<TType> : BaseMonoBehaviour, IUIComponent<TType> where TType: class
    {
        [SerializeField, Space(5)]
        protected string identifier;

        protected TType value;

        public string Identifier
        {
            get
            {
                if (string.IsNullOrEmpty(identifier))
                    Log("Identifier cannot be null for: ", LogVerbosity.Exception, LogChannel.UI, gameObject.name);
                return identifier;
            }
        }

        public TType Value
        {
            get
            {
                if (value == null)
                {
                    value = GetComponent<TType>();
                    if (value == null)
                        Log("Component cannot be null for: ", LogVerbosity.Exception, LogChannel.UI, gameObject.name);
                }
                return value;
            }
        }

        public abstract void SetColor(Color color);
        public abstract void SetUIState(UIStateType stateType, Action<(UIState state, string errorMessage)> actionCallback = null);

        public virtual void Add<T>(UIComponent<T> component) where T: class
        {
            if (component == null)
                throw new ArgumentNullException($"Add component failed. Couldn't add UI component to: {gameObject.name}. Parameter value is null.");

            component.transform.SetParent(transform);
        }
    }
}
