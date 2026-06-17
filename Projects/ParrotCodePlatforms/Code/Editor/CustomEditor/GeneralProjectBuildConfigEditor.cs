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
    [CustomEditor(typeof(GeneralProjectBuildConfig))]
    public sealed class GeneralProjectBuildConfigEditor: Editor
    {
        private SerializedProperty scriptingBackendSerializedProperty;

        private const string ScriptingBackendSerializedPropertyField = "scriptingBackend";
        private const string Il2CppCodeGenerationSerializedPropertyField = "il2CppCodeGeneration";

        private const string Il2CppCodeGenerationSerializedPropertyFieldName = "Il2CppCodeGeneration";

        private readonly Dictionary<ScriptingImplementation, (SerializedProperty property, string fieldName)> scriptingBackendSettingsPropertyFields
            = new Dictionary<ScriptingImplementation, (SerializedProperty property, string fieldName)>();       

        private readonly string[] excludedSerializedProperties =
        {
           Il2CppCodeGenerationSerializedPropertyField
        };

        private void OnEnable()
        {
            InitializeSerializedProperties();
        }

        private void InitializeSerializedProperties()
        {
            scriptingBackendSerializedProperty = serializedObject.FindProperty(ScriptingBackendSerializedPropertyField);

            scriptingBackendSettingsPropertyFields[ScriptingImplementation.IL2CPP] = (serializedObject.FindProperty(Il2CppCodeGenerationSerializedPropertyField), Il2CppCodeGenerationSerializedPropertyFieldName);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawPropertiesExcluding(serializedObject, excludedSerializedProperties);

            ScriptingImplementation scriptingImplementation = (ScriptingImplementation)scriptingBackendSerializedProperty.intValue;

            if(scriptingBackendSettingsPropertyFields.TryGetValue(scriptingImplementation, out (SerializedProperty property, string fieldName) propertyInfo))
            {
                EditorGUILayout.PropertyField(propertyInfo.property, new GUIContent(propertyInfo.fieldName));
            }


            serializedObject.ApplyModifiedProperties();
        }
    }
}
