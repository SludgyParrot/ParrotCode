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
using ParrotCode.Extensions;
using ParrotCode.Native;
#endregion

namespace ParrotCode.Platforms
{
    /// <summary>
    /// Provides shared file names used for logging throughout the build pipeline.
    /// </summary>
    /// <remarks>
    /// This class centralizes the names of log files to ensure they remain
    /// consistent across the framework.
    /// </remarks>
    public static class SharedLoggingFileNames
    {
        /// <summary>
        /// Gets the default build log file name, including its file extension.
        /// </summary>
        /// <value>
        /// The build log file name (for example, <c>Build.log</c>).
        /// </value>
        /// <remarks>
        /// This value does not include a directory path. Combine it with the
        /// appropriate directory when constructing the full path to the build log.
        /// </remarks>
        public static readonly string BuildLogFileName =
            $"Build{FileExtension.Log.Extension()}";
    }
}
