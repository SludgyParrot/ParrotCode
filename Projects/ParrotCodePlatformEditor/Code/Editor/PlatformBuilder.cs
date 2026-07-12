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

#region Included Unity Assemblies
using UnityEngine;
using UnityEditor;
using UnityEditor.Build.Reporting;
#endregion

#region Included Parrot Code Assemblies
using ParrotCode.Native.SharedEditor;
using ParrotCode.Helpers;
#endregion

namespace ParrotCode.Platforms
{
    public static class PlatformBuilder
    {
        private static readonly ProjectBackupCommandLineExecutable _projectBackupCommand =
            new ProjectBackupCommandLineExecutable();

        private static readonly ProjectBuildCommandLineExecutable _projectBuildCommand = 
            new ProjectBuildCommandLineExecutable();

        private static readonly ProjectBuildCommandLogExecutable _projectBuildLogCommand = 
            new ProjectBuildCommandLogExecutable();

        private static readonly DeleteFolderCommandLineExecutable _deleteFolderCommand = 
            new DeleteFolderCommandLineExecutable("");

        private static OpenFolderCommandLineExecutable OpenFolderCommand => new OpenFolderCommandLineExecutable("");

        public static async void InitializeBuild()
        {
            await _projectBuildLogCommand.ExecuteAsync();

            int projectBackupExitCode = await _projectBackupCommand.ExecuteAsync();

            if (projectBackupExitCode >= _projectBackupCommand.FailureExitCode)
            {
                throw new CommandLineException(SharedCommandLineUtilities.RoboCopyApplication, 
                    "Project backup", projectBackupExitCode);
            }

            int projectBuildExitCode = await _projectBuildCommand.ExecuteAsync();

            if (projectBuildExitCode >= _projectBuildCommand.FailureExitCode)
            {
                throw new CommandLineException(SharedCommandLineUtilities.UnityEditorApplication,
                    "Project build", projectBuildExitCode);
            }

            int deleteTempBuildFolderExitCode = await _deleteFolderCommand.ExecuteAsync();

            if (deleteTempBuildFolderExitCode >= _deleteFolderCommand.FailureExitCode)
            {
                throw new CommandLineException(SharedCommandLineUtilities.UnityEditorApplication,
                    "Delete temp build folder", deleteTempBuildFolderExitCode);
            }

            OpenFolderCommand.Execute();
        }

        public static void BuildPlayer()
        {
            Debug.Log("~Unity build player is starting, getting build options...");
            BuildPlayerOptions buildOptions = (BuildPlayerOptions)Storage.DeserializeFromJsonFile<ProjectBuildOptions>(SharedProjectDirectory.TemporaryBuildConfigPath);

            Debug.Log($"~Active Build Target: {EditorUserBuildSettings.activeBuildTarget}");
            Debug.Log($"~Requested Build Target: {buildOptions.target}");
            Debug.Log($"~Android Architectures: {PlayerSettings.Android.targetArchitectures}");

            BuildReport report = BuildPipeline.BuildPlayer(buildOptions);

            Debug.Log($"~{report.summary.platform} build started time: {report.summary.buildStartedAt}");
            Debug.Log($"~Build total size: {report.summary.totalSize}");
            Debug.Log($"~Build output path: {report.summary.outputPath}");
            Debug.Log($"~{report.summary.platform} build completed time: {report.summary.buildEndedAt}");
            Debug.Log($"~Build results: {report.summary.result}");
            Debug.Log($"~Build total warnings found: {report.summary.totalWarnings}");
            Debug.Log($"~Build total warnings found: {report.summary.totalErrors}");

            if(report.summary.result != BuildResult.Succeeded)
            {
                throw new Exception($"{report.summary.platform} player build failed with results: {report.summary.result}.");
            }
        }
    }
}
