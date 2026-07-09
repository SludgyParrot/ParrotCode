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
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
#endregion

#region Included Parrot Code Assemblies
using ParrotCode.Native.SharedEditor;
using System.IO;
using ParrotCode.Extensions;
using System.Linq;
using System;
using System.Text;
using System.Threading.Tasks;
using ParrotCode.Helpers.Storage;
#endregion

namespace ParrotCode.Platforms
{
    public static class ProjectBuildConfigurator
    {
        #region Project Build Configurators

        private static WindowsProjectConfigurator sharedWindowsProjectConfigurator = new WindowsProjectConfigurator();

        private static readonly Dictionary<BuildTarget, IProjectSpecificConfig> projectConfigurators = new Dictionary<BuildTarget, IProjectSpecificConfig>()
        {
            { BuildTarget.Android, new AndroidProjectConfigurator()},
            { BuildTarget.iOS, new IOSProjectConfigurator()},
            { BuildTarget.StandaloneWindows, sharedWindowsProjectConfigurator},
            { BuildTarget.StandaloneWindows64, sharedWindowsProjectConfigurator},
            { BuildTarget.WebGL, new WebGLProjectConfigurator()}
        };

        #endregion

        public static async Task ApplyProjectSettingsAndBuild(ProjectBuildConfigGroup settingsGroup)
        {
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                return;

            #region Apply Target Specific Settings

            if (!projectConfigurators.TryGetValue(settingsGroup.BuildTarget, out IProjectSpecificConfig projectConfigurator))
            {
                Debug.LogWarning($"Apply target specific settings for: {settingsGroup.BuildTarget} failed. " +
                   $"Project configuration for build target: {settingsGroup.BuildTarget} is not currently supported.");

                return;
            }

            projectConfigurator.Configure(settingsGroup.ProjectBuildConfigs);
            AssetDatabase.SaveAssets();

            #endregion

            #region Initialize Build

            var buildConfiguration = RuntimePlatformBuilder.GetBuildConfiguration(settingsGroup);

            if (buildConfiguration.results.ContainsLog())
            {
                Debug.LogWarning(buildConfiguration.results.Message);
                return;
            }

            CreateBuildConfig(buildConfiguration.options);

            int buildExitCode = await PlatformBuilder.InitializeBuild();
            Debug.Log($"Build completed with exit code: {buildExitCode}");

            #endregion
        }

        private static void CreateBuildConfig(BuildPlayerOptions options)
        {
            Storage.SerializeToJsonFile(SharedProjectDirectories.TemporaryBuildConfigPath, 
                new ProjectBuildOptions(options));
        }
    }
}
