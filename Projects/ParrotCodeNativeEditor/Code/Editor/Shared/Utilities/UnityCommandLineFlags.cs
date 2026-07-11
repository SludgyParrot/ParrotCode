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

namespace ParrotCode.Native.SharedEditor
{
    /// <summary>
    /// Defines Unity command-line argument names used when launching the Unity Editor.
    /// </summary>
    public static class UnityCommandLineFlags
    {
        #region Flags
        /// <summary>
        /// Flag for setting batch mode.
        /// </summary>
        public const string BatchMode = "-batchmode";

        /// <summary>
        /// Flag for quitting the application.
        /// </summary>
        public const string Quit = "-quit";

        /// <summary>
        /// Flag for setting the target project path.
        /// </summary>
        public const string ProjectPath = "-projectPath";

        /// <summary>
        /// Flag for setting the method to execute.
        /// </summary>
        public const string ExecuteMethod = "-executeMethod";

        /// <summary>
        /// Flag for setting the build target.
        /// </summary>
        public const string BuildTarget = "-buildTarget";

        /// <summary>
        /// Flag for setting no graphics.
        /// </summary>
        public const string NoGraphics = "-nographics";

        /// <summary>
        /// Flag to accept API update.
        /// </summary>
        public const string AcceptAPIUpdate = "-accept-apiupdate";

        /// <summary>
        /// Flag for setting an output log file name.
        /// </summary>
        public const string LogFile = "-logFile";
        #endregion
    }
}
