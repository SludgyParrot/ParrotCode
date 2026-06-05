using UnityEngine;
using UnityEditor;
using ParrotCode.UI;

namespace ParrotCode.UIEditor
{
    [CustomEditor(typeof(UIView))]
    public class UIViewEditor: Editor
    {
        private SerializedProperty navigation;
        private SerializedProperty selectables;

        private void OnEnable()
        {
            navigation = serializedObject.FindProperty("Navigation");
            selectables = serializedObject.FindProperty("selectables");
        }

        //public override void OnInspectorGUI()
        //{
        //    serializedObject.Update();

        //    EditorGUILayout.PropertyField(navigation, new GUIContent("Navigation"));

        //    //UINavigationType navigationType = (UINavigationType)navigation.enumValueIndex;

        //    //if (navigationType != UINavigationType.None)
        //    //{
        //    //    EditorGUILayout.LabelField("Selectables");
        //    //    EditorGUILayout.IntField(new GUIContent("Selectables Size"), selectables.arraySize);
        //    //}

        //    serializedObject.ApplyModifiedProperties();
        //}
    }
}
