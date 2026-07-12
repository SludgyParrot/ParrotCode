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

#region Included Parrot Code Assemblies
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
#endregion

namespace ParrotCode.Native.SharedEditor
{
    /// <summary>
    /// Provides commonly used executable names and command-line utilities used by
    /// the Parrot Code build and automation framework.
    /// </summary>
    public static class SharedCommandLineUtilities
    {
        #region Process Applications

        /// <summary>
        /// The executable name for the Windows Command Prompt application.
        /// </summary>
        /// <remarks>
        /// This value resolves to <c>cmd.exe</c> and can be used to execute
        /// Windows command-line commands or batch scripts.
        /// </remarks>
        public const string CMDApplication = "cmd.exe";

        /// <summary>
        /// The executable name for the Windows Robocopy utility.
        /// </summary>
        /// <remarks>
        /// Robocopy is a high-performance file copy utility included with
        /// Windows. It is commonly used for copying directories, incremental
        /// backups, and mirroring file structures.
        /// </remarks>
        public const string RoboCopyApplication = "robocopy.exe";

        /// <summary>
        /// The executable name for the Unity Editor application.
        /// </summary>
        /// <remarks>
        /// This value resolves to <c>unity.exe</c>. It can be used to launch
        /// Unity from external processes or automation scripts when the editor
        /// executable is available on the system.
        /// </remarks>
        public const string UnityEditorApplication = "unity.exe";

        #endregion

        /// <summary>
        /// Gets the default set of command-line arguments used when invoking
        /// the Windows <c>robocopy</c> utility.
        /// </summary>
        /// <value>
        /// A collection containing the default Robocopy command-line arguments
        /// and exclusion rules.
        /// </value>
        /// <remarks>
        /// The default arguments enable recursive copying, multithreaded file
        /// transfers, limited retry behavior, and exclude common Unity-generated
        /// directories and file extensions.
        /// </remarks>
        public static string[] RoboCopyArguments => CreateRoboCopyArguments().ToArray();

        /// <summary>
        /// Gets the default command-line arguments used to launch the Unity Editor
        /// for an automated build.
        /// </summary>
        /// <value>
        /// An array containing the default Unity command-line arguments.
        /// </value>
        /// <remarks>
        /// The returned arguments include the temporary project path and common Unity
        /// command-line options required to execute a build. A new array is created
        /// each time this property is accessed, allowing callers to modify the
        /// returned collection without affecting subsequent calls.
        /// </remarks>
        public static string[] UnityBuildArguments => CreateUnityBuildArguments().ToArray();

        /// <summary>
        /// Gets the default command-line arguments used to display the project build log
        /// in a command prompt.
        /// </summary>
        /// <value>
        /// An array containing the base <c>cmd.exe</c> arguments required to open a
        /// console window and display the project build log.
        /// </value>
        /// <remarks>
        /// The returned arguments are intended to be extended with the path to the
        /// build log file before being passed to <c>cmd.exe</c>.
        /// </remarks>
        public static string[] ProjectBuildLogArguments => CreateProjectBuildLogArguments().ToArray();

        /// <summary>
        /// Gets the default command-line arguments used to remove a directory and
        /// all of its contents.
        /// </summary>
        /// <value>
        /// An array containing the arguments required to recursively delete a
        /// directory without prompting for confirmation.
        /// </value>
        /// <remarks>
        /// The returned arguments are intended to be combined with the target
        /// directory path before being passed to <c>cmd.exe</c> or another
        /// command-line process that performs directory removal.
        /// </remarks>
        public static string[] RemoveDirectoryArguments => CreateRemoveDirectoryArguments().ToArray();

