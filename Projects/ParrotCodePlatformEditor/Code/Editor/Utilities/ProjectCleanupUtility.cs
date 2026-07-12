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
#endregion

#region Included Parrot Code Assemblies
using ParrotCode.Native.SharedEditor;
#endregion

namespace ParrotCode.Platforms
{
    /// <summary>
    /// Provides utility methods for cleaning up temporary files and directories
    /// created during the build process.
    /// </summary>
    /// <remarks>
    /// This class is responsible for removing temporary build artifacts after a
    /// build has completed, including the temporary project backup and build
    /// configuration directory.
    /// </remarks>
    public static class ProjectCleanupUtility
    {
        /// <summary>
        /// Performs post-build cleanup by removing all temporary build resources.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous cleanup operation.
        /// </returns>
        /// <remarks>
        /// This method removes the temporary project backup.
        /// </remarks>
        public static async Task PostBuildProjectCleanup()
        {
            RemoveFolderCommandLineExecutable clearTempProjectBackupCommand =
               new RemoveFolderCommandLineExecutable(
                   SharedNativeProjectDirectory.TemporaryBuildProjectPath);

            await CommandTaskUtility.ExecuteCommandOrThrowAsync(
                clearTempProjectBackupCommand,
                "Clear backup");
        } 
    }
}
