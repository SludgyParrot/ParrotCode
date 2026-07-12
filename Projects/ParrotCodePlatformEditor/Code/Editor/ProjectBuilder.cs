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
using System.IO;
#endregion

#region Included Unity Assemblies
using UnityEngine;
using UnityEditor;
using UnityEditor.Build.Reporting;
#endregion

#region Included Parrot Code Assemblies
using ParrotCode.Helpers;
#endregion

namespace ParrotCode.Platforms
{
    /// <summary>
    /// Provides functionality for building a Unity player using a serialized
    /// <see cref="BuildPlayerOptions"/> configuration.
    /// </summary>
    public static class ProjectBuilder
    {
        /// <summary>
        /// Builds a Unity player using the build configuration stored at
        /// <see cref="SharedProjectDirectory.TemporaryBuildConfigPath"/>.
        /// </summary>
        /// <remarks>
        /// This method deserializes the build configuration, invokes the Unity
        /// build pipeline, logs a summary of the build results, and throws an
        /// exception if the build does not complete successfully.
        /// </remarks>
        /// <exception cref="IOException">
        /// Thrown if the serialized build configuration cannot be read.
        /// </exception>
        /// <exception cref="Exception">
        /// Thrown if the Unity player build does not complete successfully.
        /// </exception>
        public static void BuildPlayer()
        {
            BuildPlayerOptions buildOptions = (BuildPlayerOptions)Storage.DeserializeFromJsonFile<ProjectBuildOptions>(SharedProjectDirectory.TemporaryBuildConfigPath);
            BuildReport report = BuildPipeline.BuildPlayer(buildOptions);

            Debug.Log($"{report.summary.platform} build started time: {report.summary.buildStartedAt}");
            Debug.Log($"Build total size: {report.summary.totalSize}");
            Debug.Log($"Build output path: {report.summary.outputPath}");
            Debug.Log($"{report.summary.platform} build completed time: {report.summary.buildEndedAt}");
            Debug.Log($"Build results: {report.summary.result}");
            Debug.Log($"Build total warnings found: {report.summary.totalWarnings}");
            Debug.Log($"Build total errors found: {report.summary.totalErrors}");

            if (report.summary.result != BuildResult.Succeeded)
            {
                throw new Exception($"{report.summary.platform} player build failed with results: {report.summary.result}.");
            }
        }
    }
}
