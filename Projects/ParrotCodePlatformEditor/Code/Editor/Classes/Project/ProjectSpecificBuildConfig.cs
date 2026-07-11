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

#region Included Parrot Code Assemblies
using ParrotCode.Native.SharedEditor;
#endregion

#region Included Unity Assemblies
using UnityEditor;
#endregion

namespace ParrotCode.Platforms
{
    /// <summary>
    /// Represents a base implementation for project-specific build configuration assets
    /// that define a Unity build target and validation logic.
    /// </summary>
    /// <remarks>
    /// This class extends <see cref="ProjectBuildConfig"/> and implements
    /// <see cref="IProjectBuildConfig"/> to provide a unified base for
    /// platform-specific build configuration definitions.
    /// </remarks>
    public abstract class ProjectSpecificBuildConfig : ProjectBuildConfig, IProjectBuildConfig
    {
        /// <summary>
        /// Gets the Unity build target associated with this configuration.
        /// </summary>
        public abstract BuildTarget BuildTarget { get; }
    }
}
