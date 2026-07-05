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
using System.Linq;
using UnityEngine;
#endregion

namespace ParrotCode.Platforms
{
    /// <summary>
    /// Validates <see cref="ProjectBuildConfigGroup"/> assets by executing an
    /// ordered collection of validation rules.
    /// </summary>
    public sealed class ProjectBuildConfigGroupValidator : IEditorAssetValidator
    {
        private readonly IReadOnlyList<IConfigValidationRule<ProjectBuildConfigGroup>> _configValidationRules;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectBuildConfigGroupValidator"/> class.
        /// </summary>
        /// <param name="validationRules">
        /// The validation rules to execute when validating a
        /// <see cref="ProjectBuildConfigGroup"/>.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="validationRules"/> is <see langword="null"/>
        /// or contains no validation rules.
        /// </exception>
        public ProjectBuildConfigGroupValidator(
            params IConfigValidationRule<ProjectBuildConfigGroup>[] validationRules)
        {
            if (validationRules.Length == 0)
            {
                throw new ArgumentException(
                    nameof(validationRules),
                    "At least one validation rule must be provided.");
            }

            var orderedValidationRule = validationRules
                .OrderBy(x => x.Order)
                .ToArray();

            _configValidationRules = orderedValidationRule;
        }

        #region Config Validations

        /// <summary>
        /// Validates the specified editor asset if it is a
        /// <see cref="ProjectBuildConfigGroup"/>.
        /// </summary>
        /// <typeparam name="T">
        /// The type of editor asset to validate.
        /// </typeparam>
        /// <param name="config">
        /// The editor asset to validate.
        /// </param>
        /// <returns>
        /// A <see cref="HelpBoxMessage"/> describing the first validation issue
        /// encountered; otherwise, <see cref="HelpBoxMessage.Empty"/>.
        /// </returns>
        public HelpBoxMessage Validate<T>(T config) where T : class
        {
            ProjectBuildConfigGroup projectBuildConfigGroup = config as ProjectBuildConfigGroup;

            if (projectBuildConfigGroup == null)
                return HelpBoxMessage.Empty;

            foreach (var rule in _configValidationRules)
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
