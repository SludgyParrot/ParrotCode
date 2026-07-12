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
#endregion

#region Included Parrot Code Assemblies
using ParrotCode.Helpers;
using System;
#endregion

namespace ParrotCode.Platforms
{
    /// <summary>
    /// Provides functionality for creating and persisting Unity player build
    /// configurations for use during the automated build process.
    /// </summary>
    public static class BuildConfigurationSerializer
    {
        /// <summary>
        /// Creates and serializes the specified Unity player build configuration
        /// to the temporary build configuration file.
        /// </summary>
        /// <param name="options">
        /// The <see cref="BuildPlayerOptions"/> that define how the Unity player
        /// should be built.
        /// </param>
        /// <remarks>
        /// The serialized build configuration is consumed by the automated build
        /// pipeline when invoking the Unity Editor in batch mode.
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="options"/> is <see langword="null"/>.
        /// </exception>
        public static void Serialize(BuildPlayerOptions options)
        {
            Storage.SerializeToJsonFile(
                SharedProjectDirectory.TemporaryBuildConfigPath,
                new ProjectBuildOptions(options));
        }
    }
}
