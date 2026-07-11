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
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
#endregion

#region Included Unity Assemblies
using UnityEditor;
using UnityEditorInternal;
#endregion

namespace ParrotCode.Native.SharedEditor
{
    /// <summary>
    /// Provides helper methods for executing command-line applications and external
    /// processes from within the Unity Editor.
    /// </summary>
    /// <remarks>
    /// This utility wraps <see cref="System.Diagnostics.Process"/> to simplify
    /// launching command-line tools, waiting for their completion, and retrieving
    /// their exit codes.
    ///
    /// Typical use cases include invoking utilities such as
    /// <c>robocopy.exe</c>, <c>git.exe</c>, <c>unity.exe</c>, or other
    /// command-line applications used during build automation, project setup,
    /// asset processing, and deployment workflows.
    /// </remarks>
    public static class CommandLineUtility
    {
        /// <summary>
        /// Executes the specified process asynchronously and waits for it to complete.
        /// </summary>
        /// <param name="processInfo">
        /// The <see cref="ProcessStartInfo"/> that defines the executable to start and
        /// the arguments passed to the process.
        /// </param>
        /// <param name="failureExitCode">
        /// The minimum exit code that is considered a failure. If the process exits
        /// with a code greater than or equal to this value, an error is logged to the
        /// Unity Console; otherwise, a standard log message is written.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains
        /// the exit code returned by the completed process.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="processInfo"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown if <see cref="ProcessStartInfo.FileName"/> is <see langword="null"/>,
        /// empty, or consists only of white-space characters.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the process could not be started.
        /// </exception>
        /// <remarks>
        /// The process is executed on a background thread using
        /// <see cref="Task.Run(Action)"/> so that the calling thread is not blocked while
        /// waiting for the process to complete. After the process exits, the exit code
        /// is logged on Unity's main thread using <see cref="EditorApplication.delayCall"/>.
        /// </remarks>
        public static async Task<int> RunAsync(
            ProcessStartInfo processInfo, 
            int failureExitCode)
        {
            if(processInfo == null)
            {
                throw new ArgumentNullException(nameof(processInfo),
                    "Process info cannot be null.");
            }

            if(string.IsNullOrWhiteSpace(processInfo.FileName))
            {
                throw new ArgumentException("A file name is required to run a process.",
                    nameof(processInfo.FileName));
            }

            return await Task.Run(() =>
            {
                using Process process = Process.Start(processInfo) ??
                                  throw new InvalidOperationException($"{processInfo.FileName}" +
                                  $" failed to start.");

                process.WaitForExit();

                int exitCode = process.ExitCode;

                EditorApplication.delayCall += () => 
                {
                    string logMessage = $"{processInfo.FileName} completed, with exit code: {exitCode}\nCommand: {processInfo.Arguments}";

                    if(exitCode >= failureExitCode)
                    {
                        UnityEngine.Debug.LogError(logMessage);
                    }
                    else
                    {
                        UnityEngine.Debug.Log(logMessage);
                    }
                };

                return exitCode;
            });
        }

        public static void Run(ProcessStartInfo processInfo)
        {
            if (processInfo == null)
            {
                throw new ArgumentNullException(nameof(processInfo),
                    "Process info cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(processInfo.FileName))
            {
                throw new ArgumentException("A file name is required to run a process.",
                    nameof(processInfo.FileName));
            }

            using Process process = Process.Start(processInfo) ??
                                 throw new InvalidOperationException($"{processInfo.FileName}" +
                                 $" failed to start.");
        }
    }
}
