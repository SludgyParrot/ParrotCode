/*

MIT License

Copyright (c) 2026 Sludgy Parrot

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE. 

*/

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using ParrotCode.Native.Common;
using ParrotCode.EventSystem;
using System.Text;

namespace ParrotCode.InputSystem
{ 
    public sealed class InputManager: SingletonInstance<InputManager>
    {
        [SerializeField, Space(5)]
        private List<InputActionScheme> actionSchemes = new List<InputActionScheme>();

        private readonly Dictionary<InputAction, (Action<InputAction.CallbackContext> performed, Action<InputAction.CallbackContext> canceled)> subscribedInputActionEvents 
            = new Dictionary<InputAction, (Action<InputAction.CallbackContext> performed, Action<InputAction.CallbackContext> canceled)>();

        private readonly Dictionary<InputActionType, InputAction> cachedInputActions 
            = new Dictionary<InputActionType, InputAction>();

        public IReadOnlyList<InputActionScheme> ActionSchemes => actionSchemes;

        #region Subscriptions

        private void OnEnable()
            => RegisterInputBindings();

        private void OnDisable()
            => UnregisterInputBindings();

        #endregion

        #region Initialization

        protected override void Init()
        {
            base.Init();

            string errorMessage = ValidateConfigs();

            if (!string.IsNullOrEmpty(errorMessage))
            {
                Log($"[{gameObject.name}] {errorMessage}", LogVerbosity.Error, LogChannel.InputSystem);
                return;
            }
        }

        private string ValidateConfigs()
        {
            StringBuilder errorMessageString = new StringBuilder();

            if (ActionSchemes == null || ActionSchemes.Count == 0)
                errorMessageString.Append($"{nameof(ActionSchemes)} cannot be null or empty. ");

            if (ActionSchemes.Any(scheme => scheme == null))
                errorMessageString.Append($"Action schemes cannot contain a null scheme. ");

            if (ActionSchemes.Any(scheme => scheme?.InputActionConfigs == null || scheme?.InputActionConfigs.Count == 0 || scheme.InputActionConfigs.Any(action => action == null)))
                errorMessageString.Append($"An action scheme in action schemes contains a null action config. ");

            var duplicatedSchemes = ActionSchemes.SelectMany(scheme => scheme.InputActionConfigs).GroupBy(action => action.Action).Where(group => group.Count() > 1).ToList();

            if(duplicatedSchemes.Count > 0)
                errorMessageString.Append($"There are {duplicatedSchemes.Count} input action scheme config duplicates found.");

            return errorMessageString.ToString();
        }

        #endregion

        #region Bindings

        private void RegisterInputBindings()
        {
            if(subscribedInputActionEvents.Count > 0)
            {
                Log($"[{gameObject.name}] RegisterInputBindings failed. subscribedInputActionEvents already contains {subscribedInputActionEvents.Count} registered events.", LogVerbosity.Warning, LogChannel.InputSystem);
                return;
            }

            foreach (InputActionScheme scheme in ActionSchemes)
            {
                foreach (InputActionConfig inputAction in scheme.InputActionConfigs)
                {
                    if (inputAction.ActionReference == null)
                    {
                        Log($"[{gameObject.name}] RegisterInputBindings failed. Action schemes cannot contain a null action refrence.", LogVerbosity.Error, LogChannel.InputSystem);
                        continue;
                    }

                    if(subscribedInputActionEvents.ContainsKey(inputAction?.ActionReference?.action))
                    {
                        Log($"[{gameObject.name}] RegisterInputBindings failed. subscribedInputActionEvents already contains a key '{nameof(inputAction.ActionReference.action)}' for scheme: {scheme.name} of type: {scheme.Scheme}.", LogVerbosity.Warning, LogChannel.InputSystem);
                        continue;
                    }

                    InputAction action = inputAction.ActionReference.action;

                    InputScheme currentScheme = scheme.Scheme;
                    InputActionType currentAction = inputAction.Action;

                    action.Enable();

                    Action<InputAction.CallbackContext> performedAction = callback => { OnActionEvent(currentScheme, currentAction, callback, true); };
                    Action<InputAction.CallbackContext> canceledAction = callback => { OnActionEvent(currentScheme, currentAction, callback, false); };

                    action.performed += performedAction;
                    action.canceled += canceledAction;

                    subscribedInputActionEvents[action] = (performedAction, canceledAction);
                    cachedInputActions[currentAction] = action;
                }
            }
        }

        private void UnregisterInputBindings()
        {
            foreach (InputActionScheme scheme in ActionSchemes)
            {
                foreach (InputActionConfig inputAction in scheme.InputActionConfigs)
                {
                    if (inputAction.ActionReference == null)
                    {
                        Log($"[{gameObject.name}] UnregisterInputBindings issue. Action schemes cannot contain a null action refrence for '{nameof(inputAction)}'.", LogVerbosity.Error, LogChannel.InputSystem);
                        continue;
                    }

                    if (!subscribedInputActionEvents.TryGetValue(inputAction?.ActionReference?.action, out var subscribedInputActionEvent))
                    {
                        Log($"[{gameObject.name}] Unsusbscribe input action events failed. Couldn't find subscribed input action event for '{nameof(inputAction)}'.", LogVerbosity.Warning, LogChannel.InputSystem);
                        continue;
                    }

                    InputAction action = inputAction.ActionReference.action;

                    action.performed -= subscribedInputActionEvent.performed;
                    action.canceled -= subscribedInputActionEvent.canceled;
                    action.Disable();

                    subscribedInputActionEvents.Remove(action);
                }
            }

            subscribedInputActionEvents.Clear();
        }

        #endregion

        #region Events

        private void OnActionEvent(InputScheme scheme, InputActionType action, InputAction.CallbackContext callback, bool performed)
            => EventBus.InvokeEvent(new InputActionEvent(scheme, action, callback, performed));

        #endregion

        #region API 

        public bool HasInputAction(InputActionType action)
            => cachedInputActions.ContainsKey(action);

        public bool IsPressed(InputActionType action)
            => cachedInputActions.TryGetValue(action, out InputAction inputAction) && inputAction.IsPressed();

        public bool WasPressedThisFrame(InputActionType action)
            => cachedInputActions.TryGetValue(action, out InputAction inputAction) && inputAction.WasPressedThisFrame();

        public bool WasReleasedThisFrame(InputActionType action)
            => cachedInputActions.TryGetValue(action, out InputAction inputAction) && inputAction.WasReleasedThisFrame();

        public bool WasPerformedThisFrame(InputActionType action)
            => cachedInputActions.TryGetValue(action, out InputAction inputAction) && inputAction.WasPerformedThisFrame();

        public T GetInputValue<T>(InputActionType action) where T : struct
            => cachedInputActions.TryGetValue(action, out InputAction inputAction)? inputAction.ReadValue<T>() : new T();

        public void TryGetIsPressed(InputActionType action, out bool pressed)
        {
            if (cachedInputActions.TryGetValue(action, out InputAction inputAction))
                pressed = inputAction.IsPressed();
            else
                throw new InvalidOperationException($"[{gameObject.name}] Get input press for action: {action} failed. Action was not found in the cachedInputActions.");
        }

        public void TryGetInputValue<T>(InputActionType action, out T value) where T : struct
        {
            if (cachedInputActions.TryGetValue(action, out InputAction inputAction))
                value = inputAction.ReadValue<T>();
            else
                throw new InvalidOperationException($"[{gameObject.name}] Get input value '{nameof(T)}' for action: {action} failed. Action was not found in the cachedInputActions.");
        }

        #endregion
    }
}
