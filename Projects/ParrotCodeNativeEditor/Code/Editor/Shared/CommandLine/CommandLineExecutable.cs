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
using System;
#endregion

namespace ParrotCode.Native.SharedEditor
{
    /// <summary>
    /// Provides a base implementation of the <see cref="ICommandLineExecutable"/>
    /// interface for command-line executables.
    /// </summary>
    /// <remarks>
    /// Derive from this class to implement command-line operations that can be
    /// executed synchronously or asynchronously. Override the members to provide
    /// the command-specific failure exit code and execution logic.
    /// </remarks>
    public abstract class CommandLineExecutable :
        ICommandLineExecutable
    {
        /// <summary>
        /// Gets the minimum process exit code that is considered a failure.
        /// </summary>
        /// <value>
        /// The exit code threshold used to determine whether the command execution
        /// failed. Exit codes greater than or equal to this value are considered
        /// failures.
        /// </value>
        public virtual int FailureExitCode { get; }

        /// <summary>
        /// Executes the command-line operation synchronously.
        /// </summary>
        /// <exception cref="NotImplementedException">
        /// Thrown when the derived class does not override this method.
        /// </exception>
        public virtual void Execute()
            => throw new NotImplementedException();

        /// <summary>
        /// Executes the command-line operation asynchronously.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result
        /// contains the exit code returned by the completed process.
        /// </returns>
        /// <exception cref="NotImplementedException">
        /// Thrown when the derived class does not override this method.
        /// </exception>
        public virtual Task<int> ExecuteAsync()
            => throw new NotImplementedException();
    }
}
