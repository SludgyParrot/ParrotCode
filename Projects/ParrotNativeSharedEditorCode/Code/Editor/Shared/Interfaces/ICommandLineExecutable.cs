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
using System.Threading.Tasks;
#endregion

namespace ParrotCode.Native.SharedEditor
{
    /// <summary>
    /// Defines a contract for asynchronously executing a command-line process.
    /// </summary>
    /// <remarks>
    /// Implementations encapsulate the information required to launch an external
    /// executable and return its exit code upon completion. An exit code of
    /// <c>0</c> typically indicates success, although the meaning of exit codes
    /// is determined by the executed application.
    /// </remarks>
    public interface ICommandLineExecutable
    {
        /// <summary>
        /// Executes the configured command-line process asynchronously.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result
        /// contains the exit code returned by the executed process.
        /// </returns>
        Task<int> Execute();
    }
}
