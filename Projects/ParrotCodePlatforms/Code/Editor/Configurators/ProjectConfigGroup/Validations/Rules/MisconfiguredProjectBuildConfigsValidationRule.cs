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
using JetBrains.Annotations;
#endregion

namespace ParrotCode.Platforms
{
    public sealed class MisconfiguredProjectBuildConfigsValidationRule : IConfigValidationRule<ProjectBuildConfigGroup>
    {
        public int Order => 4;

        public HelpBoxMessage Validate(ProjectBuildConfigGroup config, [CanBeNull] object data = null)
        {
            //var invalidatedProjectBuildConfigs = configurator.ProjectBuildConfigs.OfType<ProjectSpecificBuildConfig>().Where(config => config.Validate().ContainsLog()).ToArray();

            //if (invalidatedProjectBuildConfigs.Length == 0)
            //    return HelpBoxMessage.Empty;

            //var defaultInvalidatedProjectBuildConfig = invalidatedProjectBuildConfigs.FirstOrDefault();
            //var validationResults = defaultInvalidatedProjectBuildConfig.Validate();

            //string validationMessage = $"[{validationResults.MessageType} Log] \n\nFile Name [{defaultInvalidatedProjectBuildConfig.name}]\n";
            //string projectBuildConfigFilePath = $"File Path: {AssetDatabase.GetAssetPath(defaultInvalidatedProjectBuildConfig)}\n";
            //string validationErrorMessage = string.Join("\n", validationMessage, validationResults.Message, projectBuildConfigFilePath);

            //return new HelpBoxMessage(validationErrorMessage, validationResults.MessageType, CustomInspectorValidations.EnabledWideHelpBox);
            return HelpBoxMessage.Empty;
        }
    }
}
