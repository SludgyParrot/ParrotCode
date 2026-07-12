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
using System.Threading.Tasks;
#endregion

#region Included Parrot Code Assemblies
using ParrotCode.Native.SharedEditor;
#endregion

namespace ParrotCode.Platforms
{
    /// <summary>
    /// Coordinates the automated Unity project build workflow by creating a temporary
    /// project backup, executing the project build, cleaning temporary resources,
    /// and opening the build output directory.
    /// </summary>
    public sealed class ProjectBuildCommandProcessor
    {
        /// <summary>
        /// Creates a temporary backup of the Unity project before the build process begins.
        /// </summary>
        private readonly ProjectBackupCommandLineExecutable _projectBackupCommand =
            new ProjectBackupCommandLineExecutable();

        /// <summary>
        /// Executes the Unity project build.
        /// </summary>
        private readonly ProjectBuildCommandLineExecutable _projectBuildCommand =
            new ProjectBuildCommandLineExecutable();

        /// <summary>
        /// Logs the command used to execute the Unity project build.
        /// </summary>
        private readonly ProjectBuildCommandLogExecutable _projectBuildLogCommand =
            new ProjectBuildCommandLogExecutable();

        /// <summary>
        /// Executes the complete project build workflow.
        /// </summary>
        /// <remarks>
        /// The build workflow performs the following operations:
        /// <list type="number">
        /// <item>
        /// <description>Logs the build command.</description>
        /// </item>
        /// <item>
        /// <description>Creates a temporary backup of the project.</description>
        /// </item>
        /// <item>
        /// <description>Builds the Unity project.</description>
        /// </item>
        /// <item>
        /// <description>Removes the temporary backup project.</description>
        /// </item>
        /// <item>
        /// <description>Opens the build output directory.</description>
        /// </item>
        /// </list>
        /// <para>
        /// The temporary backup project is removed regardless of whether the build
        /// succeeds or fails.
        /// </para>
        /// </remarks>
        /// <returns>
        /// A task that represents the asynchronous build workflow.
        /// </returns>
        /// <exception cref="CommandLineException">
        /// Thrown when any command-line executable returns a failure exit code.
        /// </exception>
        public async Task InitializeBuild()
        {
            _projectBuildLogCommand.Execute();

            try
            {
                await CommandTaskUtility.ExecuteCommandOrThrowAsync(
                    _projectBackupCommand,
                    "Project backup");

                await CommandTaskUtility.ExecuteCommandOrThrowAsync(
                    _projectBuildCommand,
                    "Project build");
            }
            finally
            {
                RemoveFolderCommandLineExecutable _clearTempProjectBackupCommand = 
                    new RemoveFolderCommandLineExecutable(
                    SharedNativeProjectDirectory.TemporaryBuildProjectPath);

                await CommandTaskUtility.ExecuteCommandOrThrowAsync(
                    _clearTempProjectBackupCommand,
                    "Clear backup");
            }

            OpenFolderCommandLineExecutable openFolderCommand =
                new OpenFolderCommandLineExecutable(
                    SharedProjectDirectory.BuildFolderDirectory);

            openFolderCommand.Execute();
        }
    }
}
