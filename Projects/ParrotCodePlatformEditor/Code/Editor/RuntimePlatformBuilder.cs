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
using System.Linq;
#endregion

#region Included Unity Assemblies
using UnityEditor;
using UnityEngine;
#endregion

#region Included Parrot Code Assemblies
using ParrotCode.Extensions;
using ParrotCode.EditorExtensions;
using ParrotCode.Native;
using ParrotCode.Native.SharedEditor;
#endregion

namespace ParrotCode.Platforms
{
    /// <summary>
    /// Provides helper methods for configuring Unity player builds at runtime
    /// from a <see cref="ProjectBuildConfigGroup"/>.
    /// </summary>
    /// <remarks>
    /// This class is responsible for gathering the output location from the user,
    /// validating the selected destination, and creating a fully configured
    /// <see cref="BuildPlayerOptions"/> instance that can be passed to
    /// <see cref="BuildPipeline.BuildPlayer(BuildPlayerOptions)"/>.
    /// </remarks>
    public static class RuntimePlatformBuilder
    {
        /// <summary>
        /// Creates a <see cref="BuildPlayerOptions"/> instance for the specified
        /// project build configuration.
        /// </summary>
        /// <param name="projectBuildConfig">
        /// The project build configuration that defines the target platform,
        /// build type, and scenes to include.
        /// </param>
        /// <returns>
        /// A tuple containing:
        /// <list type="bullet">
        /// <item>
        /// <description>
        /// A configured <see cref="BuildPlayerOptions"/> instance.
        /// </description>
        /// </item>
        /// <item>
        /// <description>
        /// A <see cref="HelpBoxMessage"/> describing the result of the
        /// configuration operation. If the configuration succeeds,
        /// <see cref="HelpBoxMessage.Empty"/> is returned.
        /// </description>
        /// </item>
        /// </list>
        /// </returns>
        /// <remarks>
        /// Depending on the target platform, the user is prompted to select either
        /// a build file or an output directory. The selected path is validated
        /// before the build options are created.
        ///
        /// For development builds, the returned options automatically enable
        /// <see cref="BuildOptions.Development"/> and
        /// <see cref="BuildOptions.EnableDeepProfilingSupport"/>.
        /// </remarks>
        public static (BuildPlayerOptions options, HelpBoxMessage results) GetBuildConfiguration(
            ProjectBuildConfigGroup projectBuildConfig)
        {
            string applicationName = Application.productName.AddWhiteSpace();

            string buildWindowTitle =
                $"{applicationName} {projectBuildConfig.BuildTarget} {projectBuildConfig.ProjectBuild} Build";

            BuildOutput output = projectBuildConfig.BuildTarget.GetBuildOutput();

            string buildPath = output == BuildOutput.File
                ? EditorUtility.SaveFilePanel(
                    buildWindowTitle,
                    string.Empty,
                    applicationName,
                    projectBuildConfig.BuildTarget.ToBuildExtension())
                : EditorUtility.SaveFolderPanel(
                    buildWindowTitle,
                    string.Empty,
                    applicationName);

            if (buildPath.IsNullOrWhiteSpace())
            {
                return (
                    new BuildPlayerOptions(),
                    new HelpBoxMessage(
                        $"Build {buildWindowTitle} canceled by user.",
                        MessageType.Warning));
            }

            string buildDirectory = output == BuildOutput.File
                ? Path.GetDirectoryName(buildPath)
                : buildPath;

            if (!Directory.Exists(buildDirectory))
            {
                string errorMessage =
                    $"Configure build for target: {projectBuildConfig.BuildTarget} failed. " +
                    $"Missing/invalid build {output} directory: {buildPath} provided.";

                return (
                    new BuildPlayerOptions(),
                    new HelpBoxMessage(errorMessage, MessageType.Error));
            }

            BuildPlayerOptions options = new BuildPlayerOptions();

            // Add configuration for these settings.
            BuildOptions buildOptions =
                projectBuildConfig.ProjectBuild == Build.Development
                    ? BuildOptions.Development | BuildOptions.EnableDeepProfilingSupport
                    : BuildOptions.None;

            options.locationPathName = buildPath;
            options.target = projectBuildConfig.BuildTarget;
            options.targetGroup = projectBuildConfig.BuildTarget.ToBuildTargetGroup();
            options.scenes = projectBuildConfig.BuildScenes.ToArray();
            options.options = buildOptions;

            return (options, HelpBoxMessage.Empty);
        }
    }
}
