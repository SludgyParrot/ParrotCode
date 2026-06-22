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

namespace ParrotCode.Platforms
{
    [CustomEditor(typeof(ProjectBuildConfigGroup))]
    public sealed class ProjectBuildConfigGroupEditor: Editor
    {
        private string ProjectConfigurationWarningPopUpTitle = string.Join(" ", CustomEditorSharedInfo.ProjectConfigurationPopUpTitle, CustomEditorSharedInfo.ProjectSettingsTitle);
        private string ProjectConfigurationWarningPopUpMessage = string.Format(CustomEditorSharedInfo.ProjectConfigurationPopUpMessage, CustomEditorSharedInfo.ProjectSettingsTitle);

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawDefaultInspector();

            ProjectBuildConfigGroup buildConfigGroup = (ProjectBuildConfigGroup)target;

            GUI.enabled = Validate(buildConfigGroup);

            if(GUILayout.Button(CustomInspectorGUILayout.ApplySettingsButtonLabel, CustomInspectorGUILayout.ApplySettingsButtonLayoutHeight))
            {
                if(!CustomInspectorEditorPopUp.ApplySettingsPopUpConfirmed(ProjectConfigurationWarningPopUpTitle, ProjectConfigurationWarningPopUpMessage))
                {
                    return;
                }

                buildConfigGroup.ApplySettings();
            }

            serializedObject.ApplyModifiedProperties();
        }

        public bool Validate(ProjectBuildConfigGroup buildConfigGroup)
        {
            var validationResults = buildConfigGroup.Validate();

            if(validationResults.isValid)
            {
                return true;
            }

            CustomInspectorValidations.DrawHelpBoxMessage(new HelpBoxMessage(validationResults.message, validationResults.messageType, CustomInspectorValidations.EnabledWideHelpBox));

            return false;
        }
    }
}
