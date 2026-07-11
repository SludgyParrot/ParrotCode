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
using UnityEngine.Rendering;
#endregion

namespace ParrotCode.Platforms
{
    /// <summary>
    /// A configuration file for configuring iOS platform project rendering settings.
    /// </summary>
    [Serializable]
    public sealed class IOSRenderingProjectBuildConfig : BaseRenderingProjectBuildConfig
    {
        #region Graphic API Validators
        private IReadOnlyList<GraphicsDeviceType> supportedGraphicsAPI;
        private IReadOnlyList<GraphicsDeviceType> deprecateddGraphicsAPI;

        public override IReadOnlyList<GraphicsDeviceType> SupportedGraphicsAPI
        {
            get
            {
                if (supportedGraphicsAPI == null || supportedGraphicsAPI.Count == 0)
                {
                    supportedGraphicsAPI = new List<GraphicsDeviceType>()
                    {
                           GraphicsDeviceType.Metal,
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
                    deprecateddGraphicsAPI = new List<GraphicsDeviceType>();
                }
                return deprecateddGraphicsAPI;
            }
        }
        #endregion

        public override void ApplySettings()
        {
           
        }
    }
}
