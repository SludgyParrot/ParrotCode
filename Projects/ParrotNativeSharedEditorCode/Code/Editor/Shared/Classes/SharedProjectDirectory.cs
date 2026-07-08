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
using System.IO;
#endregion

#region Included Unity Assemblies
using UnityEngine;
using UnityEditor;
#endregion


#region Included Parrot Code Assemblies
using ParrotCode.Extensions;
#endregion

namespace ParrotCode.Native.SharedEditor
{
    /// <summary>
    /// Provides commonly used Unity project directory paths and shared directory-related constants.
    /// </summary>
    /// <remarks>
    /// This class centralizes project path utilities and common directory information
    /// used throughout the Parrot Code editor framework.
    /// </remarks>
    public static class SharedProjectDirectory
    {
        /// <summary>
        /// Gets the full path to the Unity Editor executable currently running
        /// the project.
        /// </summary>
        /// <returns>
        /// The absolute path to the Unity Editor executable.
        /// </returns>
        public static string GetUnityEditorApplicationPath()
            => EditorApplication.applicationPath;

        /// <summary>
        /// Gets the root directory of the current Unity project.
        /// </summary>
        /// <returns>
        /// The absolute path to the project's root directory.
        /// </returns>
        /// <remarks>
        /// This is the parent directory of the project's
        /// <see cref="Application.dataPath"/> (<c>Assets</c>) folder.
        /// </remarks>
        public static string GetRootProjectPath()
            => Path.GetDirectoryName(Application.dataPath);

        /// <summary>
        /// Gets the temporary location for storing the current Unity project during build.
        /// </summary>
        /// <returns></returns>
        public static string GetTemporaryBuildProjectPath()
        {
            string destination = $"{Application.productName.RemoveWhiteSpace()}";
            return Path.Combine(Path.GetTempPath(), destination);
        }
    }
}
