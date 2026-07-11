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

#region Included Unity Assemblies
using UnityEngine;
#endregion

#region Included Parrot Code Assemblies
using ParrotCode.Native;
#endregion

namespace ParrotCode.UI
{
    /// <summary>
    /// Base class for selectable UI components.
    /// </summary>
    public abstract class Selectable : BaseMonoBehaviour, ISelectable
    {
        [SerializeField, Space(5)]
        private Navigation navigation;

        [SerializeField, Space(5)]
        private Selectable onSelectionUp,
                           onSelectionDown,
                           onSelectionLeft,
                           onSelectionRight;

        public Navigation Navigation => navigation;

        public Selectable OnSelectionUp => onSelectionUp;
        public Selectable OnSelectionDown => onSelectionDown;
        public Selectable OnSelectionLeft => onSelectionLeft;
        public Selectable OnSelectionRight => onSelectionRight;

        /// <summary>
        /// This function is triggered during a selection.
        /// </summary>
        public abstract void Select();

        /// <summary>
        /// This function is triggered during unselect event.
        /// </summary>
        public abstract void Deselect();

        /// <summary>
        /// This function is triggered during a submission.
        /// </summary>
        public abstract void Submit();
    }
}
