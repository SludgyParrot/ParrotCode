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
#endregion

namespace ParrotCode.Platforms
{
    /// <summary>
    /// Represents a command-line executable that opens a command prompt and
    /// displays the contents of the project build log.
    /// </summary>
    /// <remarks>
    /// The build log is displayed using the Windows <c>cmd.exe</c> utility.
    /// If the directory containing the log file does not exist, it is created
    /// before the command is executed.
    /// </remarks>
    public sealed class ProjectBuildCommandLogExecutable : ICommandLineExecutable
    {
        /// <summary>
        /// Opens a command prompt and displays the project build log.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result is
        /// always <c>0</c>, indicating that the logger command was started
        /// successfully.
        /// </returns>
        public async Task<int> Execute()
        {
            string temporaryBuildLogFilePath = SharedProjectDirectory.TemporaryBuildLogFilePath;

            Directory.CreateDirectory(Path.GetDirectoryName(temporaryBuildLogFilePath));

            string arguments =
                $"{string.Join(" ", SharedCommandLineUtilities.ProjectBuildLogArguments)} " +
                $"\"{temporaryBuildLogFilePath}\"";

            UnityEngine.Debug.Log($"~Command arguments: {arguments}");

            ProcessStartInfo consoleProcessInfo = new ProcessStartInfo
            {
                FileName = SharedCommandLineUtilities.CMDApplication,
                Arguments = arguments,
                UseShellExecute = true
            };

            CommandLineUtility.Run(consoleProcessInfo);

           return await Task.FromResult(0);
        }
    }
}
