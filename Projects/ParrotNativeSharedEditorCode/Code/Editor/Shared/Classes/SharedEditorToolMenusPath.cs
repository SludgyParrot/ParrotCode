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

#region Included Parrot Code Assemblies
using ParrotCode.Native.Shared;
#endregion

namespace ParrotCode.Native.SharedEditor
{
    /// <summary>
    /// Provides shared directory paths and utility methods used by the Parrot Code project configuration system.
    /// </summary>
    public static class SharedEditorToolMenusPath
    {
        /// <summary>
        /// Root menu and asset path for the Parrot Code framework.
        /// </summary>
        public const string ParrotCodeRootPath = "Parrot Code/";

        /// <summary>
        /// Root path containing Parrot Code configuration assets.
        /// </summary>
        public const string ParrotCodeConfigRootPath =
            ParrotCodeRootPath + "Config/";

        /// <summary>
        /// Root path containing platform-specific project configuration assets.
        /// </summary>
        public const string PlatformConfigRootPath = ParrotCodeConfigRootPath + "Platforms/";

        /// <summary>
        /// Root menu path used for Project Build commands in the Unity Editor.
        /// </summary>
        public const string ProjectSettingsToolsMenuRoot = ParrotCodeRootPath + "Project/Build/";
    }
}
