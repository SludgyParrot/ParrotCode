using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using ParrotCode.Native.Common;
using ParrotCode.EventSystem;

namespace ParrotCode.InputSystem
{ 
    public sealed class InputManager: SingletonInstance<InputManager>
    {
        [SerializeField, Space(5)]
        private List<InputActionScheme> actionSchemes = new List<InputActionScheme>();

        public IReadOnlyList<InputActionScheme> ActionSchemes => actionSchemes;

        protected override void Init()
        {
            base.Init();

            if (ActionSchemes.Count == 0)
                throw new NullReferenceException($"[{gameObject.name}] Initialization failed. {nameof(ActionSchemes)} cannot be null or empty.");

            if(ActionSchemes.Any(scheme => scheme == null))
                throw new NullReferenceException($"[{gameObject.name}] Initialization Failed. Action schemes cannot contain a null scheme.");

            if (ActionSchemes.Any(scheme => scheme.InputActionConfigs.Any(action => action == null)))
                throw new NullReferenceException($"[{gameObject.name}] Initialization Failed. An action scheme in action schemes contains a null action config.");

            foreach (InputActionScheme scheme in ActionSchemes)
            {
                foreach(InputActionConfig inputAction in scheme.InputActionConfigs)
                {
                    if(inputAction.ActionReference == null)
                    {
                        Log($"[{gameObject.name}] Initialization Failed. Action schemes cannot contain a null action refrence.", LogVerbosity.Error, LogChannel.InputSystem);
                        break;
                    }

                    inputAction.ActionReference.action.Enable();
                    inputAction.ActionReference.action.performed += callback => { OnActionEvent(scheme.Scheme, inputAction.Action, callback, true); };
                    inputAction.ActionReference.action.canceled += callback => { OnActionEvent(scheme.Scheme, inputAction.Action, callback, false); };
                }
            }
        }

        private void OnActionEvent(InputScheme scheme, Action action, InputAction.CallbackContext callback, bool performed)
            => EventBus.InvokeEvent(new InputActionEvent(scheme, action, callback, performed));

        private void OnDestroy()
        {
            foreach (InputActionScheme scheme in ActionSchemes)
            {
                foreach (InputActionConfig inputAction in scheme.InputActionConfigs)
                {
                    if (inputAction.ActionReference == null)
                    {
                        Log($"[{gameObject.name}] Initialization Failed. Action schemes cannot contain a null action refrence.", LogVerbosity.Error, LogChannel.InputSystem);
                        break;
                    }

                    inputAction.ActionReference.action.Disable();
                    inputAction.ActionReference.action.Reset();
                }
            }
        }
    }
}
