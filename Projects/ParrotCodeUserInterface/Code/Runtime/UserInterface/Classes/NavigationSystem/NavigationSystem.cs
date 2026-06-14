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

using ParrotCode.EventSystem;
using ParrotCode.InputSystem;
using ParrotCode.Native.Common;
using ParrotCode.Native.Shared;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ParrotCode.UI
{
    [DisallowMultipleComponent]
    public sealed class NavigationSystem : BaseMonoBehaviour
    {
        [SerializeField, Space(5)]
        private SelectionMode selectionMode;

        private readonly Dictionary<string, List<ISelectable>> navigationGroups = new Dictionary<string, List<ISelectable>>();
        private List<ISelectable> selectableGroup;
        private ISelectable selected;
        private bool submitted;

        private NavigationCommandDisplater navigationCommand;
        private NavigationStrategy navigationStrategy;

        public SelectionMode SelectionMode => selectionMode;

        #region Unity Callbacks

        private void OnEnable()
        {
            EventBus.AddListener<NavigationViewEvent>(OnFocused);
            EventBus.AddListener<InputActionEvent>(OnInputEvent);
        }

        private void OnDisable()
        {
            EventBus.RemoveListener<NavigationViewEvent>(OnFocused);
            EventBus.RemoveListener<InputActionEvent>(OnInputEvent);
        }

        #endregion

        protected override void Init()
        {
            navigationCommand = new NavigationCommandDisplater(this);
            navigationStrategy = new NavigationStrategy(this);
        }

        private void OnFocused(NavigationViewEvent evt)
        {
            if (evt == null)
            {
                Log($"[{gameObject.name}] OnFocusedViewEvent failed. Event argument cannot be null.", LogVerbosity.Error, LogChannel.UI);
                return;
            }

            submitted = false;

            if (navigationGroups.TryGetValue(evt.ViewID, out List<ISelectable> selectables))
            {
                selectableGroup = selectables;
                selected = selectables.FirstOrDefault() as Selectable;
              
            }
            else
            {
                List<ISelectable> selectableList = evt.Selectables.ToList();

                selectableGroup = selectableList;
                selected = selectableList.FirstOrDefault() as Selectable;
                navigationGroups[evt.ViewID] = selectableList;
            }

            Select(selected);
        }

        private void OnInputEvent(InputActionEvent evt) => navigationCommand.Dispatch(evt);

        public void Cancel()
        {
            submitted = false;
        }
       

        public void Navigate(ScreenDirection direction)
        {
            Log($"~Navigate direction: {direction}", LogVerbosity.Debug, LogChannel.UI);
            navigationStrategy.Navigate(selected as Selectable, direction);
        }

        public void Select(ISelectable selectable)
        {
            selected = selectable;
            selected?.Select();
        }

        public void Deselect(ISelectable selectable) => selectable?.Deselect();

        public void Submit()
        {
            selected?.Submit();
            submitted = true;
        }
    }
}
