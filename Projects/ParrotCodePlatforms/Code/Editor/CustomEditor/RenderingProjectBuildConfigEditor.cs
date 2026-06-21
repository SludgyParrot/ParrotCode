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
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace ParrotCode.Platforms
{
    /// <summary>
    /// This is a custom editor class for <see cref="RenderingProjectBuildConfig"/>
    /// </summary>
    [CustomEditor(typeof(RenderingProjectBuildConfig))]
    public sealed class RenderingProjectBuildConfigEditor: Editor
    {
        private const bool IsWideHelpBox = true;

        private const string ApplySettingsButtonLabel = "Apply Settings";
        private const float ApplySettingsButtonLayoutHeight = 50.0f;

        private  string ProjectConfigurationWarningPopUpTitle = $"Parrot Code: Configure Project Rendering Settings";

        private static string ProjectConfigurationWarningPopUpMessage = $"This operation will configure the Unity platform specific project's rendering settings. " +
            "This action will override existing settings and this action may not be undone. Do you wish to proceed?";

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

            #region General Settings

            #endregion

            #region Platform Specific Settings

            BuildTarget buildTarget = EditorUserBuildSettings.activeBuildTarget;

            if (!platformSettingsProperties.TryGetValue(buildTarget, out SerializedProperty property))
            {
                CustomInspectorValidations.DrawHelpBoxMessage(new HelpBoxMessage($"Rendering settings are currently not supported in this version of the framework for target build: {buildTarget}.", MessageType.Warning, IsWideHelpBox));
                return;
            }
                
            if (property.boxedValue is IRenderingProjectBuildConfig settings)
            {
                ValidatePlatformConfigurations(renderingProjectBuildConfig, settings, buildTarget);
                EditorGUILayout.PropertyField(property, new GUIContent(RenderingSettingsFieldLabel));
            }

            EditorGUILayout.Space();

            #endregion

            #region Apply Settings
            if (GUILayout.Button(ApplySettingsButtonLabel, GUILayout.Height(ApplySettingsButtonLayoutHeight)))
            {
                if(CustomInspectorEditorPopUp.ApplySettingsPopUpConfirmed(ProjectConfigurationWarningPopUpTitle, ProjectConfigurationWarningPopUpMessage))
                    renderingProjectBuildConfig.ApplySettings();
            }
            #endregion

            serializedObject.ApplyModifiedProperties();
        }

        private void ValidatePlatformConfigurations(RenderingProjectBuildConfig projectBuildConfig, IRenderingProjectBuildConfig renderingProjectBuild, BuildTarget buildTarget)
        {
            IReadOnlyList<GraphicsDeviceType> selectedGraphicsAPI = projectBuildConfig.GraphicsAPI;

            if (selectedGraphicsAPI == null || selectedGraphicsAPI.Count == 0)
            {
                EditorGUILayout.Space();

                string supportedGraphicsAPIs = string.Join("\n", renderingProjectBuild.SupportedGraphicsAPI.Select(graphicsAPI => $"* {graphicsAPI.ToString()}"));
                CustomInspectorValidations.DrawHelpBoxMessage(new HelpBoxMessage($"There are no graphics APIs defined for build target: {buildTarget}." +
                    $" \nUnity will automatically select one of the following supported graphics API(s) for {buildTarget}: \n\n{supportedGraphicsAPIs}\n", MessageType.Info, IsWideHelpBox));
                return;
            }

            #region Validate Unsupported Graphic APIs
            if(!ValidateUnsupportedGraphicsAPIs(projectBuildConfig.GraphicsAPI, renderingProjectBuild, buildTarget))
            {
                return;
            }
            #endregion

            #region Validate Deprecated Graphic APIs
            ValidateDeprecatedGraphicsAPIs(projectBuildConfig.GraphicsAPI, renderingProjectBuild, buildTarget);
            #endregion
        }

        private bool ValidateUnsupportedGraphicsAPIs(IReadOnlyList<GraphicsDeviceType> selectedGraphicsAPIs, IRenderingProjectBuildConfig renderingProjectBuild, BuildTarget buildTarget)
        {
            GraphicsDeviceType[] unsupportedGraphicsAPIs = (renderingProjectBuild.UnsupportedGraphicsAPIFound(selectedGraphicsAPIs)).ToArray();

            if (unsupportedGraphicsAPIs.Length == 0)
                return true;

            EditorGUILayout.Space();

            string unsupportedGraphicsAPINames = string.Join("\n", unsupportedGraphicsAPIs.Select(graphicsAPI => $"* {graphicsAPI.ToString()}"));

            CustomInspectorValidations.DrawHelpBoxMessage(new HelpBoxMessage($"[Unsupported graphics API detected] This configuration contains unsupported graphic API(s) for build target:" +
                $" {buildTarget}. \nPlease remove the following unsupported graphics API(s) from the graphics API list: \n\n{unsupportedGraphicsAPINames}\n", MessageType.Error, IsWideHelpBox));

            return false;
        }

        private void ValidateDeprecatedGraphicsAPIs(IReadOnlyList<GraphicsDeviceType> selectedGraphicsAPIs, IRenderingProjectBuildConfig renderingProjectBuild, BuildTarget buildTarget)
        {
            IReadOnlyList<GraphicsDeviceType> deprecatedGraphicsAPIs = renderingProjectBuild.DeprecatedGraphicsAPIFound(selectedGraphicsAPIs);

            if (deprecatedGraphicsAPIs.Count == 0)
            {
                return;
            }

            EditorGUILayout.Space();

            string deprecatedGraphicsAPINames = string.Join("\n", deprecatedGraphicsAPIs.Select(graphicsAPI => $"* {graphicsAPI.ToString()}"));
            CustomInspectorValidations.DrawHelpBoxMessage(new HelpBoxMessage($"[Deprecated graphics API detected] This configuration contains a deprecated graphic API(s) for build target: " +
                $"{buildTarget}. \nPlease remove the following graphics API(s) from the graphics API list: \n\n{deprecatedGraphicsAPINames}\n", MessageType.Warning, IsWideHelpBox));
        }
    }
}
