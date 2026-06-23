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

using UnityEditor;
using UnityEngine;
using ParrotCode.Native.SharedEditor;
using ParrotCode.Extensions;

namespace ParrotCode.Platforms
{
    [CustomEditor(typeof(ProjectBuildConfigGroup))]
    public sealed class ProjectBuildConfigGroupEditor: Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawDefaultInspector();

            ProjectBuildConfigGroup buildConfigGroup = (ProjectBuildConfigGroup)target;

            var validationResults = Validate(buildConfigGroup);

            CustomInspectorValidations.DrawHelpBoxMessage(validationResults);

            using (new EditorGUI.DisabledScope(validationResults.Failed()))
            {
                GUI.backgroundColor = CustomInspectorGUILayout.ApplySettingsButtonBackgroundColor;

                if (GUILayout.Button(CustomEditorSharedInfo.ApplySettingsButtonLabel, CustomInspectorGUILayout.ApplySettingsButtonLayoutHeight))
                    buildConfigGroup.ApplySettings();
            }

            serializedObject.ApplyModifiedProperties();
        }

        private HelpBoxMessage Validate(ProjectBuildConfigGroup buildConfigGroup)
        {
            var validationResults = buildConfigGroup.Validate();
            return validationResults;
        }
    }
}
