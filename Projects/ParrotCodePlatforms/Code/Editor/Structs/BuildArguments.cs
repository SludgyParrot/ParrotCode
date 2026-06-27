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
using System.Diagnostics;
#endregion

#region Included JetBrains Assemblies
using JetBrains.Annotations;
#endregion

#region Included Unity Assemblies
using UnityEditor;
#endregion

namespace ParrotCode.Platforms
{
    /// <summary>
    /// This struct contains build arguments for executing Unity build processes using batch.
    /// </summary>
    public readonly struct BuildArguments
    {
        private readonly string unityExecutableFilePath;
        private readonly string projectPath;
        private readonly string buildExecutionMethodName;
        private readonly string logFile;

        private readonly bool useShellExecute;
        private readonly bool acceptAPIUpdates;
        private readonly bool noGraphics;

        /// <summary>
        /// This constructs a <see cref=BuildArguments"/>
        /// </summary>
        /// <param name="projectPath">The path of the targeted Unity project to build.</param>
        /// <param name="buildExecutionMethodName">
        /// Fully qualified static method to execute (e.g. BuildScript.PerformBuild).
        /// </param>
        /// <param name="logFile">
        /// Optional log file path. Defaults to "build.log".
        /// </param>
        /// <param name="useShellExecute">
        /// Whether the process should use the operating system shell.
        /// </param>
        public BuildArguments(string projectPath, 
            string buildExecutionMethodName, 
            [CanBeNull] string logFile = null,
            bool useShellExecute = true,
            bool acceptAPIUpdates = false, 
            bool noGraphics = false)
        {
            if(string.IsNullOrWhiteSpace(projectPath))
                throw new ArgumentException("Project path cannot be null, empty or entirely white space", nameof(projectPath));

            if (string.IsNullOrWhiteSpace(buildExecutionMethodName))
                throw new ArgumentException("Build execution method name cannot be null, empty or entirely white space.", nameof(buildExecutionMethodName));

            unityExecutableFilePath = EditorApplication.applicationPath;

            this.buildExecutionMethodName = buildExecutionMethodName;
            this.projectPath = projectPath;
            this.logFile = logFile ?? "build.log";
            this.useShellExecute = useShellExecute;
            this.acceptAPIUpdates = acceptAPIUpdates;
            this.noGraphics = noGraphics;
        }

        /// <summary>
        /// Converts a <see cref="BuildArguments"/> into a <see cref="ProcessStartInfo"/>
        /// </summary>
        /// <param name="additionalArguments">
        /// Additional arguments to append to the Unity command line.
        /// Each entry must be a single argument.
        ///
        /// Correct:
        /// "-buildTarget", "Android, -customBuildName", "Parrot"
        ///
        /// Incorrect:
        /// "-buildTarget Android, -customBuildName Parrot"
        /// or "-buildTarget Android -customBuildName Parrot"
        /// </param>
        /// <returns><see cref="ProcessStartInfo"/></returns>
        public ProcessStartInfo ToProcessStartInfo(params string[] additionalArguments)
        {
            ProcessStartInfo info = new ProcessStartInfo()
            {
                FileName = unityExecutableFilePath,
                UseShellExecute = useShellExecute,
            };

            #region Add Arguments
            info.ArgumentList.Add(UnityCommandLineFlags.BatchMode);
            info.ArgumentList.Add(UnityCommandLineFlags.Quit);
            info.ArgumentList.Add(UnityCommandLineFlags.ProjectPath);
            info.ArgumentList.Add(projectPath);
            info.ArgumentList.Add(UnityCommandLineFlags.ExecuteMethod);
            info.ArgumentList.Add(buildExecutionMethodName);
            info.ArgumentList.Add(UnityCommandLineFlags.LogFile);
            info.ArgumentList.Add(logFile);
            #endregion

            #region Add Conditional Arguments
            if(acceptAPIUpdates)
            {
                info.ArgumentList.Add(UnityCommandLineFlags.AcceptAPIUpdate);
            }

            if(noGraphics)
            {
                info.ArgumentList.Add(UnityCommandLineFlags.NoGraphics);
            }
            #endregion

            #region Add Additional Arguments
            if (additionalArguments.Length > 0)
            {
                foreach(string argument in additionalArguments)
                {
                    if(string.IsNullOrWhiteSpace(argument))
                        continue;

                    info.ArgumentList.Add(argument);
                }
            }
            #endregion

            return info;
        }
    }
}
