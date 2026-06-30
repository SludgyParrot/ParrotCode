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
using System.Collections.Generic;
#endregion

#region Included Unity Assemblies
using UnityEngine;
using UnityEngine.Rendering;
using UnityEditor;
#endregion

namespace ParrotCode.Platforms
{
    /// <summary>
    /// A configuration file for configuring Android platform project rendering settings.
    /// </summary>
    [Serializable]
    public sealed class AndroidRenderingProjectBuildConfig : BaseRenderingProjectBuildConfig
    {
        #region Platform Settings
        [SerializeField, Space(5), Tooltip("Determines if multi threaded reandering should be enabled or not.")]
        private bool enableMultiThreadedRendering;

        [SerializeField, Space(5), Tooltip("Determines if graphics job should be enabled or not [Unity Experimental].")]
        private bool enableGraphicsJob;

        private BuildTarget buildTarget = BuildTarget.Android;

        public bool EnableMultiThreadedRendering => enableMultiThreadedRendering;
        public bool EnableGraphicsJob => enableGraphicsJob;
        public BuildTarget BuildTarget => buildTarget;
        #endregion

        #region Graphic API Validators
        private IReadOnlyList<GraphicsDeviceType> supportedGraphicsAPI;
        private IReadOnlyList<GraphicsDeviceType> deprecateddGraphicsAPI;

        public override IReadOnlyList<GraphicsDeviceType>  SupportedGraphicsAPI
        {
            get
            {
                if (supportedGraphicsAPI == null || supportedGraphicsAPI.Count == 0)
                {
                    supportedGraphicsAPI = new List<GraphicsDeviceType>()
                    {
                           GraphicsDeviceType.Vulkan,
                           GraphicsDeviceType.OpenGLES3,
                    };
                }
                return supportedGraphicsAPI;
            }
        }

        public override IReadOnlyList<GraphicsDeviceType> DeprecatedGraphicsAPI
        {
            get
            {
                if (deprecateddGraphicsAPI == null || deprecateddGraphicsAPI.Count == 0)
                {
                    deprecateddGraphicsAPI = new List<GraphicsDeviceType>()
                    {
                           GraphicsDeviceType.OpenGLES2,
                    };
                }
                return deprecateddGraphicsAPI;
            }
        }
        #endregion

        public override void ApplySettings()
        {
            PlayerSettings.MTRendering = EnableMultiThreadedRendering;
            PlayerSettings.graphicsJobs = EnableGraphicsJob;
        }
    }
}
