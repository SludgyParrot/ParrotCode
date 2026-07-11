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

namespace ParrotCode.Platforms
{
    public sealed class ProjectBuildRenderingConfigValidator : IEditorAssetValidator
    {

        private List<IConfigValidationRule<ProjectBuildRenderingConfig>> _configValidationRules;

        public ProjectBuildRenderingConfigValidator(params IConfigValidationRule<ProjectBuildRenderingConfig>[] validationRules)
        {
            _configValidationRules = validationRules.OrderBy(config => config.Order).ToList();
        }

        #region Configuration Validation API
        /// <summary>
        /// Validation for platform specific rendering settings.
        /// </summary>
        /// <returns>Return InspectorValidationResults <see cref="InspectorValidationResults"/></returns>
        public HelpBoxMessage Validate<T>(T config) where T : class
        {

            ProjectBuildRenderingConfig renderingConfig = config as ProjectBuildRenderingConfig;

            if (renderingConfig == null)
            {
                return HelpBoxMessage.Empty;
            }

            #region Validate Platform Settings
            HelpBoxMessage validationResults = ValidatePlatformSpecificSettings(renderingConfig, out IRenderingProjectBuildConfig renderingProjectBuild);

            if (validationResults.ContainsLog())
                return validationResults;

            foreach (var validationRule in _configValidationRules)
            {
                var results = validationRule.Validate(renderingConfig, renderingProjectBuild);

                if (results.ContainsLog())
                    return results;
            }

            return HelpBoxMessage.Empty;

            #endregion
        }
        #endregion

        #region Configuration Validations

        private HelpBoxMessage ValidatePlatformSpecificSettings(ProjectBuildRenderingConfig config, out IRenderingProjectBuildConfig? renderingProjectBuild)
        {
            if (!config.RenderingSettings.TryGetValue(config.BuildTarget, out var projectBuildConfig))
            {
                renderingProjectBuild = null;
                string validationErrorMessage = $"Rendering settings are currently not supported in this version of the framework for target build: {config.BuildTarget}.";
                return new HelpBoxMessage(validationErrorMessage, MessageType.Warning);
            }

            renderingProjectBuild = projectBuildConfig;
            return HelpBoxMessage.Empty;
        }

        #endregion
    }
}
