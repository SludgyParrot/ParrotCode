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
using System;
#endregion

#region Included Unity Assemblies
using UnityEngine;
using UnityEngine.EventSystems;
#endregion

#region Included Parrot Code Assemblies
using ParrotCode.Native;
#endregion

namespace ParrotCode.UI
{ 
    [DisallowMultipleComponent]
    public sealed class UIDraggableInputHandler: BaseMonoBehaviour, 
        IBeginDragHandler, 
        IDragHandler,
        IDropHandler, 
        IEndDragHandler
    {

        public Action<PointerEventData> OnActionPerformed {  get; private set; }

        public void OnBeginDrag(PointerEventData eventData)
            => OnActionPerformed?.Invoke(eventData);

        public void OnDrag(PointerEventData eventData)
            => OnActionPerformed?.Invoke(eventData);

        public void OnDrop(PointerEventData eventData)
            => OnActionPerformed?.Invoke(eventData);

        public void OnEndDrag(PointerEventData eventData)
            => OnActionPerformed?.Invoke(eventData);
    }
}
