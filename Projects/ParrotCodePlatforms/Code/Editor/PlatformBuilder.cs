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
using System.ComponentModel;
#endregion

#region Included Unity Assemblies
using UnityEditor;
#endregion

#region Included Parrot Code Assemblies
using ParrotCode.Native.SharedEditor;
using System.IO;
#endregion

namespace ParrotCode.Platforms
{
    public static class PlatformBuilder
    {
        private static readonly ProjectBackupCommandLineExecutable _projectBackupCommand =
            new ProjectBackupCommandLineExecutable();

        private static readonly ProjectBuildCommandLineExecutable _projectBuildCommand = 
            new ProjectBuildCommandLineExecutable();

        public static async Task<int> InitializeBuild()
        {
            int projectBackupExitCode = await _projectBackupCommand.Execute();

            if (projectBackupExitCode >= 8)
            {
                throw new CommandLineException(SharedBatchCommands.RoboCopyApplication, 
                    "Project backup", projectBackupExitCode);
            }

            int projectBuildExitCode = await _projectBuildCommand.Execute();

            if (projectBuildExitCode >= 0)
            {
                throw new CommandLineException(SharedBatchCommands.CMDApplication, 
                    "Project build", projectBuildExitCode);
            }

            return projectBuildExitCode;
        }

        public static void BuilPlayer()
        {
            BuildPlayerOptions buildOptions = new BuildPlayerOptions();

            BuildPipeline.BuildPlayer(buildOptions);
        }
    }
}
