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

using System;
using System.Data.Common;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ParrotCode.Editors.Multiplayer
{
    /// <summary>
    /// Custom editor for the MultiplayerScene component. This editor provides a user-friendly interface for configuring multiplayer scenes in the Unity Editor. It allows developers to easily set up and manage multiplayer scenes, including options for player spawning, network settings, and scene transitions. By using this custom editor, developers can streamline the process of creating and maintaining multiplayer scenes, ensuring a smoother development experience and better gameplay outcomes.
    /// </summary>
    public sealed class MultiplayerSceneEditorWindow : EditorWindow
    {
        private const string MultiplayerSceneWindowName = "SP Multiplayer Manager";
        private const string WindowMenuPath = "Sludgy Parrot/Multiplayer/Generate Template Scene #M";

        #region Window properties
        private static Vector2 MinWindowSize = new Vector2(256.0f, 512.0f);
        private static Vector2 MaxWindowSize = new Vector2(720.0f, 1080.0f);
        #endregion

        [MenuItem(WindowMenuPath)]
        private static void OpenMultiplayerSceneEditorWindow()
        {
            EditorWindow window = CreateWindow<MultiplayerSceneEditorWindow>(MultiplayerSceneWindowName);
            window.minSize = MinWindowSize;
            window.maxSize = MaxWindowSize;
        }
    }
}
