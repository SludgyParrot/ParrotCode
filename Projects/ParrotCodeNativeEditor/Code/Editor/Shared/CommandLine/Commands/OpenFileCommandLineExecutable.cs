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
    /// Represents a command-line executable that opens an existing file using the
    /// operating system's default associated application.
    /// </summary>
    /// <remarks>
    /// This command uses the operating system's shell to open the specified file.
    /// The file must exist when an instance of this class is created.
    /// </remarks>
    public sealed class OpenFileCommandLineExecutable: CommandLineExecutable
    {
        /// <summary>
        /// The path to the file to open.
        /// </summary>
        private readonly string _filePath;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="OpenFileCommandLineExecutable"/> class.
        /// </summary>
        /// <param name="filePath">
        /// The absolute or relative file path of the file to open.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="filePath"/> is <see langword="null"/>, empty, or
        /// consists only of white-space characters.
        /// </exception>
        /// <exception cref="IOException">
        /// Thrown if the specified file does not exist.
        /// </exception>
        public OpenFileCommandLineExecutable(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentNullException(nameof(filePath),
                    "File path cannot be null," +
                    " or consist entirely of white space.");
            }

            if (!File.Exists(filePath))
            {
                throw new IOException($"Provided file path: {filePath} doesn't exist.");
            }

            _filePath = filePath;
        }

        /// <summary>
        /// Opens the configured file using the operating system's default
        /// associated application.
        /// </summary>
        /// <remarks>
        /// The file is opened through the operating system's shell, allowing the
        /// default application associated with the file type to determine how the
        /// file is displayed.
        /// </remarks>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the file could not be opened.
        /// </exception>
        public override void Execute()
        {
            ProcessStartInfo filePathProcessInfo = new ProcessStartInfo
            {
                FileName = _filePath,
                UseShellExecute = true,
            };

            CommandLineUtility.Run(filePathProcessInfo);
        }
    }
}
