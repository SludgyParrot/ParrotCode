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
using UnityEditor;
using UnityEngine;
#endregion

#region Included Parrot Code Assemblies
using ParrotCode.Extensions;
using ParrotCode.Native.Shared;
using System.Diagnostics;
using ParrotCode.Native.SharedEditor;
#endregion

namespace ParrotCode.Platforms
{
    /// <summary>
    /// This is a runtime platform build configurator.
    /// </summary>
    public static class RuntimePlatformBuilder
    {
        /// <summary>
        /// This 
        /// </summary>
        /// <param name="projectBuildConfig"></param>
        /// <returns></returns>
        public static (BuildPlayerOptions options, ProcessStartInfo info, HelpBoxMessage results) GetBuildConfiguration(ProjectBuildConfigGroup projectBuildConfig)
        {
            string applicationName = Application.productName.AddWhiteSpace();
            string buildWindowTitle = $"{applicationName} {projectBuildConfig.BuildTarget.ToString()} {projectBuildConfig.ProjectBuild} Build";

            BuildOutput output = projectBuildConfig.BuildTarget.GetBuildOutput();

            string buildPath = output == BuildOutput.File ? EditorUtility.SaveFilePanel(buildWindowTitle, string.Empty, applicationName, projectBuildConfig.BuildTarget.ToBuildExtension()) 
                : EditorUtility.SaveFolderPanel(buildWindowTitle, string.Empty, applicationName);

            if(buildPath.IsNullOrWhiteSpace())
                return (new BuildPlayerOptions(), null, new HelpBoxMessage($"Build {buildWindowTitle} canceled by user.", MessageType.Warning));

            string buildDirectory = output == BuildOutput.File? Path.GetDirectoryName(buildPath) : buildPath;

            if(!Directory.Exists(buildDirectory))
            {
                string errorMessage = $"Configure build for target: {projectBuildConfig.BuildTarget} failed. " +
                    $"Missing/invalid build {output.ToString()} directory: {buildPath} provided.";

                return (new BuildPlayerOptions(), null, new HelpBoxMessage(errorMessage, MessageType.Error));
            }

            BuildPlayerOptions options = new BuildPlayerOptions();

            // Add configuration for these settings.
            BuildOptions buildOptions = projectBuildConfig.ProjectBuild == Build.Development? BuildOptions.Development | BuildOptions.EnableDeepProfilingSupport : BuildOptions.None;

            options.locationPathName = buildPath;
            options.targetGroup = projectBuildConfig.BuildTarget.ToBuildTargetGroup();
            options.options = buildOptions;

            ProcessStartInfo info = new UnityCommandLineBuildArguments(nameof(PlatformBuilder.BuilPlayer)).ToProcessStartInfo(UnityCommandLineFlags.BuildTarget, projectBuildConfig.BuildTarget.ToString());

            return (options, info, HelpBoxMessage.Empty);
        }
    }
}
