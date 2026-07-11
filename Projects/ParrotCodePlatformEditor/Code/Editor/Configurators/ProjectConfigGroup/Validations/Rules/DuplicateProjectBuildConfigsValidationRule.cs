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
#endregion

#region Included Parrot Code Assemblies
using ParrotCode.Native.SharedEditor;
#endregion

#region Included Jet Brains Assemblies
using JetBrains.Annotations;
using System;
#endregion

namespace ParrotCode.Platforms
{
    /// <summary>
    /// Validates that a <see cref="ProjectBuildConfigGroup"/> does not contain
    /// duplicate <see cref="ProjectBuildConfig"/> types.
    /// </summary>
    public sealed class DuplicateProjectBuildConfigsValidationRule : IConfigValidationRule<ProjectBuildConfigGroup>
    {
        /// <summary>
        /// Gets the execution order of this validation rule.
        /// </summary>
        public int Order => 2;

        /// <summary>
        /// Validates the specified <see cref="ProjectBuildConfigGroup"/> for duplicate
        /// <see cref="ProjectBuildConfig"/> types.
        /// </summary>
        /// <param name="config">
        /// The project build configuration group to validate.
        /// </param>
        /// <param name="data">
        /// Optional contextual data used during validation.
        /// </param>
        /// <returns>
        /// A <see cref="HelpBoxMessage"/> describing the first duplicate configuration
        /// found; otherwise, <see cref="HelpBoxMessage.Empty"/>.
        /// </returns>
        public HelpBoxMessage Validate(ProjectBuildConfigGroup config, [CanBeNull] object data = null)
        {
            var duplicatedProjectBuildConfigGroups = config?.ProjectBuildConfigs?
                .GroupBy(group => group?.GetType())
                .Where(group => group.Skip(1).Any())
                .ToArray();

            if (duplicatedProjectBuildConfigGroups.Length == 0)
                return HelpBoxMessage.Empty;

            foreach (var duplicatedProjectBuildConfigGroup in duplicatedProjectBuildConfigGroups)
            {
                HelpBoxMessage validationResults =
                    GetValidationMessage(duplicatedProjectBuildConfigGroup, config.name);

                if (validationResults.ContainsLog())
                    return validationResults;
            }

            return HelpBoxMessage.Empty;
        }

        /// <summary>
        /// Creates a validation message describing the duplicate project build
        /// configurations found within the specified group.
        /// </summary>
        /// <param name="configGroup">
        /// The grouped collection of duplicate project build configurations.
        /// </param>
        /// <param name="configName">
        /// The name of the project build configuration group being validated.
        /// </param>
        /// <returns>
        /// A formatted <see cref="HelpBoxMessage"/> describing the duplicate
        /// configurations.
        /// </returns>
        private HelpBoxMessage GetValidationMessage(
            IGrouping<Type, ProjectBuildConfig> configGroup,
            string configName)
        {
            string groupKeyName = configGroup?.Key?.Name ??
                $"None ({SharedProjectNames.DefaultProjectBuildConfigName})";

            int duplicatedProjectBuildConfigCount = configGroup.Count();

            var duplicatedProjectBuildConfigGroupNames = configGroup
                .Select(config => $"* {config?.name ?? groupKeyName}")
                .ToArray();

            string duplicatedProjectBuildConfigNames =
                string.Join("\n", duplicatedProjectBuildConfigGroupNames);

            string groupValidationErrorMessage =
                $"[Duplicated '{groupKeyName}' detected] {configName}" +
                $" contains {duplicatedProjectBuildConfigCount} copies of {groupKeyName}. \n" +
                $"Please remove one of the following listed {groupKeyName} from the list: \n\n" +
                $"{duplicatedProjectBuildConfigNames}\n";

            return new HelpBoxMessage(
                groupValidationErrorMessage,
                MessageType.Error,
                CustomInspectorValidations.EnabledWideHelpBox);
        }
    }
}
