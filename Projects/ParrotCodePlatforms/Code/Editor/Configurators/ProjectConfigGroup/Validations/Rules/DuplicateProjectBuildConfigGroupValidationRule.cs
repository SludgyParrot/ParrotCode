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
    public sealed class DuplicateProjectBuildConfigGroupValidationRule : IConfigValidationRule<ProjectBuildConfigGroup>
    {
        public int Order => 0;

        public HelpBoxMessage Validate(ProjectBuildConfigGroup config, [CanBeNull] object data = null)
        {
            //var foundProjectBuildConfigDuplicatesResults = ProjectAssetsDatabaseUtility.GetProjectConfigForBuildDuplicatePaths(this);

            //if (foundProjectBuildConfigDuplicatesResults.MessageType == MessageType.None)
            //    return HelpBoxMessage.Empty;

            //string[] duplicatedProjectBuildConfigPaths = foundProjectBuildConfigDuplicatesResults.Value.Where(x => x != AssetDatabase.GetAssetPath(this)).ToArray();

            //int configCount = foundProjectBuildConfigDuplicatesResults.Value.Length;

            //string duplicatedConfigPaths = string.Join("\n", duplicatedProjectBuildConfigPaths);

            //string validationErrorMessage = $"Multiple copies detected! \n\nThere are {configCount} copies of this instance found in the project, and only '1' instance is allowed per project. " +
            //       $"This config, along with '{duplicatedProjectBuildConfigPaths.Length - 1}' additional copy/copies, will not be applied, and the tools option for '{ProjectBuild}' will be disabled." +
            //       $" Please remove this or any of the below listed files. \n\n Duplicated file(s): \n\n{duplicatedConfigPaths}\n";

            //return new HelpBoxMessage(validationErrorMessage, MessageType.Error, CustomInspectorValidations.EnabledWideHelpBox);

            return default;
        }
    }
}
