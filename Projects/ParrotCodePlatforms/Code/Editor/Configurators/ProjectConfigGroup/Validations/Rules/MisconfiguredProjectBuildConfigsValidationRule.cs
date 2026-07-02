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
using System.Linq;
#endregion

#region Included Unity Assemblies
using UnityEditor;
#endregion

#region Included Parrot Code Assemblies
using ParrotCode.Native.SharedEditor;
#endregion

#region Included Jet Brains Assemblies
using JetBrains.Annotations;
#endregion

namespace ParrotCode.Platforms
{
    /// <summary>
    /// Validates the project build configurations contained within a
    /// <see cref="ProjectBuildConfigGroup"/> and reports the first
    /// misconfigured configuration encountered.
    /// </summary>
    public sealed class MisconfiguredProjectBuildConfigsValidationRule : IConfigValidationRule<ProjectBuildConfigGroup>
    {
        private readonly ProjectBuildRenderingConfigValidationManager _validationManager =
            new ProjectBuildRenderingConfigValidationManager();

        /// <summary>
        /// Gets the execution order of this validation rule.
        /// </summary>
        public int Order => 4;

        /// <summary>
        /// Validates all project-specific build configurations contained in the specified
        /// <see cref="ProjectBuildConfigGroup"/>.
        /// </summary>
        /// <param name="config">
        /// The project build configuration group to validate.
        /// </param>
        /// <param name="data">
        /// Optional contextual data used during validation.
        /// </param>
        /// <returns>
        /// A <see cref="HelpBoxMessage"/> describing the first validation issue found;
        /// otherwise, <see cref="HelpBoxMessage.Empty"/> if all configurations are valid.
        /// </returns>
        public HelpBoxMessage Validate(ProjectBuildConfigGroup config, [CanBeNull] object data = null)
        {
            ProjectSpecificBuildConfig[] projectBuildConfigs =
                config.ProjectBuildConfigs.OfType<ProjectSpecificBuildConfig>().ToArray();

            for (int i = 0; i < projectBuildConfigs.Length; i++)
            {
                ProjectSpecificBuildConfig projectBuildConfig = projectBuildConfigs[i];

                var validationResults = GetValidationMessage(projectBuildConfig);

                if (validationResults.ContainsLog())
                    return validationResults;
            }

            return HelpBoxMessage.Empty;
        }

        /// <summary>
        /// Creates a formatted validation message for the specified project build configuration.
        /// </summary>
        /// <param name="projectBuildConfig">
        /// The project build configuration to validate.
        /// </param>
        /// <returns>
        /// A formatted <see cref="HelpBoxMessage"/> containing the validation result,
        /// configuration name, and asset path.
        /// </returns>
        private HelpBoxMessage GetValidationMessage(ProjectSpecificBuildConfig projectBuildConfig)
        {
            HelpBoxMessage validationResults = _validationManager.Validate(projectBuildConfig);

            string validationMessage =
                $"[{validationResults.MessageType} Log] \n\nFile Name [{projectBuildConfig.name}]\n";
            string projectBuildConfigFilePath =
                $"File Path: {AssetDatabase.GetAssetPath(projectBuildConfig)}\n";
            string validationErrorMessage = string.Join(
                "\n",
                validationMessage,
                validationResults.Message,
                projectBuildConfigFilePath);

            return new HelpBoxMessage(
                validationErrorMessage,
                validationResults.MessageType,
                CustomInspectorValidations.EnabledWideHelpBox);
        }
    }
}
