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

using ParrotCode.Native.SharedEditor;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

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

        public void ApplySettings()
        {

        }

        #region Config Validations
        /// <summary>
        /// This function checks validations the ProjectBuildConfigGroup.
        /// </summary>
        /// <returns>A turple (bool isValid, string message, MessageType messageType) 
        /// Valid if this config's validation was successfull, and 
        /// False if the validation failed, along with a failed message 
        /// and message type e.g <see cref="MessageType"/>.</returns>
        public InspectorValidationResults Validate()
        {
            #region Validate Project Build Config Group Duplicates
            if (!ValidateDuplicateProjectBuildConfigGroup().Validated)
                return ValidateDuplicateProjectBuildConfigGroup();
            #endregion

            #region Validate Assigned Project Build Configs
            if (!ValidateAssignedProjectBuildConfigs().Validated)
                return ValidateAssignedProjectBuildConfigs();
            #endregion

            #region Validate Null References Project Build Configs
            if (!ValidateNullReferencesProjectBuildConfigs().Validated)
                return ValidateNullReferencesProjectBuildConfigs();
            #endregion

            #region Validate Misconfigured Project Build Configs
            if (!ValidateMisconfiguredProjectBuildConfigs().Validated)
                return ValidateMisconfiguredProjectBuildConfigs();
            #endregion

            #region Validate Duplicate Project Build Configs
            if (!ValidateDuplicateProjectBuildConfigs().Validated)
                return ValidateDuplicateProjectBuildConfigs();
            #endregion

            return new InspectorValidationResults(true, string.Empty, MessageType.None);
        }

        #region Config Validations
        private InspectorValidationResults ValidateDuplicateProjectBuildConfigGroup()
        {
            var foundProjectBuildConfigDuplicatesResults = ProjectBuildConfigurator.GetProjectBuildConfigGroupDuplicatePaths(this);

            if (!foundProjectBuildConfigDuplicatesResults.hasCopies)
            {
                return new InspectorValidationResults(true, string.Empty, MessageType.None);
            }

            string[] duplicatedProjectBuildConfigPaths = foundProjectBuildConfigDuplicatesResults.paths.Where(x => x != AssetDatabase.GetAssetPath(this)).ToArray();

            string duplicatedConfigPaths = string.Join("\n", duplicatedProjectBuildConfigPaths);

            string validationErrorMessage = $"Multiple copies detected! \n\nThere are {foundProjectBuildConfigDuplicatesResults.paths.Length} copies of this instance found in the project, and only '1' instance is allowed per project. " +
                   $"This config, along with '{duplicatedProjectBuildConfigPaths.Length - 1}' additional copy/copies, will not be applied, and the tools option for '{ProjectBuild}' will be disabled." +
                   $" Please remove this or any of the below listed files. \n\n Duplicated file(s): \n\n{duplicatedConfigPaths}\n";

            return new InspectorValidationResults(false, validationErrorMessage, MessageType.Error);
        }

        private InspectorValidationResults ValidateDuplicateProjectBuildConfigs()
        {
            var duplicatedProjectBuildConfigGroups = ProjectBuildConfigs.GroupBy(group => group.GetType()).Where(group => group.Skip(1).Any()).ToArray();

            if (duplicatedProjectBuildConfigGroups.Length == 0)
            {
                return new InspectorValidationResults(true, string.Empty, MessageType.None);
            }

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

            return new InspectorValidationResults(false, validationErrorMessage, MessageType.Error);
        }

        private InspectorValidationResults ValidateNullReferencesProjectBuildConfigs()
        {
            int nullReferenceProjectBuildConfigsCount = GetNullReferenceProjectBuildConfigsCount();

            if (nullReferenceProjectBuildConfigsCount == 0)
            {
                return new InspectorValidationResults(true, string.Empty, MessageType.None);
            }

            string validationErrorMessage = $"Found {nullReferenceProjectBuildConfigsCount} 'ProjectBuildConfig' null reference(s) in: {name}.";
            return new InspectorValidationResults(false, validationErrorMessage, MessageType.Error);
        }

        private InspectorValidationResults ValidateAssignedProjectBuildConfigs()
        {
            if(ProjectBuildConfigs != null && ProjectBuildConfigs.Count > 0)
            {
                return new InspectorValidationResults(true, string.Empty, MessageType.None);
            }

            string validationErrorMessage = $"There are no build settings assigned for '{BuildTarget}'. This config group will be ignored by the project build configurator.";
            return new InspectorValidationResults(false, validationErrorMessage, MessageType.Warning);
        }

        private InspectorValidationResults ValidateMisconfiguredProjectBuildConfigs()
        {
            var misconfiguredProjectBuildSettings = ProjectBuildConfigs.OfType<ProjectSpecificBuildConfig>();

            foreach (ProjectSpecificBuildConfig misconfiguredConfig in misconfiguredProjectBuildSettings)
            {
                if (misconfiguredConfig.BuildTarget == BuildTarget)
                    continue;
            }

            //CustomInspectorValidations.DrawHelpBoxMessage(new HelpBoxMessage($"Build target '{BuildTarget}' contains a project build config file named" +
            //   $" '{misconfiguredConfig.name}' with an incorrect target build type: '{misconfiguredConfig.BuildTarget}'.", MessageType.Error, CustomInspectorValidations.EnabledWideHelpBox));

            return new InspectorValidationResults(true, string.Empty, MessageType.None);
        }
        #endregion

        private int GetNullReferenceProjectBuildConfigsCount()
            => ProjectBuildConfigs.Count(x => x == null);
        #endregion
    }
}
