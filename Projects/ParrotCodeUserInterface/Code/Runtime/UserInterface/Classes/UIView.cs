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

#region Included System Assemblies
using System.Collections.Generic;
using System.Linq;
#endregion

#region Included Unity Assemblies
using UnityEngine;
using UnityEngine.UI;
#endregion

#region Included Parrot Code Assemblies
using ParrotCode.Native;
using ParrotCode.EventSystem;
using ParrotCode.Native.SharedEditor;
#endregion

namespace ParrotCode.UI
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(CanvasScaler))]
    [RequireComponent(typeof(GraphicRaycaster))]
    [RequireComponent(typeof(CanvasGroup))]
    [RequireComponent(typeof(UINavigationHandler))]
    public sealed class UIView : BaseMonoBehaviour, IUIView
    {
        [SerializeField, Space(5)]
        private ViewType viewType;

        [SerializeField, Space(5)]
        private List<Selectable> selectables = new List<Selectable>();

        public IEnumerable<ISelectable> Selectables
        {
            get
            {
                foreach(Selectable selectableObject in selectables)
                    if (selectableObject is ISelectable)
                        yield return selectableObject;
            }
        }

        private UINavigationHandler navigationHandler;

        public UINavigationHandler NavigationHandler
        {
            get
            {
                if(navigationHandler == null)
                    navigationHandler = GetComponent<UINavigationHandler>();
                return navigationHandler;
            }
        }

        private Canvas uiViewCanvas;
        public Canvas UIViewCanvas
        {
            get
            {
                if (uiViewCanvas == null)
                    uiViewCanvas = GetComponent<Canvas>();
                return uiViewCanvas;
            }
        }

        private string viewGUID;
        private string ViewGUID
        {
            get
            {
                if(string.IsNullOrEmpty(viewGUID))
                    viewGUID = gameObject.GetInstanceID().ToString();
                return viewGUID;
            }
        }

        private void OnEnable()
        {
            OnFocus();
        }

        public void SetRenderMode(RenderMode renderMode)
            => UIViewCanvas.renderMode = renderMode;

        [Button]
        public void OnFocus()
        {
            if (viewType == ViewType.Navigation)
                EventBus.InvokeEvent(new NavigationViewEvent(ViewGUID, selectables));
        }

        public void OnBlur()
        {
            
        }

        public void FetchViewRootSelectables() => selectables = GetComponentsInChildren<Selectable>().ToList();
    }
}
