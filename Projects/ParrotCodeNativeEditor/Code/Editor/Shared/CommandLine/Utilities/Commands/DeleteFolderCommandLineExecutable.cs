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
using System.IO;
using System.Threading.Tasks;
#endregion

namespace ParrotCode.Native.SharedEditor
{
    /// <summary>
    /// Represents a command-line executable that deletes an existing directory
    /// and all of its contents.
    /// </summary>
    /// <remarks>
    /// This command executes the operating system's directory removal command
    /// using the arguments defined by
    /// <see cref="SharedCommandLineUtilities.RemoveDirectoryArguments"/>.
    /// The target directory must exist when an instance of this class is created.
    /// </remarks>
    public sealed class DeleteFolderCommandLineExecutable : CommandLineExecutable
    {
        /// <summary>
        /// The directory to delete.
        /// </summary>
        private readonly string _directory;

        /// <summary>
        /// Gets the minimum exit code that indicates the delete operation failed.
        /// </summary>
        public override int FailureExitCode => 1;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="DeleteFolderCommandLineExecutable"/> class.
        /// </summary>
        /// <param name="directory">
        /// The path of the directory to delete.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="directory"/> is <see langword="null"/>,
        /// empty, or consists only of white-space characters.
        /// </exception>
        /// <exception cref="IOException">
        /// Thrown if the specified directory does not exist.
        /// </exception>
        public DeleteFolderCommandLineExecutable(string directory)
        {
            if (string.IsNullOrWhiteSpace(directory))
            {
                throw new ArgumentNullException(nameof(directory),
                    "Directory cannot be null or consist entirely of white space.");
            }

            if (!Directory.Exists(directory))
            {
                throw new IOException($"Provided directory: {directory} doesn't exist.");
            }

            _directory = directory;
        }

        /// <summary>
        /// Deletes the configured directory asynchronously.
        /// </summary>
        /// <returns>
        /// A task that represents the asynchronous delete operation. The task
        /// result contains the exit code returned by the directory removal
        /// command.
        /// </returns>
        /// <remarks>
        /// The directory is removed recursively using the command-line arguments
        /// defined by <see cref="SharedCommandLineUtilities.RemoveDirectoryArguments"/>.
        /// The returned exit code should be compared against
        /// <see cref="FailureExitCode"/> to determine whether the operation
        /// completed successfully.
        /// </remarks>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the command-line process could not be started.
        /// </exception>
        public override async Task<int> ExecuteAsync()
        {
            string arguments = $" {string.Join(" ", SharedCommandLineUtilities.RemoveDirectoryArguments)}" +
                $" \"{_directory}\"";

            ProcessStartInfo deleteFolderProcessInfo = new ProcessStartInfo
            {
                FileName = SharedCommandLineUtilities.CMDApplication,
                Arguments = arguments,
                UseShellExecute = true,
            };

            return await CommandLineUtility.RunAsync(
                deleteFolderProcessInfo,
                FailureExitCode);
        }
    }
}
