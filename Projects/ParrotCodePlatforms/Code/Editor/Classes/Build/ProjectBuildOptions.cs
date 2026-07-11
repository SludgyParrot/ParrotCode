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
    /// <summary>
    /// Represents a serializable snapshot of Unity's <see cref="BuildPlayerOptions"/>.
    /// </summary>
    /// <remarks>
    /// This class is intended for persisting build configurations to disk (for
    /// example, as JSON) and reconstructing a <see cref="BuildPlayerOptions"/>
    /// instance when required.
    ///
    /// Unlike <see cref="BuildPlayerOptions"/>, this class is marked as
    /// <see cref="SerializableAttribute"/> and contains only serializable fields,
    /// making it suitable for Unity's JSON serialization APIs.
    /// </remarks>
    [Serializable]
    public sealed class ProjectBuildOptions
    {
        /// <summary>
        /// The target platform to build.
        /// </summary>
        public BuildTarget Target;

        /// <summary>
        /// The build target group associated with <see cref="Target"/>.
        /// </summary>
        public BuildTargetGroup TargetGroup;

        /// <summary>
        /// The output file or directory where the build will be generated.
        /// </summary>
        public string LocationPathName;

        /// <summary>
        /// The collection of scenes included in the build.
        /// </summary>
        public string[] Scenes;

        /// <summary>
        /// The optional path to an AssetBundle manifest used during the build.
        /// </summary>
        [CanBeNull]
        public string AssetBundleManifestPath;

        /// <summary>
        /// Additional scripting define symbols applied only to this build.
        /// </summary>
        [CanBeNull]
        public string[] ExtraScriptingDefines;

        /// <summary>
        /// The build options applied during the build process.
        /// </summary>
        public BuildOptions Options;

        /// <summary>
        /// The platform-specific build sub-target.
        /// </summary>
        public int SubTarget;

        /// <summary>
        /// Explicitly converts a <see cref="BuildPlayerOptions"/> instance into a
        /// serializable <see cref="ProjectBuildOptions"/>.
        /// </summary>
        /// <param name="options">
        /// The Unity build options to convert.
        /// </param>
        /// <returns>
        /// A new <see cref="ProjectBuildOptions"/> containing the values from the
        /// specified <paramref name="options"/>.
        /// </returns>
        /// <remarks>
        /// This conversion creates a serializable representation of
        /// <see cref="BuildPlayerOptions"/>, making it suitable for persistence,
        /// such as writing to a JSON file.
        /// </remarks>
        public static explicit operator ProjectBuildOptions(BuildPlayerOptions options)
            => new ProjectBuildOptions(options);

        /// <summary>
        /// Explicitly converts a <see cref="ProjectBuildOptions"/> instance into a
        /// Unity <see cref="BuildPlayerOptions"/> structure.
        /// </summary>
        /// <param name="options">
        /// The serializable build options to convert.
        /// </param>
        /// <returns>
        /// A new <see cref="BuildPlayerOptions"/> initialized with the values from
        /// the specified <paramref name="options"/>.
        /// </returns>
        /// <remarks>
        /// This conversion reconstructs a <see cref="BuildPlayerOptions"/> instance
        /// that can be passed directly to Unity's build pipeline.
        /// </remarks>
        public static explicit operator BuildPlayerOptions(ProjectBuildOptions options)
            => options.ToBuildPlayerOptions();

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectBuildOptions"/> class.
        /// </summary>
        /// <remarks>
        /// This parameterless constructor is provided to support serialization and
        /// deserialization scenarios, such as loading build configurations from JSON.
        /// Fields are initialized to their default values and can be populated
        /// during the deserialization process.
        /// </remarks>
        public ProjectBuildOptions() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectBuildOptions"/>
        /// class from an existing <see cref="BuildPlayerOptions"/> instance.
        /// </summary>
        /// <param name="options">
        /// The Unity build options to copy.
        /// </param>
        public ProjectBuildOptions(BuildPlayerOptions options)
            : this(
                options.locationPathName,
                options.scenes,
                options.target,
                options.targetGroup,
                options.assetBundleManifestPath,
                options.extraScriptingDefines,
                options.options,
                options.subtarget)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectBuildOptions"/>
        /// class using the specified build configuration values.
        /// </summary>
        /// <param name="locationPathName">
        /// The output file or directory for the build.
        /// </param>
        /// <param name="scenes">
        /// The collection of scene paths included in the build.
        /// </param>
        /// <param name="target">
        /// The target platform to build.
        /// </param>
        /// <param name="targetGroup">
        /// The build target group associated with the target platform.
        /// </param>
        /// <param name="assetBundleManifestPath">
        /// The optional AssetBundle manifest path.
        /// </param>
        /// <param name="extraScriptingDefines">
        /// Optional scripting define symbols to apply during the build.
        /// </param>
        /// <param name="options">
        /// The build options to apply.
        /// </param>
        /// <param name="subTarget">
        /// The platform-specific build sub-target.
        /// </param>
        public ProjectBuildOptions(
            string locationPathName,
            string[] scenes,
            BuildTarget target,
            BuildTargetGroup targetGroup,
            [CanBeNull] string assetBundleManifestPath = null,
            [CanBeNull] string[] extraScriptingDefines = null,
            BuildOptions options = BuildOptions.None,
            int subTarget = default)
        {
            LocationPathName = locationPathName;
            Scenes = scenes;
            Target = target;
            TargetGroup = targetGroup;
            AssetBundleManifestPath = assetBundleManifestPath;
            ExtraScriptingDefines = extraScriptingDefines;
            Options = options;
            SubTarget = subTarget;
        }

        /// <summary>
        /// Converts this instance into a Unity
        /// <see cref="BuildPlayerOptions"/> structure.
        /// </summary>
        /// <returns>
        /// A new <see cref="BuildPlayerOptions"/> populated with the values
        /// stored in this instance.
        /// </returns>
        public BuildPlayerOptions ToBuildPlayerOptions()
        {
            return new BuildPlayerOptions
            {
                locationPathName = LocationPathName,
                scenes = Scenes,
                target = Target,
                targetGroup = TargetGroup,
                assetBundleManifestPath = AssetBundleManifestPath,
                extraScriptingDefines = ExtraScriptingDefines,
                options = Options,
                subtarget = SubTarget
            };
        }
    }
}
