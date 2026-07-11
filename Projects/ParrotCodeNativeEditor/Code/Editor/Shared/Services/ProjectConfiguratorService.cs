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
using System;
#endregion

namespace ParrotCode.Native.SharedEditor
{
    /// <summary>
    /// Provides services for retrieving and validating project configurator assets.
    /// </summary>
    /// <remarks>
    /// This service coordinates repository queries and applies business rules for
    /// resolving project configurators and detecting duplicate configuration assets.
    /// </remarks>
    public sealed class ProjectConfiguratorService
    {
        private readonly IProjectConfiguratorRepository _projectConfigRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectConfiguratorService"/> class.
        /// </summary>
        /// <param name="projectConfigRepository">
        /// The repository used to retrieve project configurator assets.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="projectConfigRepository"/> is <see langword="null"/>.
        /// </exception>
        public ProjectConfiguratorService(IProjectConfiguratorRepository projectConfigRepository)
        {
            _projectConfigRepository = projectConfigRepository ??
                throw new ArgumentNullException(nameof(projectConfigRepository));
        }

        /// <summary>
        /// Retrieves the project configurator for the specified Unity build target
        /// and build configuration.
        /// </summary>
        /// <typeparam name="T">
        /// The type of project configurator to retrieve.
        /// </typeparam>
        /// <param name="buildTarget">
        /// The Unity build target.
        /// </param>
        /// <param name="build">
        /// The project build configuration.
        /// </param>
        /// <returns>
        /// A <see cref="ConfigValidationResults{T}"/> containing the matching
        /// project configurator when exactly one asset is found; otherwise,
        /// a validation result describing the lookup failure.
        /// </returns>
        public ConfigValidationResults<T> GetProjectConfigForBuild<T>(
            BuildTarget buildTarget,
            Build build)
            where T : IProjectConfigurator
        {
            var projectConfigs = _projectConfigRepository
                .GetByBuild<T>(buildTarget, build)
                .ToArray();

            int configCount = projectConfigs.Length;

            if (configCount == 1)
            {
                return new ConfigValidationResults<T>(
                    MessageType.None,
                    string.Empty,
                    projectConfigs[0]);
            }

            string warningMessage =
                $"Get build project config for build: {build} targeting: {buildTarget}" +
                $" failed. There are {configCount} build project config(s) found for target.";

            return new ConfigValidationResults<T>(
                MessageType.Warning,
                warningMessage,
                default);
        }

        /// <summary>
        /// Finds duplicate project configurator assets that target the same
        /// Unity build target and build configuration.
        /// </summary>
        /// <typeparam name="T">
        /// The type of project configurator to search for.
        /// </typeparam>
        /// <param name="projectBuildConfig">
        /// The project configurator used as the reference when searching
        /// for duplicate assets.
        /// </param>
        /// <returns>
        /// A <see cref="ConfigValidationResults{T}"/> containing the asset paths
        /// of duplicate project configurators when duplicates are found;
        /// otherwise, a successful validation result.
        /// </returns>
        public ConfigValidationResults<string[]> FindDuplicateProjectConfiguratorPaths<T>(
            T projectBuildConfig)
            where T : ScriptableObject, IProjectConfigurator
        {
            string[] duplicateProjectBuildConfigPaths = _projectConfigRepository
                .GetAll<T>()
                .Where(buildConfigGroup =>
                    buildConfigGroup.BuildTarget == projectBuildConfig.BuildTarget &&
                    buildConfigGroup.ProjectBuild == projectBuildConfig.ProjectBuild)
                .Where(buildConfig => buildConfig != projectBuildConfig)
                .Select(AssetDatabase.GetAssetPath)
                .ToArray();

            int count = duplicateProjectBuildConfigPaths.Length;

            if (count == 0)
            {
                return new ConfigValidationResults<string[]>(
                    MessageType.None,
                    string.Empty,
                    default);
            }

            string duplicatedConfigPaths =
                string.Join("\n", duplicateProjectBuildConfigPaths);

            int duplicatesCount = count + 1;

            string validationErrorMessage =
                $"Multiple copies detected!\n\n" +
                $"There are {duplicatesCount} project configuration assets for " +
                $"{projectBuildConfig.BuildTarget} ({projectBuildConfig.ProjectBuild}) " +
                $"found in the project, and only one instance is allowed for each Build Target / Build " +
                $"combination. This configuration and {count} additional duplicate(s) " +
                $"will not be applied, and the '{projectBuildConfig.ProjectBuild}' " +
                $"menu option will be disabled.\n\n" +
                $"Found duplicate(s):\n\n{duplicatedConfigPaths}\n";

            return new ConfigValidationResults<string[]>(
                MessageType.Error,
                validationErrorMessage,
                duplicateProjectBuildConfigPaths);
        }
    }
}
