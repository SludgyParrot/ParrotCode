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
using System.Linq;
#endregion

#region Included Unity Assemblies
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;
#endregion

#region Included Parrot Code Assemblies
using ParrotCode.Native.SharedEditor;
#endregion

namespace ParrotCode.Platforms
{
    public sealed class PlatformConfigurator : IActiveBuildTargetChanged
    {
        public int callbackOrder => 0;

        public void OnActiveBuildTargetChanged(BuildTarget previousTarget, BuildTarget newTarget)
        {
            PlatformConfig platformConfig = GetPlatformConfigForTarget(newTarget);

            if(platformConfig == null)
            {
                Debug.LogWarning($"PlatformConfigurator couldn't configure build target: {newTarget}. Couldn't find any config file inside the project.");
                return;
            }

            Debug.Log($"Successfully switched from: {previousTarget} platform to: {newTarget}. Configuring platform.");
            ProjectAssetsDatabaseUtility.ClearCache();
            ConfigurePlatform(platformConfig);
        }

        private void ConfigurePlatform(PlatformConfig platformConfig)
        {
            switch (platformConfig)
            {
                case AndroidPlatformConfig androidPlatformConfig:
                    Debug.Log($"Successfully configured: {nameof(androidPlatformConfig)}.");
                    break;
                case WebGLPlatformConfig webGLPlatformConfig:
                    Debug.Log($"Successfully configured: {nameof(webGLPlatformConfig)}.");
                    break;
                case WindowsPlatformConfig windowsPlatformConfig:
                    Debug.Log($"Successfully configured: {nameof(windowsPlatformConfig)}.");
                    break;
                default:
                    throw new NotSupportedException($"PlatformConfigurator couldn't configure environment for platform: {nameof(platformConfig)}. Platform: '{nameof(platformConfig)}' is not currently supported.");
            }
        }

        private PlatformConfig GetPlatformConfigForTarget(BuildTarget buildTarget)
            => GetPlatformConfigs()?.FirstOrDefault(x => x.Platform == buildTarget);

        private PlatformConfig[] GetPlatformConfigs()
            => AssetImporter.FindObjectsByType<PlatformConfig>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID);
    }
}
