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
#endregion

#region Included Parrot Code Assemblies
using ParrotCode.Extensions;
using ParrotCode.Native;
using ParrotCode.Helpers;
using System;
#endregion

namespace ParrotCode.Platforms
{
    /// <summary>
    /// Provides shared paths used by the platform build pipeline.
    /// </summary>
    /// <remarks>
    /// This class exposes commonly used directories and files for temporary build
    /// configuration, logging, and output. The paths are resolved relative to the
    /// current Unity project's <see cref="Application.dataPath"/>.
    /// </remarks>
    public static class SharedProjectDirectory
    {
        /// <summary>
        /// Gets the path to the temporary build configuration file.
        /// </summary>
        /// <value>
        /// The absolute path of the JSON file used to persist
        /// <see cref="ProjectBuildOptions"/> between build stages.
        /// </value>
        public static string TemporaryBuildConfigPath =>
            Path.Combine(
                Application.persistentDataPath,
                "Build",
                $"BuildConfig{FileExtension.JSON.Extension()}");

        /// <summary>
        /// Gets the path to the temporary build log file.
        /// </summary>
        /// <value>
        /// The absolute path of the log file used to store build output and
        /// diagnostic information.
        /// </value>
        public static string TemporaryBuildLogFilePath => 
            Path.Combine(
                Application.persistentDataPath,
               "Build",
               $"Build{FileExtension.Log.Extension()}");

        /// <summary>
        /// Gets the directory to the build log file.
        /// </summary>
        /// <value>
        /// The absolute directory of the log file used to store build output and
        /// diagnostic information.
        /// </value>
        public static string BuildLogFileDirectory =>
         Path.Combine(
             Application.dataPath,
             "Build");

        /// <summary>
        /// Gets the path to the temporary project backup log file.
        /// </summary>
        /// <value>
        /// The absolute path of the log file used to store project backup information.
        /// </value>
        public static string TemporaryProjectBackupLogFilePath =>
            Path.Combine(
                Application.persistentDataPath,
                "Build",
                $"Backup{FileExtension.Log.Extension()}");

        /// <summary>
        /// Gets the configured build output directory.
        /// </summary>
        /// <value>
        /// The output location specified by the serialized
        /// <see cref="ProjectBuildOptions"/> stored in the temporary build
        /// configuration file.
        /// </value>
        /// <exception cref="IOException">
        /// Thrown if the temporary build configuration file cannot be read.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the build configuration could not be deserialized.
        /// </exception>
        public static string BuildFolderDirectory => 
            GetBuildFolderDirectory();

        /// <summary>
        /// Retrieves the configured build output directory from the temporary build
        /// configuration file.
        /// </summary>
        /// <returns>
        /// The configured build output directory if the build configuration is loaded
        /// successfully; otherwise, <see langword="null"/>.
        /// </returns>
        /// <remarks>
        /// This method attempts to deserialize the <see cref="ProjectBuildOptions"/>
        /// stored at <see cref="TemporaryBuildConfigPath"/> and returns its
        /// <see cref="ProjectBuildOptions.LocationPathName"/>. If the build
        /// configuration cannot be loaded due to an invalid path, an I/O error, or a
        /// deserialization failure, a warning is logged and
        /// <see langword="null"/> is returned.
        /// </remarks>
        private static string? GetBuildFolderDirectory()
        {
            try
            {
                return Path.GetDirectoryName(Storage.DeserializeFromJsonFile<ProjectBuildOptions>(
                    TemporaryBuildConfigPath).LocationPathName);
            }
            catch (Exception exception)
            when (exception is ArgumentException ||
                  exception is IOException ||
                  exception is InvalidOperationException)
            {
                Debug.LogWarning($"Failed to retrieve project build options: {exception.Message}");

                return null;
            }
        }
    }
}
