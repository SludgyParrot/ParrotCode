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

using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

namespace ParrotCode.Platforms
{
    /// <summary>
    /// This class encapsulates a collection of common Android specific rendering settings.
    /// </summary>
    [Serializable]
    public sealed class AndroidRenderingProjectBuildConfig : IRenderingProjectBuildConfig
    {
        [SerializeField, Tooltip("Determines whether Unity automatically selects the graphics backend " +
            "for a platform (DirectX, Vulkan, Metal, OpenGL, etc.) or whether you explicitly define the order.")]
        private bool useAutoGraphicsAPI;

        [SerializeField, Space(5), Tooltip("Specify a list of graphics API " +
            "(DirectX, Vulkan, Metal, OpenGL, etc.) to use for the selected platform.")]
        private GraphicsDeviceType[] graphicsAPI;

        [SerializeField, Space(5), Tooltip("Determines if multi threaded reandering should be enabled or not.")]
        private bool enableMultiThreadedRendering;

        [SerializeField, Space(5), Tooltip("Determines if graphics job should be enabled or not [Unity Experimental].")]
        private bool enableGraphicsJob;

        private BuildTarget buildTarget = BuildTarget.Android;

        public bool UseAutoGraphicsAPI => useAutoGraphicsAPI;
        public IReadOnlyList<GraphicsDeviceType> GraphicsAPI => graphicsAPI;
        public bool EnableMultiThreadedRendering => enableMultiThreadedRendering;
        public bool EnableGraphicsJob => enableGraphicsJob;
        public BuildTarget BuildTarget => buildTarget;

        public (bool hasUnsupportedAPI, GraphicsDeviceType[] graphicAPIs) UnsupportedAPICheckResults()
        {
            var unsupportedAssignedGraphicsAPIs = GraphicsAPI.Select(x => x).Where(api => api != GraphicsDeviceType.Vulkan || api != GraphicsDeviceType.OpenGLES3).ToArray();
            return (unsupportedAssignedGraphicsAPIs.Length > 0, unsupportedAssignedGraphicsAPIs);
        }

        public void ApplySettings()
        {
            PlayerSettings.SetUseDefaultGraphicsAPIs(BuildTarget, UseAutoGraphicsAPI);
            PlayerSettings.SetGraphicsAPIs(BuildTarget, graphicsAPI);

            PlayerSettings.MTRendering = EnableMultiThreadedRendering;
            PlayerSettings.graphicsJobs = EnableGraphicsJob;
        }
    }
}
