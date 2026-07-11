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

#region Included System Assemblies
using System.Reflection;
using System.Linq;
#endregion

#region Included Unity Assemblies
using UnityEngine;
using UnityEditor;
#endregion

namespace ParrotCode.Native.SharedEditor
{
    /// <summary>
    /// Custom inspector for <see cref="BaseMonoBehaviour"/> that renders methods
    /// decorated with <see cref="ButtonAttribute"/> as clickable buttons.
    /// </summary>
    /// <remarks>
    /// Methods marked with <see cref="ButtonAttribute"/> are displayed beneath the
    /// default inspector. Only parameterless, non-generic methods are supported.
    /// Both public and non-public instance methods are discovered through reflection.
    /// </remarks>
    [CustomEditor(typeof(BaseMonoBehaviour), true)]
    public sealed class BaseMonoBehaviourEditor : Editor
    {
        /// <summary>
        /// The vertical spacing, in pixels, inserted before each generated button.
        /// </summary>
        private const int ButtonSpacing = 5;

        /// <summary>
        /// Draws the default inspector and renders buttons for methods decorated
        /// with <see cref="ButtonAttribute"/>.
        /// </summary>
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var methods = target.GetType().GetMethods(
                BindingFlags.Instance |
                BindingFlags.Static |
                BindingFlags.Public |
                BindingFlags.NonPublic)
                .Where(method => method.GetCustomAttribute<ButtonAttribute>() != null);

            foreach (var method in methods)
            {
                if (method.IsGenericMethod || method.GetParameters().Length > 0)
                    continue;

                var attribute = method.GetCustomAttribute<ButtonAttribute>();
                string label = string.IsNullOrEmpty(attribute.Label)
                    ? ObjectNames.NicifyVariableName(method.Name)
                    : attribute.Label;

                GUILayout.Space(ButtonSpacing);

                if (GUILayout.Button(label))
                    method.Invoke(target, null);
            }
        }
    }
}
