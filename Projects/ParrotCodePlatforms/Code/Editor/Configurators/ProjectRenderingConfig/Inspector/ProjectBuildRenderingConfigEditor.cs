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
#endregion

#region Included Unity Assemblies
using UnityEditor;
using UnityEngine;
#endregion

#region Included Parrot Code Assemblies
using ParrotCode.Native.SharedEditor;
using ParrotCode.Extensions;
#endregion

namespace ParrotCode.Platforms
{
    /// <summary>
    /// Provides a custom Unity Editor inspector for <see cref="ProjectBuildRenderingConfig"/>,
    /// allowing platform-specific rendering settings to be viewed, validated, and applied.
    /// </summary>
    [CustomEditor(typeof(ProjectBuildRenderingConfig))]
    public sealed class ProjectBuildRenderingConfigEditor: Editor
    {
        private readonly string _ProjectConfigurationWarningPopUpTitle = 
            string.Join(" ", 
                SharedCustomEditorStringInfo.ProjectConfigurationPopupTitle, 
                SharedCustomEditorStringInfo.ProjectRenderingSettingsTitle);

        private readonly string _ProjectConfigurationWarningPopUpMessage = 
            string.Format(SharedCustomEditorStringInfo.ProjectConfigurationPopupMessage,
                SharedCustomEditorStringInfo.ProjectRenderingSettingsTitle);

        #region Platform Settings

        private const string AndroidSettingsPropertyFieldName = "androidSettings";
        private const string IOSSettingsPropertyFieldName = "iosSettings";
        private const string WindowsSettingsPropertyFieldName = "windowsSettings";
        private const string WebGLSettingsPropertyFieldName = "webGLSettings";

        /// <summary>
        /// Maps Unity build targets to their corresponding serialized rendering settings.
        /// </summary>
        private readonly Dictionary<BuildTarget, SerializedProperty> _platformSettingsProperties 
            = new Dictionary<BuildTarget, SerializedProperty>();

        #endregion

        private const string RenderingSettingsFieldLabel = "Platform Settings";

        private static readonly string[] excludedProperties = 
        {
            AndroidSettingsPropertyFieldName,
            IOSSettingsPropertyFieldName,
            WindowsSettingsPropertyFieldName,
            WebGLSettingsPropertyFieldName
        };

        private readonly ProjectBuildRenderingConfigValidationManager _validationManager = 
            new ProjectBuildRenderingConfigValidationManager();

        /// <summary>
        /// Initializes the editor state when the inspector is enabled.
        /// </summary>
        private void OnEnable()
            => InitializeProperties();

        /// <summary>
        /// Initializes the serialized properties used to display
        /// platform-specific rendering settings.
        /// </summary>
        private void InitializeProperties()
        {
            _platformSettingsProperties[BuildTarget.Android] = 
                serializedObject.FindProperty(AndroidSettingsPropertyFieldName);

            _platformSettingsProperties[BuildTarget.iOS] = 
                serializedObject.FindProperty(IOSSettingsPropertyFieldName);

            _platformSettingsProperties[BuildTarget.WebGL] =
                serializedObject.FindProperty(WebGLSettingsPropertyFieldName);

            var windowsSettingsPropertyField = 
                serializedObject.FindProperty(WindowsSettingsPropertyFieldName);

            _platformSettingsProperties[BuildTarget.StandaloneWindows] =
                windowsSettingsPropertyField;

            _platformSettingsProperties[BuildTarget.StandaloneWindows64] = 
                windowsSettingsPropertyField;
        }

        /// <summary>
        /// Draws the custom inspector for the rendering configuration.
        /// </summary>
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUI.BeginChangeCheck();

            DrawPropertiesExcluding(serializedObject, excludedProperties);

            ProjectBuildRenderingConfig renderingProjectBuildConfig = (ProjectBuildRenderingConfig)target;

            BuildTarget buildTarget = EditorUserBuildSettings.activeBuildTarget;

            #region Draw Rendering Settings

            DrawRenderingSettings(buildTarget);

            #endregion

            #region Apply Platform Specific Settings

            ApplySettingsValidationScope(renderingProjectBuildConfig,
                GetProjectBuildRenderingValidationResults(renderingProjectBuildConfig));

            #endregion

            serializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        /// Draws the rendering settings for the specified build target.
        /// </summary>
        /// <param name="buildTarget">
        /// The active Unity build target.
        /// </param>
        private void DrawRenderingSettings(BuildTarget buildTarget)
        {
            if (!_platformSettingsProperties.TryGetValue(buildTarget, 
                out SerializedProperty property))
            {
                CustomInspectorValidations.DrawHelpBoxMessage(
                    new HelpBoxMessage($"Rendering settings are currently not supported " +
                    $"in this version of the framework for target build:" +
                    $" {buildTarget}.", MessageType.Warning));

                return;
            }

            EditorGUILayout.PropertyField(property,
                new GUIContent(RenderingSettingsFieldLabel));
        }

        /// Draws the Apply Settings button and handles applying the rendering
        /// configuration after successful validation and user confirmation.
        /// </summary>
        /// <param name="renderingProjectBuildConfig">
        /// The rendering configuration to apply.
        /// </param>
        /// <param name="validation">
        /// The validation result determining whether the configuration
        /// can be applied.
        /// </param>
        private void ApplySettingsValidationScope(ProjectBuildRenderingConfig 
            renderingProjectBuildConfig, 
            HelpBoxMessage validation)
        {
            using (new EditorGUI.DisabledScope(validation.Failed()))
            {
                GUI.backgroundColor = CustomInspectorGUILayout.ApplySettingsButtonBackgroundColor;

                EditorGUILayout.Space();

                if (!GUILayout.Button(SharedCustomEditorStringInfo.ApplySettingsButtonLabel,
                    CustomInspectorGUILayout.ApplySettingsButtonLayoutHeight))
                {
                    return;
                }

                if (SharedCustomInspectorEditorPopup.ShowApplySettingsConfirmationPopup(
                    _ProjectConfigurationWarningPopUpTitle,
                    _ProjectConfigurationWarningPopUpMessage))
                    renderingProjectBuildConfig.ApplySettings();

                EditorGUILayout.Space();
            }
        }

        /// <summary>
        /// Validates the rendering configuration, refreshes cached validation
        /// results when properties change, and displays any validation messages.
        /// </summary>
        /// <param name="renderingProjectBuildConfig">
        /// The rendering configuration to validate.
        /// </param>
        /// <returns>
        /// A <see cref="HelpBoxMessage"/> describing the validation result.
        /// </returns>
        private HelpBoxMessage GetProjectBuildRenderingValidationResults(
            ProjectBuildRenderingConfig renderingProjectBuildConfig)
        {
            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
                _validationManager.InvalidateCache();
            }

            var validationResults = _validationManager.Validate(renderingProjectBuildConfig);

            EditorGUILayout.Space();

            CustomInspectorValidations.DrawHelpBoxMessage(validationResults);

            return validationResults;
        }
    }
}
