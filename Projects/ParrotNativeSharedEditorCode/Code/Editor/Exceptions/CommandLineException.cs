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
using System.IO;
#endregion

#region Included Jet Brains Assemblies
using JetBrains.Annotations;
#endregion

namespace ParrotCode.Native.SharedEditor
{
    /// <summary>
    /// Represents an exception that is thrown when a command-line operation
    /// fails to execute successfully.
    /// </summary>
    /// <remarks>
    /// This exception is intended for failures encountered while invoking
    /// external processes, such as command-line utilities or build tools.
    /// In addition to the exception message, it exposes the process exit code
    /// returned by the failed operation.
    /// </remarks>
    public sealed class CommandLineException : IOException
    {
        /// <summary>
        /// Gets the exit code returned by the failed command-line process.
        /// </summary>
        public int ExitCode { get; }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="CommandLineException"/> class.
        /// </summary>
        /// <param name="fileName">
        /// The name or path of the executable that was invoked.
        /// </param>
        /// <param name="command">
        /// The command-line arguments supplied to the executable.
        /// </param>
        /// <param name="exitCode">
        /// The exit code returned by the failed process.
        /// </param>
        /// <param name="innerException">
        /// The exception that caused the current exception, or
        /// <see langword="null"/> if no inner exception is specified.
        /// </param>
        public CommandLineException(
            string fileName,
            string command,
            int exitCode,
            [CanBeNull] Exception innerException = null)
            : base(
                $"{fileName} {command} execution failed with exit code: {exitCode}",
                innerException)
        {
            ExitCode = exitCode;
        }
    }
}
