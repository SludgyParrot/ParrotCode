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

using System.Collections.Generic;
using System.Text;
using System.Linq;
using UnityEngine;
using ParrotCode.Native.Common;
using ParrotCode.EventSystem;
using ParrotCode.InputSystem;
using ParrotCode.Native.Shared;

namespace ParrotCode.UI
{ 
    public enum SelectionMode
    {
        Default,
        LockedOnSubmit
    }

    public sealed class UINavigationSystem: SingletonInstance<UINavigationSystem>
    {
        [SerializeField, Space(5)]
        private SelectionMode selectionMode;

        private readonly Dictionary<string, List<ISelectable>> navigationGroups = new Dictionary<string, List<ISelectable>>();
        private List<ISelectable> focusedSelectableGroup;
        private ISelectable focusedSelectableItem;
        private const float NavigationInputDeadzone = 1.0f;
        private bool submitted;

        public SelectionMode SelectionMode => selectionMode;

        #region Unity Callbacks

        private void OnEnable()
        {
            EventBus.AddListener<NavigationViewEvent>(OnFocusedViewEvent);
            EventBus.AddListener<InputActionEvent>(OnNavigationInputEvent);
        }

        private void OnDisable()
        {
            EventBus.RemoveListener<NavigationViewEvent>(OnFocusedViewEvent);
            EventBus.RemoveListener<InputActionEvent>(OnNavigationInputEvent);
        }

        #endregion

        #region Event Listeners

        private void OnFocusedViewEvent(NavigationViewEvent evt)
        {
            if(evt == null)
            {
                Log($"[{gameObject.name}] OnFocusedViewEvent failed. Event argument cannot be null.", LogVerbosity.Error, LogChannel.UI);
                return;
            }

            if (navigationGroups.TryGetValue(evt.ViewID, out List<ISelectable> selectables))
            {
                focusedSelectableGroup = selectables;
                OnSelection(selectables.FirstOrDefault() as Selectable, default);
                submitted = false;
            }
            else
                Log($"[{gameObject.name}] OnFocusedViewEvent failed. Couldn't find view for id: {evt.ViewID}, in the navigationGroups.", LogVerbosity.Error, LogChannel.UI);
        }

        private void OnNavigationInputEvent(InputActionEvent evt)
        {
            string validateNavigationResults = ValidateNavigation(evt);

            if (!string.IsNullOrEmpty(validateNavigationResults))
            {
                Log(validateNavigationResults, LogVerbosity.Error, LogChannel.UI);
                return;
            }

            if (!evt.Performed)
                return;

            Selectable selectedItem = focusedSelectableItem as Selectable;

            Log($"~Input action: {evt.Action}", LogVerbosity.Debug, LogChannel.UI);

            switch (evt.Action)
            {
                case InputActionType.Navigate:
                    if (submitted && SelectionMode == SelectionMode.LockedOnSubmit)
                        return;
                    OnNavigation(selectedItem: selectedItem, navigation: selectedItem.Navigation, inputDirection: evt.InputAxis2D);
                    break;
                case InputActionType.Submit:
                    if (submitted && SelectionMode == SelectionMode.LockedOnSubmit)
                        return;
                    OnSubmit();
                    break;
                case InputActionType.Back:
                case InputActionType.Return:
                case InputActionType.Cancel:
                    OnCancel();
                    break;
            }
        }

        #endregion

        #region Navigation

        private void OnNavigation(Selectable selectedItem, Navigation navigation, Vector2 inputDirection)
        {
            ScreenDirection direction = GetNavigationDirection(inputDirection);

            Log($"~Navigation: {navigation} - direction: {direction}", LogVerbosity.Debug, LogChannel.UI);

            switch (navigation)
            {
                case Navigation.Automatic:
                    PerformAutomaticNavigation(selectedItem, direction);
                    break;
                case Navigation.Horizontal:
                    PerformHorizontalNavigation(selectedItem, direction);
                    break;
                case Navigation.Vertical:
                    PerformVerticalNavigation(selectedItem, direction);
                    break;
                case Navigation.Explicit:
                    PerformExplicitNavigation(selectedItem, direction);
                    break;
                default:
                    Log($"Invalid navigation type: {selectedItem.Navigation}", LogVerbosity.Error, LogChannel.UI);
                    break;
            }
        }

        #endregion

        #region Navigation Validations

        private string PerformValidations(string viewGUID, params ISelectable[] selectables)
        {
            StringBuilder results = new StringBuilder();

            if (string.IsNullOrEmpty(viewGUID))
                results.Append($"Register selectables for guid: {viewGUID} failed. UI view guid cannot be null. ");

            if (selectables == null || selectables.Length == 0)
                results.Append($"Register selectables for guid: {viewGUID} failed. Selectables parameter value cannot be null. ");

            if (this.navigationGroups.ContainsKey(viewGUID))
                results.Append($"Register selectables for guid: {viewGUID} failed. Guid is already registered to the selectables list.");

            return results.ToString();
        }

        private string ValidateNavigation(InputActionEvent evt)
        {
            StringBuilder results = new StringBuilder();

            if (evt == null)
                results.Append($"[{gameObject.name}] OnNavigationInputEvent failed. InputActionEvent argument value is null. ");

            if (focusedSelectableItem == null)
                results.Append($"[{gameObject.name}] OnNavigationInputEvent failed. Selected item is null.");

            return results.ToString();
        }

        #endregion

