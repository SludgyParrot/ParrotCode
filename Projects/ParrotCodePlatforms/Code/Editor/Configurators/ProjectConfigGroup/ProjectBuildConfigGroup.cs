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
using System.Linq;
#endregion

#region Included Unity Assemblies
using UnityEditor;
using UnityEngine;
#endregion

#region Included Parrot Code Assemblies
using ParrotCode.Native.SharedEditor;
#endregion

namespace ParrotCode.Platforms
{
    /// <summary>
    /// Represents a collection of project build configuration assets for a specific
    /// Unity build target and build type.
    /// </summary>
    /// <remarks>
    /// A <see cref="ProjectBuildConfigGroup"/> acts as the entry point for applying
    /// and validating a set of <see cref="ProjectBuildConfig"/> assets.
    /// </remarks>
    [CreateAssetMenu(
        fileName = "Project Configuration",
        menuName = SharedEditorToolMenusPath.PlatformConfigRootPath + "Project Configuration")]
    public sealed class ProjectBuildConfigGroup : ScriptableObject, IProjectConfigurator
    {
        [SerializeField]
        private BuildTarget _buildTarget;

        [SerializeField, Space(5)]
        private Build _projectBuild;

        [Header("Build Options")]
        [SerializeField, Space(5)]
        private string[] _buildScenes;

        [Header("Build Settings")]
        [SerializeField, Space(5)]
        private List<ProjectBuildConfig> _projectBuildSettings;

        /// <summary>
        /// Gets the Unity build target associated with this configuration group.
        /// </summary>
        public BuildTarget BuildTarget => _buildTarget;

        /// <summary>
        /// Gets the build type associated with this configuration group.
        /// </summary>
        public Build ProjectBuild => _projectBuild;

        /// <summary>
        /// Gets the scenes to include with the build.
        /// </summary>
        public IReadOnlyList<string> BuildScenes => _buildScenes;

        /// <summary>
        /// Gets the name of this configuration asset.
        /// </summary>
        public string Name => name;

        /// <summary>
        /// Gets the collection of project build configurations contained within this group.
        /// </summary>
        public IReadOnlyList<ProjectBuildConfig> ProjectBuildConfigs => _projectBuildSettings;

        private readonly string _projectConfigurationWarningPopupTitle =
            string.Join(
                " ",
                SharedCustomEditorStringInfo.ProjectConfigurationPopupTitle,
                SharedCustomEditorStringInfo.ProjectSettingsTitle);

        private readonly string _projectConfigurationWarningPopupMessage =
            string.Format(
                SharedCustomEditorStringInfo.ProjectConfigurationPopupMessage,
                SharedCustomEditorStringInfo.ProjectSettingsTitle);

        /// <summary>
        /// Displays a confirmation dialog before applying the project configuration.
        /// </summary>
        public void ApplySettings()
        {
            if (!SharedCustomInspectorEditorPopup.ShowApplySettingsConfirmationPopup(
                    _projectConfigurationWarningPopupTitle,
                    _projectConfigurationWarningPopupMessage))
            {
                return;
            }
        }
    }
}