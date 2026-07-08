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
    /// Provides Unity Editor menu commands for applying and validating Parrot Code project build configurations.
    /// </summary>
    public static class ProjectBuildConfiguratorEditor
    {
        private static readonly ProjectBuildConfigGroupValidationManager _validationManager = new ProjectBuildConfigGroupValidationManager();

        #region Unity Menu Commands
        /// <summary>
        /// Applies the Development project configuration for the active build target.
        /// </summary>
        [MenuItem(SharedCustomEditorStringInfo.DevelopmentSettingsRootPath)]
        private static void ConfigureProjectDevelopmentSettings()
        {
            var projectSettings = ProjectAssetsDatabaseUtility.GetProjectConfiguratorForBuild<ProjectBuildConfigGroup>(
               EditorUserBuildSettings.activeBuildTarget,
               Build.Development);

            ProjectBuildConfigurator.ApplyProjectSettingsAndBuild(projectSettings.Value);
        }

        /// <summary>
        /// Applies the Production project configuration for the active build target.
        /// </summary>
        [MenuItem(SharedCustomEditorStringInfo.ProductionSettingsRootPath)]
        private static void ConfigureProjectProductionSettings()
        {
            var projectSettings = ProjectAssetsDatabaseUtility.GetProjectConfiguratorForBuild<ProjectBuildConfigGroup>(
               EditorUserBuildSettings.activeBuildTarget,
               Build.Production);

            ProjectBuildConfigurator.ApplyProjectSettingsAndBuild(projectSettings.Value);
        }

        /// <summary>
        /// Determines whether the Development project configuration can be applied for the active build target.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if a valid Development configuration exists for the active build target; otherwise,
        /// <see langword="false"/>.
        /// </returns>
        [MenuItem(SharedCustomEditorStringInfo.DevelopmentSettingsRootPath, true)]
        private static bool ValidateProjectDevelopmentSettings()
        {
            var projectSettings = ProjectAssetsDatabaseUtility.GetProjectConfiguratorForBuild<ProjectBuildConfigGroup>(
              EditorUserBuildSettings.activeBuildTarget,
              Build.Development);

            if (projectSettings.Value == null)
                return false;

            var validationResults = _validationManager.Validate(projectSettings.Value);

            return validationResults.MessageType != MessageType.Error;
        }

        /// <summary>
        /// Determines whether the Production project configuration can be applied for the active build target.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if a valid Production configuration exists for the active build target; otherwise,
        /// <see langword="false"/>.
        /// </returns>
        [MenuItem(SharedCustomEditorStringInfo.ProductionSettingsRootPath, true)]
        private static bool ValidateProjectProductionSettings()
        {
            var projectSettings = ProjectAssetsDatabaseUtility.GetProjectConfiguratorForBuild<ProjectBuildConfigGroup>(
               EditorUserBuildSettings.activeBuildTarget,
               Build.Production);

            if(projectSettings.Value == null)
                return false;

            var validationResults = _validationManager.Validate(projectSettings.Value);

            return validationResults.MessageType != MessageType.Error;
        }
        #endregion
    }
}
