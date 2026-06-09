/*

Parrot Code
Copyright (c) 2026 Sludgy Parrot (Pty) Ltd. All Rights Reserved.

This source code is proprietary and confidential software owned by
Sludgy Parrot (Pty) Ltd.

Parrot Code is a commercial software product developed and distributed
by Sludgy Parrot (Pty) Ltd.

Unauthorized copying, modification, distribution, sublicensing,
reverse engineering, decompilation, disclosure, or use of this
software, in whole or in part, is strictly prohibited without
prior written permission from Sludgy Parrot (Pty) Ltd.

This software is provided under the terms of a separate license
agreement. Possession of this source code does not grant any rights
to use, modify, distribute, or create derivative works unless
explicitly authorized by a valid written license.

THE SOFTWARE IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, EXCEPT AS REQUIRED BY APPLICABLE LAW.

For licensing inquiries:
licensing@sludgyparrot.com

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
            var duplicates = duplicatedSchemes.Select(duplicateGroup => duplicateGroup.Key).ToList();

            if(duplicatedSchemes.Count > 0)
            {
                foreach ( var duplicatedScheme in duplicatedSchemes)
                    errorMessageString.Append($"Found {duplicatedScheme.Count()} duplicates for: {duplicatedScheme.Key}");
            }

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
            cachedInputActions.Clear();
        }

        #endregion

        #region Events

        private void OnActionEvent(InputScheme scheme, InputActionType action, InputAction.CallbackContext callback, bool performed)
        {
            switch(action)
            {
                case InputActionType.Move:
                case InputActionType.Look:
                case InputActionType.Navigate:
                    EventBus.InvokeEvent(new InputActionEvent(scheme: scheme, action: action, inputAxis: default, inputAxis2D: callback.action.ReadValue<Vector2>(), performed: performed));
                    break;
                case InputActionType.Accelerate:
                case InputActionType.Brake:
                    EventBus.InvokeEvent(new InputActionEvent(scheme: scheme, action: action, inputAxis: callback.action.ReadValue<float>(), inputAxis2D: default, performed: performed));
                    break;
                default:
                    EventBus.InvokeEvent(new InputActionEvent(scheme: scheme, action: action, inputAxis: default, inputAxis2D: default, performed: performed));
                    break;
            }    
        }

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
            => cachedInputActions.TryGetValue(action, out InputAction inputAction)? inputAction.ReadValue<T>() : default;

        public bool TryGetIsPressed(InputActionType action, out bool pressed)
        {
            if (cachedInputActions.TryGetValue(action, out InputAction inputAction))
            {
                pressed = inputAction.IsPressed();
                return true;
            }

            pressed = false;
            return false;
        }

        public bool TryGetInputValue<T>(InputActionType action, out T value) where T : struct
        {
            if (cachedInputActions.TryGetValue(action, out InputAction inputAction))
            {
                value = inputAction.ReadValue<T>();
                return true;
            }

            value = default;
            return false;
        }

        public float GetAxis(InputActionType action)
            => GetInputValue<float>(action);

        public Vector2 GetAxis2D(InputActionType action)
            => GetInputValue<Vector2>(action);

        #endregion
    }
}
