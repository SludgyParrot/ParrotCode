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
        private const bool IsWideHelpBox = true;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            Validate((ProjectBuildConfigGroup)target);
        }

        private void Validate(ProjectBuildConfigGroup projectBuildConfigGroup)
        {

            if(CustomInspectorValidations.OnValidationFailed(projectBuildConfigGroup,
                ValidateAssignedProjectBuildConfigs, 
                new HelpBoxMessage($"There are no build settings assigned for '{projectBuildConfigGroup.BuildTarget}'. This config group might be ignored.", MessageType.Warning, IsWideHelpBox)))
            {
                return;
            }

            int nullReferenceProjectBuildConfigsCount = GetNullReferenceProjectBuildConfigsCount(projectBuildConfigGroup);

            if (nullReferenceProjectBuildConfigsCount > 0)
            {
                CustomInspectorValidations.DrawHelpBoxMessage(new HelpBoxMessage($"Found {nullReferenceProjectBuildConfigsCount} 'ProjectBuildConfig' null reference(s) in: {projectBuildConfigGroup.name}.", MessageType.Error, IsWideHelpBox));
                return;
            }

            ValidateMisconfiguredProjectBuildConfigs(projectBuildConfigGroup);
            ValidateDuplicateProjectBuildConfigs(projectBuildConfigGroup);
        }


        private bool ValidateAssignedProjectBuildConfigs(ProjectBuildConfigGroup projectBuildConfigGroup)
        {
            var projectBuildConfigs = projectBuildConfigGroup.ProjectBuildConfigs;
            return projectBuildConfigs != null && projectBuildConfigs.Count > 0;
        }

        private void ValidateMisconfiguredProjectBuildConfigs(ProjectBuildConfigGroup projectBuildConfigGroup)
        {
            var misconfiguredProjectBuildSettings = projectBuildConfigGroup.ProjectBuildConfigs.OfType<ProjectSpecificBuildConfig>();

            foreach (ProjectSpecificBuildConfig misconfiguredConfig in misconfiguredProjectBuildSettings)
            {
                if (misconfiguredConfig.BuildTarget == projectBuildConfigGroup.BuildTarget)
                    continue;

                CustomInspectorValidations.DrawHelpBoxMessage(new HelpBoxMessage($"Build target '{projectBuildConfigGroup.BuildTarget}' contains a project build config file named" +
                    $" '{misconfiguredConfig.name}' with an incorrect target build type: '{misconfiguredConfig.BuildTarget}'.", MessageType.Error, IsWideHelpBox));
            }
        }

        private void ValidateDuplicateProjectBuildConfigs(ProjectBuildConfigGroup projectBuildConfigGroup)
        {
            var duplicatedConfigGroups = projectBuildConfigGroup.ProjectBuildConfigs.GroupBy(group => group.GetType()).Where(group => group.Skip(1).Any());

            foreach (var group in duplicatedConfigGroups)
            {
                int groupedInstancesCount = group.Count();

                CustomInspectorValidations.DrawHelpBoxMessage(new HelpBoxMessage($"Duplicate config detected. Found '{groupedInstancesCount}' instances of '{group.Key.DeclaringType.Name}' in 'ProjectBuildConfigs' for" +
                    $" '{projectBuildConfigGroup.name}', and only '1' instance is allowed.", MessageType.Error, IsWideHelpBox));
            }

            var foundProjectBuildConfigDuplicatesResults = ProjectBuildConfigurator.GetProjectBuildConfigGroupDuplicatesCount(projectBuildConfigGroup);

            if (foundProjectBuildConfigDuplicatesResults.hasCopies)
            {
                string[] paths = foundProjectBuildConfigDuplicatesResults.paths;

                string duplicatedConfigPaths = string.Join("\n", paths);

                CustomInspectorValidations.DrawHelpBoxMessage(new HelpBoxMessage($"Multiple copies detected! \n\nThere are {foundProjectBuildConfigDuplicatesResults.paths.Length} copies of this instance found in the project, and only '1' instance is allowed per project. " +
                    $"This config, along with '{paths.Length - 1}' additional copies, will not be applied, and the tools option for '{projectBuildConfigGroup.ProjectBuild}' will be disabled. Please remove this or any of the additional copies \n\n Duplicate files: \n\n{duplicatedConfigPaths}", MessageType.Error, IsWideHelpBox));
            }
        }

        private int GetNullReferenceProjectBuildConfigsCount(ProjectBuildConfigGroup projectBuildConfigGroup)
            => projectBuildConfigGroup.ProjectBuildConfigs.Count(x => x == null);
    }
}
