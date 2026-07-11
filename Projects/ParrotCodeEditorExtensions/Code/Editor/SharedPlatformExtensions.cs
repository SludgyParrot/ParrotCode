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

#region Included Unity Assemblies
using UnityEditor;
using UnityEditor.Build;
#endregion

#region Included Parrot Code Assemblies
using ParrotCode.Native;
#endregion

namespace ParrotCode.EditorExtensions
{
    /// <summary>
    /// This class contains extensions for platform build specific settings.
    /// </summary>
    public static class SharedPlatformExtensions
    {
        /// <summary>
        /// This function converts a <see cref="BuildTarget"/> into a <see cref="NamedBuildTarget"/>.
        /// </summary>
        /// <returns>A <see cref="NamedBuildTarget"/> for this <see cref="BuildTarget"/></returns>
        public static NamedBuildTarget ToNamedBuildTarget(this BuildTarget buildTarget)
        {
            BuildTargetGroup targetGroup = ToBuildTargetGroup(buildTarget);
            return NamedBuildTarget.FromBuildTargetGroup(targetGroup);
        }

        /// <summary>
        /// This function converts a <see cref="BuildTarget"/> into a <see cref="BuildTargetGroup"/>.
        /// </summary>
        /// <returns>A <see cref="BuildTargetGroup"/> matching this <see cref="BuildTarget"/> </returns>
        public static BuildTargetGroup ToBuildTargetGroup(this BuildTarget buildTarget)
        {
            return buildTarget switch
            {
                BuildTarget.Android => BuildTargetGroup.Android,
                BuildTarget.iOS => BuildTargetGroup.iOS,
                BuildTarget.StandaloneLinux64 |
                BuildTarget.StandaloneOSX |
                BuildTarget.StandaloneWindows |
                BuildTarget.StandaloneWindows64 => BuildTargetGroup.Standalone,
                BuildTarget.WebGL => BuildTargetGroup.WebGL,
                BuildTarget.PS5 => BuildTargetGroup.PS5,
                BuildTarget.XboxOne => BuildTargetGroup.XboxOne,
                BuildTarget.Switch => BuildTargetGroup.Switch,
                BuildTarget.QNX => BuildTargetGroup.QNX,
                _ => BuildTargetGroup.Unknown
            };
        }

        /// <summary>
        /// Returns the file extension associated with the specified build target.
        /// </summary>
        /// <returns>
        /// The file extension (including the leading '.') if the build target
        /// produces a single output file; otherwise, <see cref="string.Empty"/>.
        /// </returns>
        public static string ToBuildExtension(this BuildTarget buildTarget)
        {
            switch(buildTarget)
            {
                case BuildTarget.Android:
                    return "apk";
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                    return "exe";
                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// Used to check <see cref="BuildTarget"/>'s output type.
        /// </summary>
        /// <returns>File if <see cref="BuildTarget"/> has an associated file extension, else Folder.</returns>
        public static BuildOutput GetBuildOutput(this BuildTarget buildTarget)
            => string.IsNullOrEmpty(buildTarget.ToBuildExtension()) ? BuildOutput.Folder : BuildOutput.File;
    }
}