        private ScreenDirection GetNavigationDirection(Vector2 direction2D)
        {
            ScreenDirection direction = ScreenDirection.Up;

            if (direction2D.y >= NavigationInputDeadzone && direction2D.y > direction2D.x)
                direction = ScreenDirection.Up;

            if (direction2D.y <= -NavigationInputDeadzone && direction2D.y < direction2D.x)
                direction = ScreenDirection.Down;

            if (direction2D.x <= -NavigationInputDeadzone && direction2D.x < direction2D.y)
                direction = ScreenDirection.Left;

            if (direction2D.x >= NavigationInputDeadzone && direction2D.x > direction2D.y)
                direction = ScreenDirection.Right;

            return direction;
        }

        private (Selectable selectable, string errorMessage) GetNearestSelectableFromDirection(Selectable selectable, ScreenDirection direction)
        {
            Selectable foundSelectable = default;
            string errorMessage = foundSelectable == null? $"GetNearestSelectableFromDirection failed. Couldn't find selectable on the: {direction} of {nameof(selectable)}." : string.Empty;
            return (foundSelectable, errorMessage);
        }

        #region Perform Navigations

        private void PerformAutomaticNavigation(Selectable selectable, ScreenDirection direction)
        {
            var nearestSelectableResults = GetNearestSelectableFromDirection(selectable, direction);

            if(!string.IsNullOrEmpty(nearestSelectableResults.errorMessage))
            {
                Log(nearestSelectableResults.errorMessage, LogVerbosity.Error, LogChannel.UI);
                return;
            }

            OnSelection(selectable, nearestSelectableResults.selectable);
        }

        private void PerformHorizontalNavigation(Selectable selectable, ScreenDirection direction)
        {
            switch (direction)
            {
                case ScreenDirection.Left:
                    if (selectable.OnSelectionLeft == null)
                        PerformAutomaticNavigation(selectable, ScreenDirection.Left);
                    else
                        OnSelection(selectable, selectable.OnSelectionLeft);
                    break;
                case ScreenDirection.Right:
                    if (selectable.OnSelectionRight == null)
                        PerformAutomaticNavigation(selectable, ScreenDirection.Right);
                    else
                        OnSelection(selectable, selectable.OnSelectionRight);
                    break;
            }
        }

        private void PerformVerticalNavigation(Selectable selectable, ScreenDirection direction)
        {
            switch (direction)
            {
                case ScreenDirection.Up:
                    if(selectable.OnSelectionUp == null)
                        PerformAutomaticNavigation(selectable, ScreenDirection.Up);
                    else
                        OnSelection(selectable, selectable.OnSelectionUp);
                    break;
                case ScreenDirection.Down:
                    if (selectable.OnSelectionDown == null)
                        PerformAutomaticNavigation(selectable, ScreenDirection.Down);
                    else
                        OnSelection(selectable, selectable.OnSelectionDown);
                    break;
            }
        }

        private void PerformExplicitNavigation(Selectable selectable, ScreenDirection direction)
        {
            switch (direction)
            {
                case ScreenDirection.Up:
                    OnSelection(selectable, selectable.OnSelectionUp);
                    break;
                case ScreenDirection.Down:
                    OnSelection(selectable, selectable.OnSelectionDown);
                    break;
                case ScreenDirection.Left:
                    OnSelection(selectable, selectable.OnSelectionLeft);
                    break;
                case ScreenDirection.Right:
                    OnSelection(selectable, selectable.OnSelectionRight);
                    break;
            }
        }

        #endregion

        #region Selections

        private void OnSelection(Selectable previousSelection, Selectable nextSelection)
        {
            if(nextSelection == null)
            {
                focusedSelectableItem = previousSelection;
            }
            else
            {
                focusedSelectableItem = nextSelection;
                previousSelection.Deselect();
            }

            focusedSelectableItem.Select();
        }

        private void OnSubmit()
        {
            if(focusedSelectableItem == null)
                return;

            focusedSelectableItem.Submit();
            submitted = true;
        }

        private void OnCancel()
        {
            if (focusedSelectableItem == null)
                return;

            focusedSelectableItem.Select();
            submitted = false;
        }

        #endregion

        #region Navigation View Registractions

        public void RegisterSelectables(string viewGUID, params ISelectable[] selectables)
        {
            string validationResults = PerformValidations(viewGUID, selectables);

            if(!string.IsNullOrEmpty(validationResults))
            {
                Log(validationResults, LogVerbosity.Error, LogChannel.UI);
                return;
            }

            navigationGroups[viewGUID] = selectables.ToList();
        }

        public void UnregisterSelectables(params string[] viewGUIDs)
        {
            if(viewGUIDs == null || viewGUIDs.Length == 0)
            {
                Log($"[{gameObject.name}] UnregisterSelectables failed. Navigation GUIDs cannot be null.", LogVerbosity.Error, LogChannel.UI);
                return;
            }

            for(int i = 0; i < viewGUIDs.Length; i++)
            {
                if (string.IsNullOrEmpty(viewGUIDs[i]))
                {
                    Log($"[{gameObject.name}] UnregisterSelectables skipped null/empty GUID at index {i}.", LogVerbosity.Warning, LogChannel.UI);
                    continue;
                }

                if (navigationGroups.ContainsKey(viewGUIDs[i]))
                    navigationGroups.Remove(viewGUIDs[i]);
                else
                    Log($"[{gameObject.name}] UnregisterSelectables failed. Couldn't find view for GUID: {viewGUIDs[i]}.", LogVerbosity.Warning, LogChannel.UI);
            }
        }

        #endregion
    }
}
