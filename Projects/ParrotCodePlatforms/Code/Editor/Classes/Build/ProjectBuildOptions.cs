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
#endregion

#region Included Unity Assemblies
using UnityEditor;
#endregion

#region Included Jet Brains Assemblies
using JetBrains.Annotations;
#endregion

namespace ParrotCode.Platforms
{
    [Serializable]
    public sealed class ProjectBuildOptions
    {
        public BuildTarget Target { get; private set; }
        public BuildTargetGroup TargetGroup { get; private set; }
        public string LocationPathName { get; private set; }
        public string[] Scenes { get; private set; }
        public string AssetBundleManifestPath { get; private set; }
        public string[] ExtraScriptingDefines { get; private set; }
        public BuildOptions Options { get; private set; }
        public int SubTarget { get; private set; }

        public ProjectBuildOptions(BuildPlayerOptions options): this(
            options.locationPathName, 
            options.scenes,
             options.target,
            options.targetGroup,
            options.assetBundleManifestPath, 
            options.extraScriptingDefines, 
            options.options, 
            options.subtarget) { }

        public ProjectBuildOptions(string locationPathName, 
            string[] scenes,
            BuildTarget target,
            BuildTargetGroup targetGroup,
            [CanBeNull] string assetBundleManifestPath = null,
            [CanBeNull] string[] extraScriptingDefines = null,
            BuildOptions options = BuildOptions.None,
        int subTarget = default)
        {
            Target = target;
            TargetGroup = targetGroup;
            LocationPathName = locationPathName;
            Scenes = scenes;
            AssetBundleManifestPath = assetBundleManifestPath;
            ExtraScriptingDefines = extraScriptingDefines;
            Options = options;
            SubTarget = subTarget;
        }
    }
}
