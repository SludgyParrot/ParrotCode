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
#endregion

#region Included Jet Brains Assemblies
using JetBrains.Annotations;
#endregion

#region Included Parrot Code Assemblies
using ParrotCode.Native.SharedEditor;
#endregion

namespace ParrotCode.Platforms
{
    public sealed class AssignedRenderingPlatformValidationRule : ConfigValidationRule<ProjectBuildRenderingConfig>
    {
        public override int Order => 0;

        public override HelpBoxMessage Validate(ProjectBuildRenderingConfig config,[CanBeNull] object data = null)
        {
            if (config.GraphicsAPI != null && config.GraphicsAPI.Count > 0)
                return HelpBoxMessage.Empty;

            IRenderingProjectBuildConfig renderingProjectBuild = (IRenderingProjectBuildConfig)data;

            if (renderingProjectBuild == null)
            {
                return HelpBoxMessage.Empty;
            }

            string supportedGraphicsAPIs = string.Join("\n", renderingProjectBuild.SupportedGraphicsAPI.Select(graphicsAPI => $"* {graphicsAPI.ToString()}"));

            string validationErrorMessage = $"There are no graphics APIs defined for build target: {config.BuildTarget}." +
                $" \nUnity will automatically select one of the following supported graphics API(s) for {config.BuildTarget}: \n\n{supportedGraphicsAPIs}\n";

            return new HelpBoxMessage(validationErrorMessage, MessageType.Info);
        }
    }
}
