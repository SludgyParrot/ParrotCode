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

using UnityEngine;
using UnityEngine.InputSystem;

namespace ParrotCode.InputSystem
{ 
    public class InputActionEvent
    {
        private readonly InputScheme scheme;
        private readonly InputActionType action;
        private readonly float inputAxis;
        private readonly Vector2 inputAxis2D;
        private readonly InputAction.CallbackContext callback;
        private readonly bool performed;

        public InputScheme Sheme => scheme;
        public InputActionType Action => action;
        public float InputAxis => inputAxis;
        public Vector2 InputAxis2D => inputAxis2D;
        public bool Performed => performed;

        public InputActionEvent(InputScheme scheme = default, InputActionType action = default, float inputAxis = default, Vector2 inputAxis2D = default, bool performed = default)
        {
            this.scheme = scheme;
            this.action = action;
            this.inputAxis = inputAxis;
            this.inputAxis2D = inputAxis2D;
            this.performed = performed;
        }
    }
}
