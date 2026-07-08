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
    public static class SharedBatchCommands
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
                   SharedCommonFiltersAndPatterns.CopySubDirectoriesIncludingEmpty,
                   SharedCommonFiltersAndPatterns.EnableMultiThreading,
                   SharedCommonFiltersAndPatterns.RetryOnceOnFail,
                   SharedCommonFiltersAndPatterns.WaitASecondBeforeRetry
                };

            arguments.Add(SharedCommonFiltersAndPatterns.ExcludeDirectories);
            arguments.AddRange(SharedCommonFiltersAndPatterns.CommonExcludedUnityFolders);

            arguments.Add(SharedCommonFiltersAndPatterns.ExcludeFiles);
            arguments.AddRange(SharedCommonFiltersAndPatterns.CommonExcludedUnityFileExtensions);

            return arguments.AsReadOnly();
        }
    }
}
