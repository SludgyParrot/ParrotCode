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
    /// <summary>
    /// Validates that only a single <see cref="ProjectBuildConfigGroup"/> exists
    /// for a given build target and build configuration.
    /// </summary>
    public sealed class DuplicateProjectBuildConfigGroupValidationRule :
        IConfigValidationRule<ProjectBuildConfigGroup>
    {
        /// <summary>
        /// Gets the execution order of this validation rule.
        /// </summary>
        public int Order => 0;

        /// <summary>
        /// Validates the specified <see cref="ProjectBuildConfigGroup"/> for duplicate
        /// configuration assets within the project.
        /// </summary>
        /// <param name="config">
        /// The project build configuration group to validate.
        /// </param>
        /// <param name="data">
        /// Optional contextual data used during validation.
        /// </param>
        /// <returns>
        /// A <see cref="HelpBoxMessage"/> describing any duplicate configuration
        /// groups found; otherwise, <see cref="HelpBoxMessage.Empty"/>.
        /// </returns>
        public HelpBoxMessage Validate(
            ProjectBuildConfigGroup config,
            [CanBeNull] object data = null)
        {
            var foundProjectBuildConfigDuplicatesResults =
                ProjectAssetsDatabaseUtility.GetProjectConfigForBuildDuplicatePaths(config);

            return new HelpBoxMessage(
                foundProjectBuildConfigDuplicatesResults.Message,
                foundProjectBuildConfigDuplicatesResults.MessageType,
                CustomInspectorValidations.EnabledWideHelpBox);
        }
    }
}
