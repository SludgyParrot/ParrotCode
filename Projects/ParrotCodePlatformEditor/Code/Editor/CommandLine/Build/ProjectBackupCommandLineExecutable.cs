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
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
#endregion

#region Included Parrot Code Assemblies
using ParrotCode.Native.SharedEditor;
using UnityEngine.DedicatedServer;
#endregion

namespace ParrotCode.Platforms
{
    /// <summary>
    /// Represents a command-line executable that creates a temporary backup of
    /// the current Unity project using the Windows <c>robocopy</c> utility.
    /// </summary>
    /// <remarks>
    /// The project is copied to a temporary build directory before the build
    /// process begins. This allows automated builds to operate on an isolated
    /// copy of the project without modifying the original working directory.
    /// </remarks>
    public sealed class ProjectBackupCommandLineExecutable : CommandLineExecutable
    {
        /// <summary>
        /// Gets the minimum process exit code that is considered a failure.
        /// </summary>
        /// <value>
        /// The exit code threshold used to determine whether command execution
        /// completed successfully. Any exit code greater than or equal to this
        /// value is treated as a failure.
        /// </value>
        /// <remarks>
        /// This value defaults to <c>8</c>, which corresponds to the Robocopy exit
        /// code indicating that one or more failures occurred during the copy
        /// operation. Exit codes below this value are generally considered
        /// successful or informational by Robocopy.
        /// </remarks>
        public override int FailureExitCode => 8;

        /// <summary>
        /// Executes a Robocopy command that creates a temporary backup of the current Unity project
        /// before the build process begins.
        /// </summary>
        /// <remarks>
        /// The method copies the root project directory to the temporary build project directory
        /// using the predefined Robocopy arguments in <see cref="SharedCommandLineUtilities.RoboCopyArguments"/>.
        /// <para>
        /// A directory for the Robocopy log file is created automatically if it does not already exist.
        /// </para>
        /// <para>
        /// The command is executed asynchronously through <see cref="CommandLineUtility.RunAsync(System.Diagnostics.ProcessStartInfo)"/>
        /// and returns the Robocopy exit code when the copy operation completes.
        /// </para>
        /// </remarks>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the
        /// exit code returned by Robocopy.
        /// </returns>
        /// <exception cref="System.IO.IOException">
        /// Thrown if the Robocopy process fails to complete successfully.
        /// </exception>
        /// <exception cref="System.InvalidOperationException">
        /// Thrown if the Robocopy process could not be started.
        /// </exception>
        public override async Task<int> ExecuteAsync()
        {
            string projectRootDirectory = SharedNativeProjectDirectory.RootProjectPath;
            string temporaryProjectBackupDirectory = SharedNativeProjectDirectory.TemporaryBuildProjectPath;
            string temporaryBuildLogFilePath = SharedProjectDirectory.TemporaryProjectBackupLogFilePath;

            Directory.CreateDirectory(Path.GetDirectoryName(temporaryBuildLogFilePath));

            string arguments =
                $"\"{projectRootDirectory}\" \"{temporaryProjectBackupDirectory}\" " +
                $"{string.Join(" ", SharedCommandLineUtilities.RoboCopyArguments)}\"{temporaryBuildLogFilePath}\"";

            ProcessStartInfo projectCopyProcessStartInfo = new ProcessStartInfo
            {
                FileName = SharedCommandLineUtilities.RoboCopyApplication,
                Arguments = arguments,
                UseShellExecute = false
            };

            return await CommandLineUtility.RunAsync(projectCopyProcessStartInfo, 
                FailureExitCode);
        }
    }
}
