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
using System.Threading.Tasks;
#endregion

#region Included Parrot Code Assemblies
using ParrotCode.Native.SharedEditor;
#endregion

namespace ParrotCode.Platforms
{
    /// <summary>
    /// Represents a command-line executable that launches the Unity Editor to
    /// perform an automated project build.
    /// </summary>
    /// <remarks>
    /// This executable starts the Unity Editor using the current editor
    /// installation and invokes the build entry point specified by
    /// <see cref="PlatformBuilder.BuildPlayer"/>.
    /// </remarks>
    public sealed class ProjectBuildCommandLineExecutable : CommandLineExecutable
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
        public override int FailureExitCode => 1;

        /// <summary>
        /// Executes the Unity build process asynchronously.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result
        /// contains the exit code returned by the Unity Editor process.
        /// </returns>
        /// <remarks>
        /// The Unity Editor is launched using the command-line arguments defined
        /// by <see cref="SharedCommandLineUtilities.UnityBuildArguments"/>. The build is
        /// started by invoking the method specified by Unity's
        /// <c>-executeMethod</c> command-line argument.
        /// </remarks>
        public override async Task<int> ExecuteAsync()
        {
            string arguments = $" {string.Join(" ", SharedCommandLineUtilities.UnityBuildArguments)}" +
                $" {typeof(ProjectBuilder).FullName}.{nameof(ProjectBuilder.BuildPlayer)} " +
                $"{SharedCommonFiltersAndPatterns.UnityLogFile} " +
                $"{SharedProjectDirectory.TemporaryBuildLogFilePath}";

            ProcessStartInfo processInfo = new ProcessStartInfo
            {
                FileName = SharedNativeProjectDirectory.UnityEditorApplicationPath,
                Arguments = arguments,
                UseShellExecute = true
            };

            return await CommandLineUtility.RunAsync(processInfo,
                FailureExitCode);
        }
    }
}
