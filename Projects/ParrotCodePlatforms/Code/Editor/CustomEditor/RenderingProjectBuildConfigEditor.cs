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
using UnityEngine;
using UnityEditor;

namespace ParrotCode.Platforms
{
    [CustomEditor(typeof(RenderingProjectBuildConfig))]
    public sealed class RenderingProjectBuildConfigEditor: Editor
    {
        private const bool IsWideHelpBox = true;
        private const string BuildTargetPropertyFieldName = "buildTarget";

        #region Platform Settings

        private const string AndroidSettingsPropertyFieldName = "androidSettings";
        private const string IOSSettingsPropertyFieldName = "iosSettings";
        private const string WindowsSettingsPropertyFieldName = "windowsSettings";
        private const string WebGLSettingsPropertyFieldName = "webGLSettings";

        private readonly Dictionary<BuildTarget, SerializedProperty> platformSettingsPropertyFields 
            = new Dictionary<BuildTarget, SerializedProperty>();

        #endregion

        private const string RenderingSettingsPropertyFieldName = "Rendering Settings";

        private string[] excludedProperties = 
        {
            AndroidSettingsPropertyFieldName,
            IOSSettingsPropertyFieldName,
            WindowsSettingsPropertyFieldName,
            WebGLSettingsPropertyFieldName
        };

        private void OnEnable()
        {
            InitializeProperies();
        }

        private void InitializeProperies()
        {
            platformSettingsPropertyFields[BuildTarget.Android] = serializedObject.FindProperty(AndroidSettingsPropertyFieldName);
            platformSettingsPropertyFields[BuildTarget.iOS] = serializedObject.FindProperty(IOSSettingsPropertyFieldName);
            platformSettingsPropertyFields[BuildTarget.WebGL] = serializedObject.FindProperty(WebGLSettingsPropertyFieldName);

            var windowsSettingsPropertyField = serializedObject.FindProperty(WindowsSettingsPropertyFieldName);

            platformSettingsPropertyFields[BuildTarget.StandaloneWindows] = windowsSettingsPropertyField;
            platformSettingsPropertyFields[BuildTarget.StandaloneWindows64] = windowsSettingsPropertyField;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawPropertiesExcluding(serializedObject, excludedProperties);

            BuildTarget target = EditorUserBuildSettings.activeBuildTarget;

            if (platformSettingsPropertyFields.TryGetValue(target, out SerializedProperty property))
            {
                EditorGUILayout.PropertyField(property, new GUIContent(RenderingSettingsPropertyFieldName));

                if(target == BuildTarget.Android)
                {
                    AndroidRenderingProjectBuildConfig androidSettings = (AndroidRenderingProjectBuildConfig)property.boxedValue;

                    var validateConfigsResults = androidSettings.UnsupportedAPICheckResults();

                    if(validateConfigsResults.hasUnsupportedAPI)
                    {
                        foreach (var api in validateConfigsResults.graphicAPIs)
                        {
                            if (api == UnityEngine.Rendering.GraphicsDeviceType.Vulkan || api == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3)
                                continue;

                            if(api == UnityEngine.Rendering.GraphicsDeviceType.OpenGLES2)
                                CustomInspectorValidations.DrawHelpBoxMessage(new HelpBoxMessage($"Graphics API: {api} is for target build: {target} is deprecated. Please use Andoid supported APIs like (Vulkan, OpenGLES2 etc).", MessageType.Warning, IsWideHelpBox));
                            else
                                CustomInspectorValidations.DrawHelpBoxMessage(new HelpBoxMessage($"Graphics API: {api} is not supported for build target build: {target}.", MessageType.Error, IsWideHelpBox));
                        }
                    }
                }
            }
            else
                CustomInspectorValidations.DrawHelpBoxMessage(new HelpBoxMessage($"Rendering settings are currently not supported in this version of the framework for target build: {target}.", MessageType.Warning, IsWideHelpBox));

            serializedObject.ApplyModifiedProperties();
        }
    }
}
