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

#region Included Systems Assemblies
using System.Linq;
#endregion

#region Included Unity Assemblies
using UnityEngine;
using UnityEditor;
#endregion

namespace ParrotCode.Native.SharedEditor
{
    public sealed class ProjectConfiguratorServices
    {
        private readonly ProjectConfiguratorRepository projectConfigRepository;

        public ProjectConfiguratorServices(ProjectConfiguratorRepository projectConfigRepository)
            => this.projectConfigRepository = projectConfigRepository;

        public ConfigValidationResults<T> GetProjectConfigForBuild<T>(BuildTarget buildTarget, Build build) where T : IProjectConfigurator
        {
            var projectConfigs = projectConfigRepository.GetByBuild<T>(buildTarget, build).ToArray();

            if(projectConfigs?.Length == 1)
            {
                return new ConfigValidationResults<T>(MessageType.None, string.Empty, projectConfigs[0]);
            }

            string warningMessage = $"Get build project config for build: {build} targeting: {buildTarget}" +
                $" failed. There are {projectConfigs?.Length ?? 0} build project config(s) found for target.";

            return new ConfigValidationResults<T>(MessageType.Warning, warningMessage, default);
        }

        public ConfigValidationResults<string[]> GetProjectConfigForBuildDuplicatePaths<T>(BuildTarget buildTarget, Build build) where T : IProjectConfigurator
        {
            var duplicatedProjectBuildConfigGroups = projectConfigRepository.GetByBuild<T>(buildTarget, build).Where(buildConfigGroup =>
              buildConfigGroup.BuildTarget == buildTarget && buildConfigGroup.ProjectBuild == build).OfType<ScriptableObject>().Select(x => AssetDatabase.GetAssetPath(x)).ToArray();

            int count = duplicatedProjectBuildConfigGroups.Length;

            if (count == 0)
            {
                return new ConfigValidationResults<string[]>(MessageType.None, string.Empty, default);
            }

            string duplicatedConfigPaths = string.Join("\n", duplicatedProjectBuildConfigGroups);

            string validationErrorMessage = $"Multiple copies detected! \n\nThere are {count} copies of this instance found in the project, and only '1' instance is allowed per project. " +
               $"This config, along with '{count - 1}' additional copy/copies, will not be applied, and the tools option for '{build}' will be disabled." +
               $" Please remove this or any of the below listed files. \n\n Duplicated file(s): \n\n{duplicatedConfigPaths}\n";

            return new ConfigValidationResults<string[]>(MessageType.Error, validationErrorMessage, duplicatedProjectBuildConfigGroups);
        }
    }
}
