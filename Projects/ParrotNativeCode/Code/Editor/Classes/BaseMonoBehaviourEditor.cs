using System.Reflection;
using UnityEngine;
using UnityEditor;
using ParrotCode.Native.Common;
using System.Linq;

namespace ParrotCode.Native.Inspector
{ 
    [CustomEditor(typeof(BaseMonoBehaviour), true)]
    public sealed class BaseMonoBehaviourEditor: Editor
    {
        private const int ButtonSpacing = 5;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var methods = target.GetType().GetMethods(BindingFlags.Instance | 
                BindingFlags.Static | 
                BindingFlags.Public | 
                BindingFlags.NonPublic).Where(method => method.GetCustomAttribute<ButtonAttribute>() != null);

            foreach (var method in methods)
            {
                if(method.IsGenericMethod || method.GetParameters().Length > 0)
                    continue;

                var attribute = method.GetCustomAttribute<ButtonAttribute>();
                string label = string.IsNullOrEmpty(attribute.Label)? ObjectNames.NicifyVariableName(method.Name): attribute.Label;

                GUILayout.Space(ButtonSpacing);

                if (GUILayout.Button(label))
                    method.Invoke(target, null);
            }
        }

        private GUIStyle GetStyle()
        {
            GUIStyle style = GUI.skin.box;
            style.fixedHeight = 25;
            style.fixedWidth = 300;
            style.hover.textColor = Color.white;
            return style;
        }
    }
}
