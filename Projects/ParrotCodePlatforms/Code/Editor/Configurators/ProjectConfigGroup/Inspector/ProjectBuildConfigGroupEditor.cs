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

#region Included Unity Assemblies
using UnityEditor;
using UnityEngine;
#endregion

#region Included Parrot Code Assemblies
using ParrotCode.Native.SharedEditor;
#endregion

namespace ParrotCode.Platforms
{
    /// <summary>
    /// Custom inspector for <see cref="ProjectBuildConfigGroup"/> that integrates
    /// validation feedback and provides an "Apply Settings" action.
    /// </summary>
    /// <remarks>
    /// This editor uses <see cref="ProjectBuildConfigGroupValidationManager"/> to validate
    /// configuration changes in real time and display validation results inside the inspector.
    /// Cached validation results are invalidated whenever serialized properties change.
    /// </remarks>
    [CustomEditor(typeof(ProjectBuildConfigGroup))]
    public sealed class ProjectBuildConfigGroupEditor : Editor
    {
        /// <summary>
        /// Manages validation and caching of <see cref="ProjectBuildConfigGroup"/> editor data.
        /// </summary>
        private readonly ProjectBuildConfigGroupValidationManager _validationsManager =
            new ProjectBuildConfigGroupValidationManager();

        /// <summary>
        /// Overrides the default inspector GUI to include validation feedback and custom actions.
        /// </summary>
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUI.BeginChangeCheck();

            DrawDefaultInspector();

            ProjectBuildConfigGroup buildConfigGroup =
                (ProjectBuildConfigGroup)target;

            if (EditorGUI.EndChangeCheck())
            {
                _validationsManager.InvalidateCache();
            }

            var validationResults = _validationsManager.Validate(buildConfigGroup);

            CustomInspectorValidations.DrawHelpBoxMessage(validationResults);

            using (new EditorGUI.DisabledScope(validationResults.Failed()))
            {
                GUI.backgroundColor =
                    CustomInspectorGUILayout.ApplySettingsButtonBackgroundColor;

                if (GUILayout.Button(
                    SharedCustomEditorStringInfo.ApplySettingsButtonLabel,
                    CustomInspectorGUILayout.ApplySettingsButtonLayoutHeight))
                {
                    buildConfigGroup.ApplySettings();
                }
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void OnDisable()
        {
            _validationsManager.Dispose();
        }
    }
}
