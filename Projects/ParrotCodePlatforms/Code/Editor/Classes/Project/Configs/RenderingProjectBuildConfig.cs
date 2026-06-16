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

using UnityEditor;
using UnityEngine;

namespace ParrotCode.Platforms
{
    [CreateAssetMenu(fileName = "Rendering Settings", menuName = ProjectSharedDirectory.PlatformConfigRootPath + "Rendering Settings")]
    public sealed class RenderingProjectBuildConfig : ProjectSpecificBuildConfig
    {
        [SerializeField]
        private BuildTarget buildTarget;
        public override BuildTarget BuildTarget => buildTarget;

        #region Android Rendering Settings
        [SerializeField, Space(5)]
        private AndroidRenderingProjectBuildConfig androidSettings;
        public AndroidRenderingProjectBuildConfig AndroidSettings => androidSettings;
        #endregion

        #region IOS Rendering Settings
        [SerializeField, Space(5)]
        private IOSRenderingProjectBuildConfig iosSettings;
        public IOSRenderingProjectBuildConfig IOSSettings => iosSettings;
        #endregion

        #region Windows Rendering Settings
        [SerializeField, Space(5)]
        private WindowsRenderingProjectBuildConfig windowsSettings;
        public WindowsRenderingProjectBuildConfig WindowsSettings => windowsSettings;
        #endregion

        #region WebGL Rendering Settings
        [SerializeField, Space(5)]
        private WebGLRenderingProjectBuildConfig webGLSettings;
        public WebGLRenderingProjectBuildConfig WebGLSettings => webGLSettings;
        #endregion

        public override void ApplySettings()
        {
            
        }
    }
}
