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
using System.Collections.Generic;
using System;
#endregion

namespace ParrotCode.Native.SharedEditor
{
    public static class CommandLineSwitch
    {
        public static readonly IReadOnlyList<string> CommonExcludedUnityFolders =
            Array.AsReadOnly(new string[]
            {
                "Library",
                "Temp",
                "Logs",
                "obj",
            });

        public static readonly IReadOnlyList<string> CommonExcludedUnityFileExtensions =
           Array.AsReadOnly(new string[]
           {
                "*.lock"
           });

        public const string CopySubDirectoriesIncludingEmpty = "/E";
        public const string ExcludeDirectories = "/XD";
        public const string ExcludeFiles = "/XF";
        public const string RetryOnceOnFail = "/R:1";
        public const string WaitASecondBeforeRetry = "/W:1";
        public const string EnableMultiThreading = "/MT";
        public const string RoboCopy = "robocopy";
        public const string CommandWindow = "cmd.exe";
        public const string KeepConsoleWindowOpen = "/k";
        public const string CloseConsoleWindowOnExit = "/c";
        public const string Type = "type";
        public const string Quit = "-quit";
        public const string BatchMode = "-batchmode";
        public const string ProjectPath = "-projectPath";
        public const string ExecudeMethod = "-executeMethod";
        public const string UnityLogFile = "-logFile";
        public const string RobocopyLogFile = "/LOG:";
        public const string Echo = "echo";
        public const string RemoveDirectory = "rmdir";
        public const string IncludeSubdirectories = "/s";
        public const string QuietMode = "/q";
        public const string CopyCMD = "copy";
        public const string MoveCMD = "move";
        public const string Mov = "/Mov";
        public const string Move = "/Move";
    }
}
