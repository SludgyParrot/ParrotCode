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
using UnityEngine;
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
    public sealed class DuplicateProjectBuildConfigsValidationRule : IConfigValidationRule<ProjectBuildConfigGroup>
    {
        public int Order => 2;

        public HelpBoxMessage Validate(ProjectBuildConfigGroup config, [CanBeNull] object data = null)
        {
            var duplicatedProjectBuildConfigGroups = config?.ProjectBuildConfigs?.GroupBy(group => group?.GetType()).Where(group => group.Skip(1).Any()).ToArray();

            if (duplicatedProjectBuildConfigGroups.Length == 0)
                return HelpBoxMessage.Empty;

            List<string> validationErrorMessages = new List<string>();

            foreach (var duplicatedProjectBuildConfigGroup in duplicatedProjectBuildConfigGroups)
            {
                string groupKeyName = duplicatedProjectBuildConfigGroup?.Key?.Name ?? $"None ({SharedProjectNames.DefaultProjectBuildConfigName})";
                int duplicatedProjectBuildConfigCount = duplicatedProjectBuildConfigGroup.Count();
                var duplicatedProjectBuildConfigGroupNames = duplicatedProjectBuildConfigGroup.Select(config => $"* {config?.name ?? groupKeyName}").ToArray();
                string duplicatedProjectBuildConfigNames = string.Join("\n", duplicatedProjectBuildConfigGroupNames);

                string groupValidationErrorMessage = $"[Duplicated '{groupKeyName}' detected] {config.name}" +
                    $" contains {duplicatedProjectBuildConfigCount} copies of {groupKeyName}. \nPlease remove one of the following listed {groupKeyName} " +
                    $"from the list: \n\n{duplicatedProjectBuildConfigNames}\n";

                validationErrorMessages.Add(groupValidationErrorMessage);
            }

            string validationErrorMessage = string.Join("\n", validationErrorMessages);

            return new HelpBoxMessage(validationErrorMessage, MessageType.Error, CustomInspectorValidations.EnabledWideHelpBox);
        }
    }
}
