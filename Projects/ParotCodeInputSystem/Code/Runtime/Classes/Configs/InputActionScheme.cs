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
using ParrotCode.Native.SharedEditor;

namespace ParrotCode.InputSystem
{
    [CreateAssetMenu(fileName = "Input Action Scheme", menuName = SharedEditorToolMenusPath.ParrotCodeConfigRootPath + "Input Action Scheme")]
    public sealed class InputActionScheme: ScriptableObject
    {
        [SerializeField]
        private InputScheme scheme;

        [SerializeField, Space(5)]
        private List<InputActionConfig> inputActionConfigs = new List<InputActionConfig>();

        public InputScheme Scheme => scheme;
        public IReadOnlyList<InputActionConfig> InputActionConfigs => inputActionConfigs;
    }
}
