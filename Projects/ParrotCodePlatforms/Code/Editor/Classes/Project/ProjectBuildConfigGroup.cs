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

#region System
using System.Collections.Generic;
using System.Linq;
#endregion

#region Unity
using UnityEditor;
using UnityEngine;
#endregion

#region Parrot Code
using ParrotCode.Native.SharedEditor;
using ParrotCode.Extensions;
#endregion

namespace ParrotCode.Platforms
{
    [CreateAssetMenu(fileName = "Project Configuration", menuName = ProjectSharedDirectory.PlatformConfigRootPath + "Project Configuration")]
    public sealed class ProjectBuildConfigGroup: ScriptableObject
    {
        [SerializeField]
        private BuildTarget buildTarget;

        [SerializeField, Space(5)]
        private Build projectBuild;

        [Header("Build Settings")]
        [SerializeField, Space(5)]
        private List<ProjectBuildConfig> projectBuildSettings;

        public BuildTarget BuildTarget => buildTarget;
        public Build ProjectBuild => projectBuild;

        public IReadOnlyList<ProjectBuildConfig> ProjectBuildConfigs => projectBuildSettings;

        private string ProjectConfigurationWarningPopUpTitle = string.Join(" ", CustomEditorSharedInfo.ProjectConfigurationPopUpTitle, CustomEditorSharedInfo.ProjectSettingsTitle);
        private string ProjectConfigurationWarningPopUpMessage = string.Format(CustomEditorSharedInfo.ProjectConfigurationPopUpMessage, CustomEditorSharedInfo.ProjectSettingsTitle);

        public void ApplySettings()
        {
            if (!CustomInspectorEditorPopup.ApplySettingsPopUpConfirmed(ProjectConfigurationWarningPopUpTitle, ProjectConfigurationWarningPopUpMessage))
                return;
        }

        #region Config Validations
        /// <summary>
        /// This function checks validations the ProjectBuildConfigGroup.
        /// </summary>
        /// <returns>A turple (bool isValid, string message, MessageType messageType) 
        /// Valid if this config's validation was successfull, and 
        /// False if the validation failed, along with a failed message 
        /// and message type e.g <see cref="MessageType"/>.</returns>
        public HelpBoxMessage Validate()
        {
            HelpBoxMessage validationResults;

            #region Validate Project Build Config Group Duplicates
            validationResults = ValidateDuplicateProjectBuildConfigGroup();
            if (validationResults.ContainsLog())
                return validationResults;
            #endregion

            #region Validate Null References Project Build Configs
            validationResults = ValidateNullReferencesProjectBuildConfigs();
            if (validationResults.ContainsLog())
                return validationResults;
            #endregion

            #region Validate Duplicate Project Build Configs
            validationResults = ValidateDuplicateProjectBuildConfigs();
            if (validationResults.ContainsLog())
                return validationResults;
            #endregion

            #region Validate Assigned Project Build Configs
            validationResults = ValidateAssignedProjectBuildConfigs();
            if (validationResults.ContainsLog())
                return validationResults;
            #endregion

            #region Validate Misconfigured Project Build Configs
            validationResults = ValidateMisconfiguredProjectBuildConfigs();
            if (validationResults.ContainsLog())
                return validationResults;
            #endregion

            return validationResults;
        }

        #region Config Validations
        private HelpBoxMessage ValidateDuplicateProjectBuildConfigGroup()
        {
            var foundProjectBuildConfigDuplicatesResults = ProjectBuildConfigurator.GetProjectBuildConfigGroupDuplicatePaths(this);

            if (!foundProjectBuildConfigDuplicatesResults.hasCopies)
                return HelpBoxMessage.Empty;

            string[] duplicatedProjectBuildConfigPaths = foundProjectBuildConfigDuplicatesResults.paths.Where(x => x != AssetDatabase.GetAssetPath(this)).ToArray();

            string duplicatedConfigPaths = string.Join("\n", duplicatedProjectBuildConfigPaths);

            string validationErrorMessage = $"Multiple copies detected! \n\nThere are {foundProjectBuildConfigDuplicatesResults.paths.Length} copies of this instance found in the project, and only '1' instance is allowed per project. " +
                   $"This config, along with '{duplicatedProjectBuildConfigPaths.Length - 1}' additional copy/copies, will not be applied, and the tools option for '{ProjectBuild}' will be disabled." +
                   $" Please remove this or any of the below listed files. \n\n Duplicated file(s): \n\n{duplicatedConfigPaths}\n";

            return new HelpBoxMessage(validationErrorMessage, MessageType.Error, CustomInspectorValidations.EnabledWideHelpBox);
        }

