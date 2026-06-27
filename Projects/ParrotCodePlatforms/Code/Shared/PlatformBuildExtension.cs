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
#endregion

#region Included Unity Assemblies
using UnityEditor;
using UnityEditor.SceneManagement;
#endregion

namespace ParrotCode.Platforms
{
    /// <summary>
    /// 
    /// </summary>
    public static class PlatformBuildExtension
    {
        #region Extension Strings.
        private const string AndroidExtension = "apk";
        private const string iOSExtension = "";
        private const string StandaloneWindowsExtension = "exe";
        private const string WebGLExtension = "";
        #endregion

        private static readonly Dictionary<BuildTarget, string> PlatformExtensions = new Dictionary<BuildTarget, string>
        {
            {
                BuildTarget.Android,
                AndroidExtension
            },
            {
                BuildTarget.iOS,
                iOSExtension
            },
            {
                BuildTarget.StandaloneWindows,
                StandaloneWindowsExtension
            }
            ,
            {
                BuildTarget.StandaloneWindows64,
                StandaloneWindowsExtension
            }
        };

        public static bool GetPlatformExtension(BuildTarget target, out string results)
        {
            if(!PlatformExtensions.TryGetValue(target, out var extension))
            {
                results = string.Empty;
                return false;
            }

            results = extension;
            return true;
        }
    }
}
