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

using UnityEngine;

namespace ParrotCode.Native.SharedEditor
{
    /// <summary>
    /// Provides shared layout options and styling values for custom Unity
    /// Editor inspectors.
    /// </summary>
    /// <remarks>
    /// This class centralizes commonly used <see cref="GUILayoutOption"/>s and
    /// visual styling values to ensure a consistent appearance across custom
    /// inspectors.
    /// </remarks>
    public static class CustomInspectorGUILayout
    {
        #region GUI Layouts

        /// <summary>
        /// Gets the default height applied to inspector buttons.
        /// </summary>
        /// <value>
        /// A <see cref="GUILayoutOption"/> that sets the button height to
        /// <c>50</c> pixels.
        /// </value>
        public static readonly GUILayoutOption DefaultInspectorButtonLayoutHeight =
            GUILayout.Height(50.0f);

        #endregion

        #region GUI Styles

        /// <summary>
        /// Gets or sets the foreground color used when rendering apply settings
        /// buttons.
        /// </summary>
        /// <value>
        /// The text color for apply settings buttons.
        /// </value>
        public static readonly Color ApplySettingsButtonColor = Color.white;

        /// <summary>
        /// Gets or sets the background color used when rendering apply settings
        /// buttons.
        /// </summary>
        /// <value>
        /// The background color for apply settings buttons.
        /// </value>
        public static readonly Color ApplySettingsButtonBackgroundColor = Color.gray;

        #endregion
    }
}
