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
using System.Collections.Generic;
using System.Linq;
#endregion

#region Included Unity Assemblies
using UnityEditor;
#endregion

#region Included Parrot Code Assemblies
using ParrotCode.Native.SharedEditor;
#endregion

namespace ParrotCode.Platforms
{
    public sealed class ProjectBuildConfigGroupValidator : IEditorAssetValidator
    {
        #region Config Validations
        /// <summary>
        /// This function checks validations the ProjectBuildConfigGroup.
        /// </summary>
        /// <returns>A turple (bool isValid, string message, MessageType messageType) 
        /// Valid if this config's validation was successfull, and 
        /// False if the validation failed, along with a failed message 
        /// and message type e.g <see cref="MessageType"/>.</returns>
        public HelpBoxMessage Validate<T>(T config) where T : class
        {
            ProjectBuildConfigGroup projectBuildConfigGroup = config as ProjectBuildConfigGroup;

            if (projectBuildConfigGroup == null)
                return HelpBoxMessage.Empty;

            HelpBoxMessage validationResults;

            #region Validate Project Build Config Group Duplicates
            validationResults = ValidateDuplicateProjectBuildConfigGroup(projectBuildConfigGroup);
            if (validationResults.ContainsLog())
                return validationResults;
            #endregion

            #region Validate Null References Project Build Configs
            validationResults = ValidateNullReferencesProjectBuildConfigs(projectBuildConfigGroup);
            if (validationResults.ContainsLog())
                return validationResults;
            #endregion

            #region Validate Duplicate Project Build Configs
            validationResults = ValidateDuplicateProjectBuildConfigs(projectBuildConfigGroup);
            if (validationResults.ContainsLog())
                return validationResults;
            #endregion

            #region Validate Assigned Project Build Configs
            validationResults = ValidateAssignedProjectBuildConfigs(projectBuildConfigGroup);
            if (validationResults.ContainsLog())
                return validationResults;
            #endregion

            #region Validate Misconfigured Project Build Configs
            validationResults = ValidateMisconfiguredProjectBuildConfigs(projectBuildConfigGroup);
            if (validationResults.ContainsLog())
                return validationResults;
            #endregion

            return validationResults;
        }

        #region Config Validations
        private HelpBoxMessage ValidateDuplicateProjectBuildConfigGroup(ProjectBuildConfigGroup configurator)
        {
            //var foundProjectBuildConfigDuplicatesResults = ProjectAssetsDatabaseUtility.GetProjectConfigForBuildDuplicatePaths(this);

            //if (foundProjectBuildConfigDuplicatesResults.MessageType == MessageType.None)
            //    return HelpBoxMessage.Empty;

            //string[] duplicatedProjectBuildConfigPaths = foundProjectBuildConfigDuplicatesResults.Value.Where(x => x != AssetDatabase.GetAssetPath(this)).ToArray();

            //int configCount = foundProjectBuildConfigDuplicatesResults.Value.Length;

            //string duplicatedConfigPaths = string.Join("\n", duplicatedProjectBuildConfigPaths);

            //string validationErrorMessage = $"Multiple copies detected! \n\nThere are {configCount} copies of this instance found in the project, and only '1' instance is allowed per project. " +
            //       $"This config, along with '{duplicatedProjectBuildConfigPaths.Length - 1}' additional copy/copies, will not be applied, and the tools option for '{ProjectBuild}' will be disabled." +
            //       $" Please remove this or any of the below listed files. \n\n Duplicated file(s): \n\n{duplicatedConfigPaths}\n";

            //return new HelpBoxMessage(validationErrorMessage, MessageType.Error, CustomInspectorValidations.EnabledWideHelpBox);

            return default;
        }

