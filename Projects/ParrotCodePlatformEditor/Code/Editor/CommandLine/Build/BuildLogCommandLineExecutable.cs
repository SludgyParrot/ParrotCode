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
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
#endregion

#region Included Parrot Code Assemblies
using ParrotCode.Native.SharedEditor;
#endregion

namespace ParrotCode.Platforms
{
    /// <summary>
    /// Represents a command-line executable that moves the temporary build log
    /// to the configured build output directory.
    /// </summary>
    /// <remarks>
    /// This command uses RoboCopy to move the generated build log from the
    /// temporary build directory to the final build log directory. The destination
    /// directory is created automatically if it does not already exist.
    /// </remarks>
    public sealed class BuildLogCommandLineExecutable : CommandLineExecutable
    {
        /// <summary>
        /// Gets the minimum process exit code that is considered a failure when
        /// executing the build log command.
        /// </summary>
        public override int FailureExitCode => 8;

        /// <summary>
        /// Moves the temporary build log to the configured build log directory
        /// asynchronously.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous move operation. The task result
        /// contains the exit code returned by the RoboCopy process.
        /// </returns>
        /// <remarks>
        /// The destination build log directory is created if it does not already
        /// exist. The build log is moved using RoboCopy with the
        /// <see cref="CommandLineSwitch.Mov"/> switch to remove the source file
        /// after a successful copy.
        /// </remarks>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the RoboCopy process could not be started.
        /// </exception>
        public override async Task<int> ExecuteAsync()
        {
            Directory.CreateDirectory(SharedProjectDirectory.BuildLogFileDirectory);

            string arguments =
                $"\"{SharedProjectDirectory.TemporaryBuildLogFilePath}\" " +
                $"\"{SharedProjectDirectory.BuildLogFileDirectory}\" " +
                $"{string.Join(" ", SharedCommandLineUtilities.RoboCopyArguments)} " +
                $"{CommandLineSwitch.Mov}";

            ProcessStartInfo moveFileProcessInfo = new ProcessStartInfo
            {
                FileName = SharedCommandLineArguments.RoboCopyApplication,
                Arguments = arguments,
                UseShellExecute = false,
            };

            return await CommandLineUtility.RunAsync(
                moveFileProcessInfo,
                FailureExitCode);
        }
    }
}
