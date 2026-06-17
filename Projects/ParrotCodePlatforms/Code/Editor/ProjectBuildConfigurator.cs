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

        private const string ProjectConfigurationWarningPopUpTitle = "Parrot Code: Configure Project Settings";

        private static string ProjectConfigurationWarningPopUpMessage = $"This operation will configure the Unity project's settings to a predefined {0} configuration data. " +
            "This action will override existing settings and this action may not be undone. Do you wish to proceed?";

        private const string ProjectConfigurationWarningPopUpConfirmButtonTitle = "Yes Please!";
        private const string ProjectConfigurationWarningPopUpCancelButtonTitle = "No Thanks!";

        private const string ProjectBuildConfigGroupConfigSearchFilter = "t:ProjectBuildConfigGroup";

        private static WindowsProjectConfigurator sharedWindowsProjectConfigurator = new WindowsProjectConfigurator();

        private static readonly Dictionary<BuildTarget, IProjectConfigurator> projectConfigurators = new Dictionary<BuildTarget, IProjectConfigurator>()
        {
            { BuildTarget.Android, new AndroidProjectConfigurator()},
            { BuildTarget.iOS, new IOSProjectConfigurator()},
            { BuildTarget.StandaloneWindows, sharedWindowsProjectConfigurator},
            { BuildTarget.StandaloneWindows64, sharedWindowsProjectConfigurator},
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
            string warningMessage = string.Format(ProjectConfigurationWarningPopUpMessage, build.ToString());

            if (!EditorUtility.DisplayDialog(ProjectConfigurationWarningPopUpTitle, warningMessage, ProjectConfigurationWarningPopUpConfirmButtonTitle, ProjectConfigurationWarningPopUpCancelButtonTitle))
                return;

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
            ProjectBuildConfigGroup[] projectConfigs = GetBuildProjectConfigs().Where(x => x.BuildTarget == target && x.ProjectBuild == build).ToArray();

            if (projectConfigs.Length == 1)
                return (projectConfigs[0], string.Empty);

            return (null, $"Get build project config for build: {build} targeting: {target} failed. There are {projectConfigs.Length} build project config(s) found for target.");
        }

        private static ProjectBuildConfigGroup[] GetBuildProjectConfigs()
        {
            string[] projectConfigGuids = AssetDatabase.FindAssets(ProjectBuildConfigGroupConfigSearchFilter);
            ProjectBuildConfigGroup[] projectConfigs = projectConfigGuids.Select(guid => AssetDatabase.LoadAssetAtPath<ProjectBuildConfigGroup>(AssetDatabase.GUIDToAssetPath(guid))).ToArray();
            return projectConfigs;
        }

        #endregion

        #region Configs
        public static (bool hasCopies, string[] paths) GetProjectBuildConfigGroupDuplicatesCount(ProjectBuildConfigGroup configGroup)
        {
            var duplicatePaths = GetBuildProjectConfigs().Where(x => x.BuildTarget == configGroup.BuildTarget && x.ProjectBuild == configGroup.ProjectBuild).Select(x => AssetDatabase.GetAssetPath(x)).ToArray();

            int count = duplicatePaths.Length;
            bool hasCopies = count > 1;
            return (hasCopies, duplicatePaths);
        }
        #endregion
    }
}
