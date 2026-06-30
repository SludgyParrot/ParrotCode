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
using System.Linq;
#endregion

#region Included Unity Assemblies
using UnityEditor;
using UnityEngine.Rendering;
#endregion

#region Included Jet Brains Assemblies
using JetBrains.Annotations;
#endregion

#region Included Parrot Code Assemblies
using ParrotCode.Native.SharedEditor;
using System.Xml.Linq;
using System.Collections.Generic;
#endregion

namespace ParrotCode.Platforms
{
    public sealed class DeprecatedGraphicsAPIsValidationRule : ConfigValidationRule<ProjectBuildRenderingConfig>
    {
        public override int Order => 3;

        public override HelpBoxMessage Validate(ProjectBuildRenderingConfig config, [CanBeNull] object data = null)
        {
            IRenderingProjectBuildConfig renderingProjectBuild = (IRenderingProjectBuildConfig)data;

            if (renderingProjectBuild == null)
            {
                return HelpBoxMessage.Empty;
            }

            IReadOnlyList<GraphicsDeviceType> deprecatedGraphicsAPIs = renderingProjectBuild.DeprecatedGraphicsAPIFound(config.GraphicsAPI);

            if (deprecatedGraphicsAPIs.Count == 0)
                return HelpBoxMessage.Empty;

            string deprecatedGraphicsAPINames = string.Join("\n", deprecatedGraphicsAPIs.Select(graphicsAPI => $"* {graphicsAPI.ToString()}"));

            string validationErrorMessage = $"[Deprecated graphics API(s) detected] This configuration contains a deprecated graphic API(s) for build target: " +
                $"{config.BuildTarget}. \nPlease remove the following graphics API(s) from the graphics API list: \n\n{deprecatedGraphicsAPINames}\n";

            return new HelpBoxMessage(validationErrorMessage, MessageType.Warning);
        }
    }
}
