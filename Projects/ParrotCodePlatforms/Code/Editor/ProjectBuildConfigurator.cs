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

using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace ParrotCode.Platforms
{
    public static class ProjectBuildConfigurator
    {
        private const string DevelopmentSettingsRootPath = ProjectSharedDirectory.ProjectSettingsToolsMenuRoot + "Development";
        private const string ProductionSettingsRootPath = ProjectSharedDirectory.ProjectSettingsToolsMenuRoot + "Production";

        private const string ProjectBuildConfigGroupConfigFilterName = "t:ProjectBuildConfigGroup";

        private static readonly Dictionary<BuildTarget, IProjectConfigurator> projectConfigurators = new Dictionary<BuildTarget, IProjectConfigurator>()
        {
            { BuildTarget.Android, new AndroidProjectConfigurator()},
            { BuildTarget.iOS, new IOSProjectConfigurator()},
            { BuildTarget.StandaloneWindows, new WindowsProjectConfigurator()},
            { BuildTarget.StandaloneWindows64, new WindowsProjectConfigurator()},
            { BuildTarget.WebGL, new WebGLProjectConfigurator()}
        };

        #region Configuration Menu

        [MenuItem(DevelopmentSettingsRootPath)]
        private static void ConfigureProjectDevelopmentSettings()
            => ApplyProjectSettings(Build.Development);

        [MenuItem(ProductionSettingsRootPath)]
        private static void ConfigureProjectProductionSettings()
            => ApplyProjectSettings(Build.Production);

        [MenuItem(DevelopmentSettingsRootPath, true)]
        private static bool ValidateProjectDevelopmentSettings()
        {
            var projectSettings = GetBuildProjectConfigForBuild(EditorUserBuildSettings.activeBuildTarget, Build.Development);
            return string.IsNullOrEmpty(projectSettings.errorMessage);
        }

        [MenuItem(ProductionSettingsRootPath, true)]
        private static bool ValidateProjectProductionSettings()
        {
            var projectSettings = GetBuildProjectConfigForBuild(EditorUserBuildSettings.activeBuildTarget, Build.Production);
            return string.IsNullOrEmpty(projectSettings.errorMessage);
        }

        #endregion

        private static void ApplyProjectSettings(Build build)
        {
            BuildTarget target = EditorUserBuildSettings.activeBuildTarget;

            var projectSettings = GetBuildProjectConfigForBuild(target, build);

            if (!string.IsNullOrEmpty(projectSettings.errorMessage))
            {
                Debug.LogError(projectSettings.errorMessage);
                return;
            }

            ProjectBuildConfigGroup settingsGroup = projectSettings.config;

            #region Apply Target Specific Settings
            if (projectConfigurators.TryGetValue(settingsGroup.BuildTarget, out IProjectConfigurator projectConfigurator))
                projectConfigurator.Configure(settingsGroup.ProjectBuildConfigs);
            else
                Debug.LogError($"Apply target specific settings for: {settingsGroup.BuildTarget} failed. Project configuration for build target: {settingsGroup.BuildTarget} is not currently supported.");
            #endregion

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }


        #region Build Project Configs

        private static (ProjectBuildConfigGroup config, string errorMessage) GetBuildProjectConfigForBuild(BuildTarget target, Build build)
        {
            string[] projectConfigGuids = AssetDatabase.FindAssets(ProjectBuildConfigGroupConfigFilterName);
            ProjectBuildConfigGroup[] projectConfigs = projectConfigGuids.Select(guid => AssetDatabase.LoadAssetAtPath<ProjectBuildConfigGroup>(AssetDatabase.GUIDToAssetPath(guid))).ToArray();
            ProjectBuildConfigGroup[] matchedConfigs = projectConfigs.Where(x => x.BuildTarget == target && x.ProjectBuild == build).ToArray();

            if (matchedConfigs.Length == 1)
                return (matchedConfigs[0], string.Empty);

            return (null, $"Get build project config for build: {build} targeting: {target} failed. There are {matchedConfigs.Length} build project config(s) found for target.");
        }

        #endregion
    }
}