        private HelpBoxMessage ValidateDuplicateProjectBuildConfigs(ProjectBuildConfigGroup configurator)
        {
            var duplicatedProjectBuildConfigGroups = configurator.ProjectBuildConfigs.GroupBy(group => group.GetType()).Where(group => group.Skip(1).Any()).ToArray();

            if (duplicatedProjectBuildConfigGroups.Length == 0)
                return HelpBoxMessage.Empty;

            List<string> validationErrorMessages = new List<string>();

            foreach (var duplicatedProjectBuildConfigGroup in duplicatedProjectBuildConfigGroups)
            {
                int duplicatedProjectBuildConfigCount = duplicatedProjectBuildConfigGroup.Count();
                var duplicatedProjectBuildConfigGroupNames = duplicatedProjectBuildConfigGroup.Select(x => $"* {x.name}").ToArray();
                string duplicatedProjectBuildConfigNames = string.Join("\n", duplicatedProjectBuildConfigGroupNames);

                string groupValidationErrorMessage = $"[Duplicated '{duplicatedProjectBuildConfigGroup.Key.Name}' detected] {configurator.name}" +
                    $" contains {duplicatedProjectBuildConfigCount} copies of {duplicatedProjectBuildConfigGroup.Key.Name}. \nPlease remove one of the following listed {duplicatedProjectBuildConfigGroup.Key.Name} " +
                    $"from the list: \n\n{duplicatedProjectBuildConfigNames}\n";

                validationErrorMessages.Add(groupValidationErrorMessage);
            }

            string validationErrorMessage = string.Join("\n", validationErrorMessages);

            return new HelpBoxMessage(validationErrorMessage, MessageType.Error, CustomInspectorValidations.EnabledWideHelpBox);
        }

        private HelpBoxMessage ValidateNullReferencesProjectBuildConfigs(ProjectBuildConfigGroup configurator)
        {
            int nullReferenceProjectBuildConfigsCount = GetNullReferenceProjectBuildConfigsCount(configurator);

            if (nullReferenceProjectBuildConfigsCount == 0)
                return HelpBoxMessage.Empty;

            string validationErrorMessage = $"Found {nullReferenceProjectBuildConfigsCount} 'ProjectBuildConfig' null reference(s) in: {configurator.name}.";
            return new HelpBoxMessage(validationErrorMessage, MessageType.Error, CustomInspectorValidations.EnabledWideHelpBox);
        }

        private HelpBoxMessage ValidateAssignedProjectBuildConfigs(ProjectBuildConfigGroup configurator)
        {
            if (configurator.ProjectBuildConfigs != null && configurator.ProjectBuildConfigs.Count > 0)
                return HelpBoxMessage.Empty;

            string validationErrorMessage = $"There are no build settings assigned for '{configurator.BuildTarget}'. This config group will be ignored by the project build configurator.";
            return new HelpBoxMessage(validationErrorMessage, MessageType.Warning, CustomInspectorValidations.EnabledWideHelpBox);
        }

        private HelpBoxMessage ValidateMisconfiguredProjectBuildConfigs(ProjectBuildConfigGroup configurator)
        {
            //var invalidatedProjectBuildConfigs = configurator.ProjectBuildConfigs.OfType<ProjectSpecificBuildConfig>().Where(config => config.Validate().ContainsLog()).ToArray();

            //if (invalidatedProjectBuildConfigs.Length == 0)
            //    return HelpBoxMessage.Empty;

            //var defaultInvalidatedProjectBuildConfig = invalidatedProjectBuildConfigs.FirstOrDefault();
            //var validationResults = defaultInvalidatedProjectBuildConfig.Validate();

            //string validationMessage = $"[{validationResults.MessageType} Log] \n\nFile Name [{defaultInvalidatedProjectBuildConfig.name}]\n";
            //string projectBuildConfigFilePath = $"File Path: {AssetDatabase.GetAssetPath(defaultInvalidatedProjectBuildConfig)}\n";
            //string validationErrorMessage = string.Join("\n", validationMessage, validationResults.Message, projectBuildConfigFilePath);

            //return new HelpBoxMessage(validationErrorMessage, validationResults.MessageType, CustomInspectorValidations.EnabledWideHelpBox);
            return HelpBoxMessage.Empty;
        }

        #endregion

        private int GetNullReferenceProjectBuildConfigsCount(ProjectBuildConfigGroup configurator)
            => configurator.ProjectBuildConfigs.Count(x => x == null);

        #endregion
    }
}
