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

using UnityEngine;
using UnityEditor;
using UnityEditor.Build;

namespace ParrotCode.Extensions
{
    public static class SharedPlatformExtensions
    {

        public static NamedBuildTarget ToNamedBuildTarget(this BuildTarget buildTarget)
        {
            BuildTargetGroup targetGroup = ToBuildTargetGroup(buildTarget);
            return NamedBuildTarget.FromBuildTargetGroup(targetGroup);
        }

        public static BuildTargetGroup ToBuildTargetGroup(this BuildTarget buildTarget)
        {
            switch (buildTarget)
            {
                case BuildTarget.Android:
                    return BuildTargetGroup.Android;
                case BuildTarget.iOS:
                    return BuildTargetGroup.iOS;
                case BuildTarget.StandaloneLinux64:
                case BuildTarget.StandaloneOSX:
                case BuildTarget.StandaloneWindows64:
                case BuildTarget.StandaloneWindows:
                    return BuildTargetGroup.Standalone;
                case BuildTarget.WebGL:
                    return BuildTargetGroup.WebGL;
                case BuildTarget.XboxOne:
                    return BuildTargetGroup.XboxOne;
                case BuildTarget.Switch:
                    return BuildTargetGroup.Switch;
                case BuildTarget.PS4:
                    return BuildTargetGroup.PS4;
                case BuildTarget.PS5:
                    return BuildTargetGroup.PS5;
                case BuildTarget.QNX:
                    return BuildTargetGroup.QNX;
                default:
                    Debug.LogError($"GetBuildTargetGroup for build target: {buildTarget} failed. Build target is currently not supported in this version of Parrot Code.");
                    return BuildTargetGroup.Unknown;
            }
        }
    }
}
