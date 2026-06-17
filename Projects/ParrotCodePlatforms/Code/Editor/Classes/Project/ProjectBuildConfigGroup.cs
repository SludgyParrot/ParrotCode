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

using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ParrotCode.Platforms
{
    [CreateAssetMenu(fileName = "Project Configuration", menuName = ProjectSharedDirectory.PlatformConfigRootPath + "Project Configuration")]
    public sealed class ProjectBuildConfigGroup: ScriptableObject
    {
        [SerializeField]
        private BuildTarget buildTarget;

        [SerializeField, Space(5)]
        private Build projectBuild;

        [Header("Project Settings")]
        [SerializeField, Space(5)]
        private List<ProjectBuildConfig> buildConfigs;

        public BuildTarget BuildTarget => buildTarget;
        public Build ProjectBuild => projectBuild;

        public IReadOnlyList<ProjectBuildConfig> ProjectBuildConfigs => buildConfigs;
    }
}
