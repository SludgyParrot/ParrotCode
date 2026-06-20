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

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ParrotCode.Platforms
{
    /// <summary>
    /// This class 
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    [CreateAssetMenu(fileName = "Rendering Settings", menuName = ProjectSharedDirectory.PlatformConfigRootPath + "Rendering Settings")]
    public sealed class RenderingProjectBuildConfig : ProjectSpecificBuildConfig
    {
        #region General Rendering Settings
        [SerializeField, Space(5)]
        private GeneralRenderingProjectBuildConfig generalSettings = new GeneralRenderingProjectBuildConfig();

        /// <summary>
        /// General rendering settings for configuring general platform rendering settings
        /// </summary>
        public GeneralRenderingProjectBuildConfig GeneralSettings => generalSettings;
        #endregion

        #region Android Rendering Settings
        [SerializeField, Space(5)]
        private AndroidRenderingProjectBuildConfig androidSettings = new AndroidRenderingProjectBuildConfig();
        public AndroidRenderingProjectBuildConfig AndroidSettings => androidSettings;
        #endregion

        #region IOS Rendering Settings
        [SerializeField, Space(5)]
        private IOSRenderingProjectBuildConfig iosSettings = new IOSRenderingProjectBuildConfig();
        public IOSRenderingProjectBuildConfig IOSSettings => iosSettings;
        #endregion

        #region Windows Rendering Settings
        [SerializeField, Space(5)]
        private WindowsRenderingProjectBuildConfig windowsSettings = new WindowsRenderingProjectBuildConfig();
        public WindowsRenderingProjectBuildConfig WindowsSettings => windowsSettings;
        #endregion

        #region WebGL Rendering Settings
        [SerializeField, Space(5)]
        private WebGLRenderingProjectBuildConfig webGLSettings = new WebGLRenderingProjectBuildConfig();
        public WebGLRenderingProjectBuildConfig WebGLSettings => webGLSettings;
        #endregion

        #region Settings
        private readonly Dictionary<BuildTarget, IRenderingProjectBuildConfig> renderingSettings 
            = new Dictionary<BuildTarget, IRenderingProjectBuildConfig>();
        #endregion

        public override BuildTarget BuildTarget => EditorUserBuildSettings.activeBuildTarget;

        private void OnEnable()
        {
            InitializeRenderingSettings();
        }

        private void OnDisable() 
            => OnReset();

        private void InitializeRenderingSettings()
        {
            renderingSettings[BuildTarget.Android] = AndroidSettings;
            renderingSettings[BuildTarget.iOS] = IOSSettings;
            renderingSettings[BuildTarget.StandaloneWindows] = WindowsSettings;
            renderingSettings[BuildTarget.StandaloneWindows64] = WindowsSettings;
            renderingSettings[BuildTarget.WebGL] = WebGLSettings;
        }

        private void OnReset()
        {
            renderingSettings.Clear();
        }

        public override void ApplySettings()
        {
            if(!renderingSettings.TryGetValue(BuildTarget, out IRenderingProjectBuildConfig settings))
            {
                Debug.LogError($"[{name}] Apply project rendering settings for build target: {BuildTarget} failed. " +
                    $"Build target: {BuildTarget} is not currently supported on this version of Parrot Code framework.");
                return;
            }

            GeneralSettings.ApplySettings();
            settings.ApplySettings();
        }
    }
}
