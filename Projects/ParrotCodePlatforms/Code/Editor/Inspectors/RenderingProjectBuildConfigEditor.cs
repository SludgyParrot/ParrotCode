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

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using ParrotCode.Native.SharedEditor;

namespace ParrotCode.Platforms
{
    /// <summary>
    /// This is a custom editor class for <see cref="RenderingProjectBuildConfig"/>
    /// </summary>
    [CustomEditor(typeof(RenderingProjectBuildConfig))]
    public sealed class RenderingProjectBuildConfigEditor: Editor
    {
        private string ProjectConfigurationWarningPopUpTitle = string.Join(" ", CustomEditorSharedInfo.ProjectConfigurationPopUpTitle, CustomEditorSharedInfo.ProjectRenderingSettingsTitle);
        private string ProjectConfigurationWarningPopUpMessage = string.Format(CustomEditorSharedInfo.ProjectConfigurationPopUpMessage, CustomEditorSharedInfo.ProjectRenderingSettingsTitle);

        #region Platform Settings

        private const string AndroidSettingsPropertyFieldName = "androidSettings";
        private const string IOSSettingsPropertyFieldName = "iosSettings";
        private const string WindowsSettingsPropertyFieldName = "windowsSettings";
        private const string WebGLSettingsPropertyFieldName = "webGLSettings";

        private readonly Dictionary<BuildTarget, SerializedProperty> platformSettingsProperties 
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

        private void OnEnable()
        {
            InitializeProperties();
        }

        private void InitializeProperties()
        {
            platformSettingsProperties[BuildTarget.Android] = serializedObject.FindProperty(AndroidSettingsPropertyFieldName);
            platformSettingsProperties[BuildTarget.iOS] = serializedObject.FindProperty(IOSSettingsPropertyFieldName);
            platformSettingsProperties[BuildTarget.WebGL] = serializedObject.FindProperty(WebGLSettingsPropertyFieldName);

            var windowsSettingsPropertyField = serializedObject.FindProperty(WindowsSettingsPropertyFieldName);

            platformSettingsProperties[BuildTarget.StandaloneWindows] = windowsSettingsPropertyField;
            platformSettingsProperties[BuildTarget.StandaloneWindows64] = windowsSettingsPropertyField;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawPropertiesExcluding(serializedObject, excludedProperties);

            RenderingProjectBuildConfig renderingProjectBuildConfig = (RenderingProjectBuildConfig)target;

            #region Settings

            BuildTarget buildTarget = EditorUserBuildSettings.activeBuildTarget;

            if (!platformSettingsProperties.TryGetValue(buildTarget, out SerializedProperty property))
            {
                CustomInspectorValidations.DrawHelpBoxMessage(new HelpBoxMessage($"Rendering settings are currently not supported in this version of the framework for target build:" +
                    $" {buildTarget}.", MessageType.Warning, CustomInspectorValidations.EnabledWideHelpBox));
                return;
            }

            #endregion

            #region Platform Specific Settings
            OnPlatformSpecificConfigInspectorGUI(renderingProjectBuildConfig, property, buildTarget);
            #endregion

            serializedObject.ApplyModifiedProperties();
        }

        private void OnPlatformSpecificConfigInspectorGUI(RenderingProjectBuildConfig renderingProjectBuildConfig, SerializedProperty property, BuildTarget buildTarget)
        {
            if (property.boxedValue is IRenderingProjectBuildConfig settings)
            {
                var validationResults = renderingProjectBuildConfig.Validate(settings);

                if (!validationResults.Validated)
                {
                    EditorGUILayout.Space();
                    CustomInspectorValidations.DrawHelpBoxMessage(new HelpBoxMessage(validationResults));
                }

               EditorGUILayout.PropertyField(property, new GUIContent(RenderingSettingsFieldLabel));

                GUI.enabled = renderingProjectBuildConfig.GraphicsAPI.Count == 0 ? true: validationResults.Validated;

                EditorGUILayout.Space();
                OnApplyRenderingSettingsInspectorGUI(renderingProjectBuildConfig);
            }

            EditorGUILayout.Space();
        }

        private void OnApplyRenderingSettingsInspectorGUI(RenderingProjectBuildConfig renderingProjectBuildConfig)
        {
            if (GUILayout.Button(CustomEditorSharedInfo.ApplySettingsButtonLabel, CustomInspectorGUILayout.ApplySettingsButtonLayoutHeight))
            {
                if (CustomInspectorEditorPopUp.ApplySettingsPopUpConfirmed(ProjectConfigurationWarningPopUpTitle, ProjectConfigurationWarningPopUpMessage))
                    renderingProjectBuildConfig.ApplySettings();
            }
        }
    }
}
