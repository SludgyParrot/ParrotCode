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
#endregion

namespace ParrotCode.Native.SharedEditor
{
    /// <summary>
    /// Represents a command-line executable that opens an existing directory
    /// using the operating system's default file explorer.
    /// </summary>
    /// <remarks>
    /// This command uses the Windows Shell to open the specified directory.
    /// The directory must exist when an instance of this class is created.
    /// </remarks>
    public sealed class OpenFolderCommandLineExecutable : CommandLineExecutable
    {
        /// <summary>
        /// The directory to open.
        /// </summary>
        private readonly string _directory;

        /// <summary>
        /// Initializes a new instance of the <see cref="OpenFolderCommandLineExecutable"/> class.
        /// </summary>
        /// <param name="directory">
        /// The path of the directory to open.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="directory"/> is <see langword="null"/>,
        /// empty, or consists only of white-space characters.
        /// </exception>
        /// <exception cref="IOException">
        /// Thrown if the specified directory does not exist.
        /// </exception>
        public OpenFolderCommandLineExecutable(string directory)
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
        /// Opens the configured directory using the operating system's default
        /// file explorer.
        /// </summary>
        /// <remarks>
        /// The directory is opened by launching it through the Windows Shell,
        /// allowing the operating system to determine the appropriate application
        /// for displaying the folder.
        /// </remarks>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the directory could not be opened.
        /// </exception>
        public override void Execute()
        {
            ProcessStartInfo directoryProcessInfo = new ProcessStartInfo
            {
                FileName = _directory,
                UseShellExecute = true,
            };

            CommandLineUtility.Run(directoryProcessInfo);
        }
    }
}
