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
using System;
using System.Collections.Generic;
#endregion

#region Included Unity Assemblies
using UnityEditor;
#endregion

#region Included Parrot Code Assemblies
using ParrotCode.Native.SharedEditor;
#endregion

namespace ParrotCode.Platforms
{
    public sealed class ProjectBuildConfigGroupValidator : IEditorAssetValidator
    {
        private IReadOnlyList<IConfigValidationRule<ProjectBuildConfigGroup>> _configValidationRules;

        public ProjectBuildConfigGroupValidator(params IConfigValidationRule<ProjectBuildConfigGroup>[] validationRules)
        {
            if(validationRules?.Length == 0)
            {
                throw new ArgumentNullException(nameof(validationRules), "Validation initialization failed. Validation rules cannot be null.");
            }

            _configValidationRules = validationRules;
        }

        #region Config Validations

        /// <summary>
        /// This function checks validations the ProjectBuildConfigGroup.
        /// </summary>
        /// <returns>A turple (bool isValid, string message, MessageType messageType) 
        /// Valid if this config's validation was successfull, and 
        /// False if the validation failed, along with a failed message 
        /// and message type e.g <see cref="MessageType"/>.</returns>
        public HelpBoxMessage Validate<T>(T config) where T : class
        {
            ProjectBuildConfigGroup projectBuildConfigGroup = config as ProjectBuildConfigGroup;

            if (projectBuildConfigGroup == null)
                return HelpBoxMessage.Empty;

            foreach(var rule in _configValidationRules)
            {
                var validationResults = rule.Validate(projectBuildConfigGroup);

                if (validationResults.ContainsLog())
                    return validationResults;
            }

            return HelpBoxMessage.Empty;
        }

        #endregion
    }
}
