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
using UnityEngine.Rendering;

namespace ParrotCode.Platforms
{
    /// <summary>
    /// An interface for <see cref="RenderingProjectBuildConfig"/>
    /// </summary>
    public interface IRenderingProjectBuildConfig
    {
        IReadOnlyList<GraphicsDeviceType> SupportedGraphicsAPI { get; }
        IReadOnlyList<GraphicsDeviceType> DeprecatedGraphicsAPI { get; }

        /// <summary>
        /// A list of detected unsupported graphics APIs.
        /// </summary>
        /// <param name="selectedGraphicsAPIs">A readonly list of explicitly selected graphics APIs.</param>
        /// <returns>A readonly list of unsupported graphics APIs.</returns>
        IReadOnlyList<GraphicsDeviceType> UnsupportedGraphicsAPIFound(IReadOnlyList<GraphicsDeviceType> selectedGraphicsAPIs);

        /// <summary>
        /// A list of detected deprecated graphics APIs.
        /// </summary>
        /// <param name="selectedGraphicsAPIs">A readonly list of explicitly selected graphics APIs.</param>
        /// <returns>A readonly list of deprecated graphics APIs</returns>
        IReadOnlyList<GraphicsDeviceType> DeprecatedGraphicsAPIFound(IReadOnlyList<GraphicsDeviceType> selectedGraphicsAPIs);

        /// <summary>
        /// Applies the configurations to the target platform. e.g Android, Windows, WebGL etc.
        /// </summary>
        void ApplySettings();
    }
}
