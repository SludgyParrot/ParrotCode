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
using ParrotCode.Native.SharedEditor;
#endregion

namespace ParrotCode.Platforms
{
    /// <summary>
    /// Custom editor for the Android build target configuration. 
    /// This editor provides a user-friendly interface for configuring Android projects in the Unity Editor. 
    /// It allows developers to easily set up build enviroment for ANdroid development, including options for mobile VR,
    /// By using this custom editor, 
    /// developers can streamline the process of creating and maintaining Android projects, 
    /// ensuring a smoother development experience and better system outcomes.
    /// </summary>
    public sealed class AndroidPlatformSelector : EditorWindow
    {
        private const string WindowMenuPath = SharedProjectDirectory.ParrotCodeRootPath + "Android/Configure Project Environment";

        [MenuItem(WindowMenuPath)]
        private static void ConfigureAndroidProjectEnvironment()
        {
            if(!BuildPlatformHandler.SwitchBuildPlatform(BuildTarget.Android, BuildTargetGroup.Android))
            {
                return;
            }

        }
    }
}