        private HelpBoxMessage ValidateDuplicateProjectBuildConfigs()
        {
            var duplicatedProjectBuildConfigGroups = ProjectBuildConfigs.GroupBy(group => group.GetType()).Where(group => group.Skip(1).Any()).ToArray();

            if (duplicatedProjectBuildConfigGroups.Length == 0)
                return HelpBoxMessage.Empty;

            List<string> validationErrorMessages = new List<string>();

            foreach (var duplicatedProjectBuildConfigGroup in duplicatedProjectBuildConfigGroups)
            {
                int duplicatedProjectBuildConfigCount = duplicatedProjectBuildConfigGroup.Count();
                var duplicatedProjectBuildConfigGroupNames = duplicatedProjectBuildConfigGroup.Select(x => $"* {x.name}").ToArray();
                string duplicatedProjectBuildConfigNames = string.Join("\n", duplicatedProjectBuildConfigGroupNames);

                string groupValidationErrorMessage = $"[Duplicated '{duplicatedProjectBuildConfigGroup.Key.Name}' detected] {name}" +
                    $" contains {duplicatedProjectBuildConfigCount} copies of {duplicatedProjectBuildConfigGroup.Key.Name}. \nPlease remove one of the following listed {duplicatedProjectBuildConfigGroup.Key.Name} " +
                    $"from the list: \n\n{duplicatedProjectBuildConfigNames}\n";

                validationErrorMessages.Add(groupValidationErrorMessage);
            }

            string validationErrorMessage = string.Join ("\n", validationErrorMessages);

            return new HelpBoxMessage(validationErrorMessage, MessageType.Error, CustomInspectorValidations.EnabledWideHelpBox);
        }

        private HelpBoxMessage ValidateNullReferencesProjectBuildConfigs()
        {
            int nullReferenceProjectBuildConfigsCount = GetNullReferenceProjectBuildConfigsCount();

            if (nullReferenceProjectBuildConfigsCount == 0)
                return HelpBoxMessage.Empty;

            string validationErrorMessage = $"Found {nullReferenceProjectBuildConfigsCount} 'ProjectBuildConfig' null reference(s) in: {name}.";
            return new HelpBoxMessage(validationErrorMessage, MessageType.Error, CustomInspectorValidations.EnabledWideHelpBox);
        }

        private HelpBoxMessage ValidateAssignedProjectBuildConfigs()
        {
            if(ProjectBuildConfigs != null && ProjectBuildConfigs.Count > 0)
                return HelpBoxMessage.Empty;

            string validationErrorMessage = $"There are no build settings assigned for '{BuildTarget}'. This config group will be ignored by the project build configurator.";
            return new HelpBoxMessage(validationErrorMessage, MessageType.Warning, CustomInspectorValidations.EnabledWideHelpBox);
        }

        private HelpBoxMessage ValidateMisconfiguredProjectBuildConfigs()
        {
            var invalidatedProjectBuildConfigs = ProjectBuildConfigs.OfType<ProjectSpecificBuildConfig>().Where(config => config.Validate().ContainsLog()).ToArray();

            if(invalidatedProjectBuildConfigs.Length == 0)
                return HelpBoxMessage.Empty;

            var defaultInvalidatedProjectBuildConfig = invalidatedProjectBuildConfigs.FirstOrDefault();
            var validationResults = defaultInvalidatedProjectBuildConfig.Validate();

            string validationMessage = $"[{validationResults.MessageType} Log] \n\nFile Name [{defaultInvalidatedProjectBuildConfig.name}]\n";
            string projectBuildConfigFilePath = $"File Path: {AssetDatabase.GetAssetPath(defaultInvalidatedProjectBuildConfig)}\n";
            string validationErrorMessage = string.Join("\n", validationMessage, validationResults.Message, projectBuildConfigFilePath);

            return new HelpBoxMessage(validationErrorMessage, validationResults.MessageType, CustomInspectorValidations.EnabledWideHelpBox);
        }

        #endregion

        private int GetNullReferenceProjectBuildConfigsCount()
            => ProjectBuildConfigs.Count(x => x == null);
        #endregion
    }
}
