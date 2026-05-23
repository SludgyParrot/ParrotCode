using System.Reflection;
using UnityEngine;
using UnityEditor;
using ParrotCode.Native.Common;
using System.Linq;
namespace ParrotCode.Native.Inspector
{
    [CustomEditor(typeof(BaseMonoBehaviour), true)]
    public sealed class BaseMonobehaviorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            var methods = target.GetType().GetMethods( BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).Where(method => method.GetCustomAttribute<ButtonAttribute>() !=  null);

            foreach(var method in methods)
            {
                if (method.IsGenericMethod)
                    continue;

                var attribute = method.GetCustomAttribute<ButtonAttribute>();
                string label = string.IsNullOrEmpty(attribute.Label) ? ObjectNames.NicifyVariableName(method.Name) : attribute.Label;
                GUILayout.Space(5);
                if (GUILayout.Button(label))
                    method.Invoke(target, null);
            }
        }
    }
}
