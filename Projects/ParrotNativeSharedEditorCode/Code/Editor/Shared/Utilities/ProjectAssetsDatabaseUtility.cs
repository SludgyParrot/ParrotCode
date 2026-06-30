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
#endregion

#region Included Unity Assemblies
using UnityEditor;
using UnityEngine;
#endregion

namespace ParrotCode.Native.SharedEditor
{
    public static class ProjectAssetsDatabaseUtility
    {
        private static readonly Dictionary<(BuildTarget, Build), object> cachedDatabaseAssets = new Dictionary<(BuildTarget, Build), object>();
        private static readonly Dictionary<(BuildTarget, Build), object> cachedDuplicatedDatabaseAssets = new Dictionary<(BuildTarget, Build), object>();

        public static ConfigValidationResults<T> GetProjectConfiguratorForBuild<T>(BuildTarget buildTarget, Build build) where T : IProjectConfigurator
        {
            if(cachedDatabaseAssets.TryGetValue((buildTarget, build), out var config))
            {
                return (ConfigValidationResults<T>)config;
            }

            Debug.Log($"~GetProjectConfiguratorForBuild for build target: {buildTarget} of project build: {build}");

            ProjectConfiguratorRepository repository = new ProjectConfiguratorRepository();
            ProjectConfiguratorServices configuratorServices = new ProjectConfiguratorServices(repository);

            ConfigValidationResults<T> configValidationResults = configuratorServices.GetProjectConfigForBuild<T>(buildTarget, build);

            if (configValidationResults.MessageType != MessageType.None)
            {
                return configValidationResults;
            }

            cachedDatabaseAssets[(buildTarget, build)] = configValidationResults;

            return configValidationResults;
        }

        public static ConfigValidationResults<string[]> GetProjectConfigForBuildDuplicatePaths<T>(T projectConfig) where T : IProjectConfigurator
        {
            if (cachedDuplicatedDatabaseAssets.TryGetValue((projectConfig.BuildTarget, projectConfig.ProjectBuild), out var config))
            {
                return (ConfigValidationResults<string[]>)config;
            }

            Debug.Log($"~GetProjectConfigForBuildDuplicatePaths for build target: {projectConfig.BuildTarget} of project build: {projectConfig.ProjectBuild}");

            ProjectConfiguratorRepository repository = new ProjectConfiguratorRepository();
            ProjectConfiguratorServices configuratorServices = new ProjectConfiguratorServices(repository);

            ConfigValidationResults<string[]> configValidationResults = configuratorServices.GetProjectConfigForBuildDuplicatePaths<T>(projectConfig.BuildTarget, projectConfig.ProjectBuild);

            if (configValidationResults.MessageType != MessageType.None)
            {
                return configValidationResults;
            }

            cachedDuplicatedDatabaseAssets[(projectConfig.BuildTarget, projectConfig.ProjectBuild)] = configValidationResults;

            return configValidationResults;
        }

        public static void ClearCache()
        {
            cachedDatabaseAssets.Clear();
            cachedDuplicatedDatabaseAssets.Clear();
            Debug.Log("Project asset database cache cleared.");
        }
    }
}
