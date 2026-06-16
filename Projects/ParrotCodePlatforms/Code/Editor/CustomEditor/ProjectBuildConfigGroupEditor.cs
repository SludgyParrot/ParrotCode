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

using System.Linq;
using UnityEditor;

namespace ParrotCode.Platforms
{
    [CustomEditor(typeof(ProjectBuildConfigGroup))]
    public sealed class ProjectBuildConfigGroupEditor: Editor
    {
        private const bool IsWideHelpBox = false;

        public override void OnInspectorGUI()
        {
            ValidateProjectBuildConfigGroup((ProjectBuildConfigGroup)target);
            DrawDefaultInspector();
        }

        private void ValidateProjectBuildConfigGroup(ProjectBuildConfigGroup projectBuildConfigGroup)
        {
            var projectBuildConfigs = projectBuildConfigGroup.ProjectBuildConfigs;

            if (projectBuildConfigs == null || projectBuildConfigs.Count == 0)
            {
                CustomInspectorValidations.DrawHelpBoxMessage(new HelpBoxMessage($"There are no build settings assigned for '{projectBuildConfigGroup.BuildTarget}'." +
                    $" This config group might be ignored.", MessageType.Warning, IsWideHelpBox));
                return;
            }


            var nullReferencesFoundCount = projectBuildConfigs.Count(x => x == null);

            if (nullReferencesFoundCount > 0)
            {
                CustomInspectorValidations.DrawHelpBoxMessage(new HelpBoxMessage($"Found {nullReferencesFoundCount} missing  '{nameof(projectBuildConfigs)}' reference(s) in:" +
                    $" {projectBuildConfigGroup.name}.", MessageType.Error, IsWideHelpBox));
                return;
            }

            var duplicatedConfigs = projectBuildConfigs.GroupBy(x => x.GetType()).Where(x => x.Skip(1).Any());

            foreach (var duplicatedConfig in duplicatedConfigs)
            {
                CustomInspectorValidations.DrawHelpBoxMessage(new HelpBoxMessage($"Duplicate '{nameof(projectBuildConfigs)}' detected. Found '{duplicatedConfig.Count()}' instances of '{duplicatedConfig.Key.Name}' in '{nameof(projectBuildConfigs)}' for" +
                    $" '{projectBuildConfigGroup.name}', and only '1' instance is alowed.", MessageType.Error, IsWideHelpBox));
            }

            var misconfiguredConfigs = projectBuildConfigs.OfType<ProjectSpecificBuildConfig>();

            foreach (ProjectSpecificBuildConfig misconfiguredConfig in misconfiguredConfigs)
            {
                if (misconfiguredConfig.BuildTarget == projectBuildConfigGroup.BuildTarget)
                    continue;

                CustomInspectorValidations.DrawHelpBoxMessage(new HelpBoxMessage($"Build target '{projectBuildConfigGroup.BuildTarget}' contains a '{nameof(misconfiguredConfig)}' file named" +
                    $" '{misconfiguredConfig.name}' with an incorrect target build type: '{misconfiguredConfig.BuildTarget}'.", MessageType.Error, IsWideHelpBox));
            }
        }
    }
}
