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

#region Included Systems Assemblies
using System;
using System.Threading.Tasks;
#endregion

namespace ParrotCode.Native.SharedEditor
{
    /// <summary>
    /// Defines the contract for executing a command-line process.
    /// </summary>
    /// <remarks>
    /// Implementations encapsulate the configuration and execution of a specific
    /// command-line operation. The interface provides both synchronous and
    /// asynchronous execution methods, along with the exit code threshold that
    /// determines whether the command execution is considered a failure.
    /// </remarks>
    public interface ICommandLineExecutable
    {
        /// <summary>
        /// Gets the minimum process exit code that is considered a failure.
        /// </summary>
        /// <value>
        /// The exit code threshold used to determine whether the command executed
        /// successfully. Any exit code greater than or equal to this value is
        /// considered a failure.
        /// </value>
        int FailureExitCode { get; }

        /// <summary>
        /// Executes the command-line operation synchronously.
        /// </summary>
        /// <remarks>
        /// This method blocks the calling thread until the command-line process
        /// has completed execution.
        /// </remarks>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the command-line process could not be started.
        /// </exception>
        /// <exception cref="System.ComponentModel.Win32Exception">
        /// Thrown if an operating system error occurs while starting the process.
        /// </exception>
        void Execute();

        /// <summary>
        /// Executes the command-line operation asynchronously.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result
        /// contains the exit code returned by the completed process.
        /// </returns>
        /// <remarks>
        /// The returned exit code should be compared against
        /// <see cref="FailureExitCode"/> to determine whether the command
        /// completed successfully.
        /// </remarks>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the command-line process could not be started.
        /// </exception>
        /// <exception cref="System.ComponentModel.Win32Exception">
        /// Thrown if an operating system error occurs while starting the process.
        /// </exception>
        Task<int> ExecuteAsync();
    }
}
