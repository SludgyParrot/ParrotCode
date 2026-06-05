using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using ParrotCode.UI;

namespace ParrotCode.UIEditor
{
    [CustomEditor(typeof(Selectable), editorForChildClasses: true)]
    public class SelectableEditor: Editor
    {
        private SerializedProperty navigation;
        private SerializedProperty onSelectionUp;
        private SerializedProperty onSelectionDown;
        private SerializedProperty onSelectionLeft;
        private SerializedProperty onSelectionRight;

        private GUIContent navigationLabel = new GUIContent("Navigation");
        private GUIContent onSelectionUpLabel = new GUIContent("On Selection Up");
        private GUIContent onSelectionDownLabel = new GUIContent("On Selection Down");
        private GUIContent onSelectionLeftLabel = new GUIContent("On Selection Left");
        private GUIContent onSelectionRightLabel = new GUIContent("On Selection Right");

        private List<string> excludedProperties = new List<string>
        {
            "navigation",
            "onSelectionUp",
            "onSelectionDown",
            "onSelectionLeft",
            "onSelectionRight"
        };

        private void OnEnable()
        {
            navigation = serializedObject.FindProperty("navigation");
            onSelectionUp = serializedObject.FindProperty("onSelectionUp");
            onSelectionDown = serializedObject.FindProperty("onSelectionDown");
            onSelectionLeft = serializedObject.FindProperty("onSelectionLeft");
            onSelectionRight = serializedObject.FindProperty("onSelectionRight");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawPropertiesExcluding(serializedObject, excludedProperties.ToArray());

            EditorGUILayout.PropertyField(navigation, navigationLabel);

            Navigation navigationType = (Navigation)navigation.enumValueIndex;

            if(navigationType == Navigation.Explicit)
            {
                EditorGUILayout.Space(10);
                EditorGUILayout.LabelField("Navigation Targets", EditorStyles.boldLabel);
                EditorGUILayout.Space(5);

                EditorGUILayout.PropertyField(onSelectionUp, onSelectionUpLabel);
                EditorGUILayout.Space(5);

                EditorGUILayout.PropertyField(onSelectionDown, onSelectionDownLabel);
                EditorGUILayout.Space(5);

                EditorGUILayout.PropertyField(onSelectionLeft, onSelectionLeftLabel);
                EditorGUILayout.Space(5);

                EditorGUILayout.PropertyField(onSelectionRight, onSelectionRightLabel);
                EditorGUILayout.Space(5);
            }
          
            serializedObject.ApplyModifiedProperties();
        }
    }
}
