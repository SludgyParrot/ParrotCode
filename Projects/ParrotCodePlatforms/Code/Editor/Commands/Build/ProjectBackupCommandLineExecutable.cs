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
#endregion

#region Included Parrot Code Assemblies
using ParrotCode.Native.SharedEditor;
using System.Threading.Tasks;
#endregion

namespace ParrotCode.Platforms
{
    public sealed class ProjectBackupCommandLineExecutable : ICommandLineExecutable
    {
        public async Task<int> Execute()
        {
            string projectRootDirectory = SharedProjectDirectory.GetRootProjectPath();
            string temporaryProjectBackupDirectory = SharedProjectDirectory.GetTemporaryBuildProjectPath();

            string projectCopyArguments = $"/K \"{projectRootDirectory}\" \"{temporaryProjectBackupDirectory}\" " +
                $"{string.Join(" ", SharedBatchCommands.RoboCopyArguments)}";

            ProcessStartInfo projectCopyProcessStartInfo = new ProcessStartInfo
            {
                FileName = SharedBatchCommands.RoboCopyApplication,
                Arguments = projectCopyArguments,
                UseShellExecute = true,
            };

            return await CommandLineUtility.RunAsync(projectCopyProcessStartInfo);
        }
    }
}
