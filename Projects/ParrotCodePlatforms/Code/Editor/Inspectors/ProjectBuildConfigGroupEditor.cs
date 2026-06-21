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
using UnityEngine;

namespace ParrotCode.Platforms
{
    [CustomEditor(typeof(ProjectBuildConfigGroup))]
    public sealed class ProjectBuildConfigGroupEditor: Editor
    {
        private string ProjectConfigurationWarningPopUpTitle = string.Join(" ", CustomEditorSharedInfo.ProjectConfigurationPopUpTitle, CustomEditorSharedInfo.ProjectSettingsTitle);
        private string ProjectConfigurationWarningPopUpMessage = string.Format(CustomEditorSharedInfo.ProjectConfigurationPopUpMessage, CustomEditorSharedInfo.ProjectSettingsTitle);

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawDefaultInspector();

            ProjectBuildConfigGroup buildConfigGroup = (ProjectBuildConfigGroup)target;

            GUI.enabled = Validate(buildConfigGroup);

            if(GUILayout.Button(CustomInspectorGUILayout.ApplySettingsButtonLabel, CustomInspectorGUILayout.ApplySettingsButtonLayoutHeight))
            {
                if(!CustomInspectorEditorPopUp.ApplySettingsPopUpConfirmed(ProjectConfigurationWarningPopUpTitle, ProjectConfigurationWarningPopUpMessage))
                {
                    return;
                }

                buildConfigGroup.ApplySettings();
            }

            serializedObject.ApplyModifiedProperties();
        }

        private bool Validate(ProjectBuildConfigGroup projectBuildConfigGroup)
        {

            if(CustomInspectorValidations.OnValidationFailed(projectBuildConfigGroup,
                ValidateAssignedProjectBuildConfigs, 
                new HelpBoxMessage($"There are no build settings assigned for '{projectBuildConfigGroup.BuildTarget}'. This config group might be ignored.", MessageType.Warning, CustomInspectorValidations.EnabledWideHelpBox)))
            {
                return false;
            }

            int nullReferenceProjectBuildConfigsCount = GetNullReferenceProjectBuildConfigsCount(projectBuildConfigGroup);

            if (nullReferenceProjectBuildConfigsCount > 0)
            {
                CustomInspectorValidations.DrawHelpBoxMessage(new HelpBoxMessage($"Found {nullReferenceProjectBuildConfigsCount} 'ProjectBuildConfig' null reference(s) in: {projectBuildConfigGroup.name}.", MessageType.Error, CustomInspectorValidations.EnabledWideHelpBox));
                return false;
            }

            if(!ValidateDuplicateProjectBuildConfigGroup(projectBuildConfigGroup))
            {
                return false;
            }

            ValidateMisconfiguredProjectBuildConfigs(projectBuildConfigGroup);
            
            if(!ValidateDuplicateProjectBuildConfigs(projectBuildConfigGroup))
            {
                return false; 
            }

            return true;
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
                    $" '{misconfiguredConfig.name}' with an incorrect target build type: '{misconfiguredConfig.BuildTarget}'.", MessageType.Error, CustomInspectorValidations.EnabledWideHelpBox));
            }
        }

        private bool ValidateDuplicateProjectBuildConfigGroup(ProjectBuildConfigGroup projectBuildConfigGroup)
        {
            var foundProjectBuildConfigDuplicatesResults = ProjectBuildConfigurator.GetProjectBuildConfigGroupDuplicatesCount(projectBuildConfigGroup);

            if (!foundProjectBuildConfigDuplicatesResults.hasCopies)
            {
                return true;
            }

            string[] paths = foundProjectBuildConfigDuplicatesResults.paths;

            string duplicatedConfigPaths = string.Join("\n", paths);

            CustomInspectorValidations.DrawHelpBoxMessage(new HelpBoxMessage($"Multiple copies detected! \n\nThere are {foundProjectBuildConfigDuplicatesResults.paths.Length} copies of this instance found in the project, and only '1' instance is allowed per project. " +
                $"This config, along with '{paths.Length - 1}' additional copies, will not be applied, and the tools option for '{projectBuildConfigGroup.ProjectBuild}' will be disabled. Please remove this or any of the additional copies \n\n Duplicate files: \n\n{duplicatedConfigPaths}", MessageType.Error, CustomInspectorValidations.EnabledWideHelpBox));

            return false;
        }

        private bool ValidateDuplicateProjectBuildConfigs(ProjectBuildConfigGroup projectBuildConfigGroup)
        {
            var duplicatedProjectBuildConfigGroups = projectBuildConfigGroup.ProjectBuildConfigs.GroupBy(group => group.GetType()).Where(group => group.Skip(1).Any()).ToArray();

            if(duplicatedProjectBuildConfigGroups.Length == 0)
            {
                return true;
            }

            string[] duplicatedProjectBuildConfigGroupNames = duplicatedProjectBuildConfigGroups.Select(duplicatedProjectBuildConfigGroup => $"* {duplicatedProjectBuildConfigGroup.Key} [{duplicatedProjectBuildConfigGroup.Count()}]").ToArray();
            string duplicatedProjectBuildConfigNames = string.Join("\n", duplicatedProjectBuildConfigGroupNames);

            CustomInspectorValidations.DrawHelpBoxMessage(new HelpBoxMessage($"[Duplicate ProjectBuildConfig(s) detected] {name} contains duplicated project build config(s). \nPlease remove the following duplicated project build config(s) from the list: \n\n{duplicatedProjectBuildConfigNames}\n", MessageType.Error, CustomInspectorValidations.EnabledWideHelpBox));

            return false;
        }

        private int GetNullReferenceProjectBuildConfigsCount(ProjectBuildConfigGroup projectBuildConfigGroup)
            => projectBuildConfigGroup.ProjectBuildConfigs.Count(x => x == null);
    }
}
