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
using System.Collections.Generic;
using System.Linq;
#endregion

#region Included Unity Assemblies
using UnityEngine;
using UnityEditor;
#endregion

namespace ParrotCode.Native.SharedEditor
{
    /// <summary>
    /// A repository for <see cref="IProjectConfigurator"/>
    /// </summary>
    public sealed class ProjectConfiguratorRepository : IProjectConfiguratorRepository
    {
        /// <summary>
        /// Gets all <see cref="IProjectConfigurator"/> in this repository.
        /// </summary>
        /// <typeparam name="T">The asset type to retrieve.</typeparam>
        /// <returns><see langword="IEnumerable"/> of <see cref="IProjectConfigurator"/></returns>
        public IEnumerable<T> GetAll<T>() where T : IProjectConfigurator
        {
            string[] projectConfigGuids = AssetDatabase.FindAssets(SharedCustomEditorStringInfo.GetAssetDatabaseTypeFilter<T>());

            T[] projectConfigs = projectConfigGuids.Select(guid =>
            AssetDatabase.LoadAssetAtPath<ScriptableObject>(AssetDatabase.GUIDToAssetPath(guid))).OfType<T>().ToArray();

            return projectConfigs;
        }

        /// <summary>
        /// Gets a <see cref="IProjectConfigurator"/> for <see cref="BuildTarget"/> and <see cref="Build"/>.
        /// </summary>
        /// <typeparam name="T">The asset type to retrieve.</typeparam>
        /// <param name="buldTarget">The build target for the requested <see cref="IProjectConfigurator"/></param>
        /// <param name="build">The build type for the requested <see cref="IProjectConfigurator"/></param>
        /// <returns>A <see cref="IProjectConfigurator"/> for the defined type.</returns>
        public IEnumerable<T> GetByBuild<T>(BuildTarget buldTarget, Build build) where T : IProjectConfigurator
        {
            //return GetAll<T>()?.Where(projectConfig => projectConfig.BuildTarget == buldTarget && 
            //projectConfig.ProjectBuild == build && projectConfig.Validate().Success()).ToArray();

            return default;
        }
    }
}
