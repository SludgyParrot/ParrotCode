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
using UnityEditor;
#endregion

namespace ParrotCode.Native.SharedEditor
{
    /// <summary>
    /// Provides shared helper methods for displaying common Unity Editor confirmation dialogs
    /// and handling user confirmation workflows.
    /// </summary>
    public static class SharedCustomInspectorEditorPopup
    {
        /// <summary>
        /// Displays a confirmation dialog and returns the user's selection.
        /// </summary>
        /// <param name="popUpTitle">
        /// The title displayed in the confirmation dialog.
        /// </param>
        /// <param name="popUpMessage">
        /// The message displayed in the confirmation dialog.
        /// </param>
        /// <param name="confirmButtonTitle">
        /// The label displayed on the confirmation button. Defaults to <c>"Yes Please!"</c>.
        /// </param>
        /// <param name="cancelButtonTitle">
        /// The label displayed on the cancel button. Defaults to <c>"No Thanks!"</c>.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the user confirms the operation; otherwise,
        /// <see langword="false"/>.
        /// </returns>
        public static bool ShowApplySettingsConfirmationPopup(
            string popUpTitle,
            string popUpMessage,
            string confirmButtonTitle = SharedCustomEditorStringInfo.ConfirmButtonTitle,
            string cancelButtonTitle = SharedCustomEditorStringInfo.CancelButtonTitle)
            => EditorUtility.DisplayDialog(
                popUpTitle,
                popUpMessage,
                confirmButtonTitle,
                cancelButtonTitle);
    }
}
