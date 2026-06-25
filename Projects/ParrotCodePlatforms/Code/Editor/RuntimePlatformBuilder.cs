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

#region System
using System;
#endregion

#region Unity
using UnityEditor;
using UnityEngine;
#endregion

#region Parrot Code
using ParrotCode.Extensions;
using System.Diagnostics;
#endregion

namespace ParrotCode.Platforms
{
    public static class RuntimePlatformBuilder
    {
        public static void Build(string buildPath)
        {
            if (CustomInspectorEditorPopup.CancelledDuringUserSceneChangesSaveRequest())
                return;

            if(!IsCurrentBuildPlatformSupported())
                return;

            Process.Start(new ProcessStartInfo
            {
                FileName = CMDFileName(),
                Arguments = BuildExecutionArgument(),
                UseShellExecute = true,
            });
        }

        public static string BuildExecutionArgument()
            => $"@echo off\r\n\r\n\"C:\\Program Files\\Unity\\Hub\\Editor\\2023.2.20f1\\Editor\\Unity.exe\" ^\r\n    -batchmode ^\r\n    -quit ^\r\n    -projectPath \"%~dp0\" ^\r\n    -executeMethod BuildScript.PerformBuild\r\n\r\npause";

        public static string CMDFileName()
            => "cm.exe";

        private static bool IsCurrentBuildPlatformSupported()
        {
            var buildTarget = EditorUserBuildSettings.activeBuildTarget;
            var targetGroup = buildTarget.ToBuildTargetGroup();

            if (!BuildPipeline.IsBuildTargetSupported(targetGroup, buildTarget))
            {
                UnityEngine.Debug.LogException(new NotSupportedException($"Build target: {buildTarget} of target group: {targetGroup} is currently not supported."));
                return false;
            }

            return true;
        }
    }
}