        /// <summary>
        /// Gets the default set of command-line arguments used when invoking
        /// the Windows <c>robocopy</c> utility.
        /// </summary>
        /// <returns>
        /// A read-only collection containing the default Robocopy command-line
        /// arguments and exclusion rules.
        /// </returns>
        /// <remarks>
        /// The returned arguments enable recursive copying, multithreaded file
        /// transfers, limited retry behavior, and exclude common Unity-generated
        /// directories and file extensions.
        /// </remarks>
        private static IReadOnlyList<string> CreateRoboCopyArguments()
        {
            List<string> arguments = new List<string>
                {
                   CommandLineSwitch.CopySubDirectoriesIncludingEmpty,
                   CommandLineSwitch.EnableMultiThreading,
                   CommandLineSwitch.RetryOnceOnFail,
                   CommandLineSwitch.WaitASecondBeforeRetry
                };

            arguments.Add(CommandLineSwitch.ExcludeDirectories);
            arguments.AddRange(CommandLineSwitch.CommonExcludedUnityFolders);

            arguments.Add(CommandLineSwitch.ExcludeFiles);
            arguments.AddRange(CommandLineSwitch.CommonExcludedUnityFileExtensions);

            arguments.Add(CommandLineSwitch.RobocopyLogFile);

            return arguments.AsReadOnly();
        }

        /// <summary>
        /// Creates the default command-line arguments used to launch the Unity Editor
        /// in batch mode for an automated build.
        /// </summary>
        /// <returns>
        /// A read-only collection containing the command-line arguments required to
        /// execute a Unity build process.
        /// </returns>
        /// <remarks>
        /// The returned arguments include the temporary project path and common Unity
        /// command-line options, such as the project path and execute method.
        /// Additional arguments may be appended by the caller before launching the
        /// Unity Editor process.
        /// </remarks>
        private static IReadOnlyList<string> CreateUnityBuildArguments()
        {
            return new List<string>
            {
                CommandLineSwitch.BatchMode,
                CommandLineSwitch.Quit,
                CommandLineSwitch.ProjectPath,
                SharedNativeProjectDirectory.TemporaryBuildProjectPath,
                CommandLineSwitch.ExecudeMethod
            };
        }

        /// <summary>
        /// Creates the command-line arguments used to open a command prompt and display
        /// the project build log.
        /// </summary>
        /// <remarks>
        /// The returned arguments are intended to be passed to <c>cmd.exe</c>.
        /// They keep the console window open after execution, print a descriptive
        /// message identifying the build log, and display the contents of the log file.
        /// <para>
        /// Additional arguments, such as the build log file path, should be appended
        /// by the caller before executing the command.
        /// </para>
        /// </remarks>
        /// <returns>
        /// An immutable collection containing the base command-line arguments required
        /// to display the project build log.
        /// </returns>
        private static IReadOnlyList<string> CreateProjectBuildLogArguments()
        {
            return new List<string>
            {
                CommandLineSwitch.KeepConsoleWindowOpen,
                CommandLineSwitch.Type
            };
        }

        /// <summary>
        /// Creates the default command-line arguments used to remove a directory and
        /// all of its contents.
        /// </summary>
        /// <remarks>
        /// The returned arguments are intended to be passed to <c>cmd.exe</c> when
        /// executing the directory removal command. The arguments perform the
        /// following actions:
        /// <list type="bullet">
        /// <item>
        /// <description>Removes the specified directory.</description>
        /// </item>
        /// <item>
        /// <description>Includes all subdirectories in the removal operation.</description>
        /// </item>
        /// <item>
        /// <description>Suppresses confirmation prompts by enabling quiet mode.</description>
        /// </item>
        /// </list>
        /// The target directory path should be appended by the caller before executing
        /// the command.
        /// </remarks>
        /// <returns>
        /// An immutable collection containing the default command-line arguments used
        /// to remove a directory.
        /// </returns>
        private static IReadOnlyList<string> CreateRemoveDirectoryArguments()
        {
            return new List<string>
            {
                CommandLineSwitch.CloseConsoleWindowOnExit,
                CommandLineSwitch.RemoveDirectory,
                CommandLineSwitch.IncludeSubdirectories,
                CommandLineSwitch.QuietMode
            };
        }
    }
}
