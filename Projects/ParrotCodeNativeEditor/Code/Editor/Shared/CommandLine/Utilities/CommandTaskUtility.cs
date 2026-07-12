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
using System.Threading.Tasks;
#endregion

#region Included Parrot Code Assemblies
using ParrotCode.Native.SharedEditor;
using ParrotCode.Helpers;
#endregion

namespace ParrotCode.Native.SharedEditor
{
    /// <summary>
    /// Provides helper methods for executing command-line executables and validating
    /// their exit codes.
    /// </summary>
    public static class CommandTaskUtility
    {
        /// <summary>
        /// Executes the specified command-line executable asynchronously and throws
        /// a <see cref="CommandLineException"/> if the process exits with a failure
        /// exit code.
        /// </summary>
        /// <param name="executable">
        /// The command-line executable to execute.
        /// </param>
        /// <param name="operationName">
        /// A descriptive name of the operation being performed. This value is included
        /// in any thrown <see cref="CommandLineException"/>.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous command execution.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="executable"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="operationName"/> is <see langword="null"/>,
        /// empty, or consists only of white-space characters.
        /// </exception>
        /// <exception cref="CommandLineException">
        /// Thrown when the executable exits with a failure exit code.
        /// </exception>
        public static async Task ExecuteCommandOrThrowAsync(
            CommandLineExecutable executable,
            string operationName)
        {
            if (executable == null)
            {
                throw new ArgumentNullException(nameof(executable),
                    "Executable cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(operationName))
            {
                throw new ArgumentException("Operation name cannot be null, " +
                    "or consist entirely of white space.", nameof(operationName));
            }

            int exitCode = await ParrotCodeEditorTask.Run(executable.ExecuteAsync);

            if (exitCode >= executable.FailureExitCode)
            {
                throw new CommandLineException(
                    SharedCommandLineUtilities.UnityEditorApplication,
                    operationName,
                    exitCode);
            }
        }
    }
}
