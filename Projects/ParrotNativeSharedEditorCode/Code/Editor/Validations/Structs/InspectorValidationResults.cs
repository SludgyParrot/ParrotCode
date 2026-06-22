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

namespace ParrotCode.Native.SharedEditor
{
    /// <summary>
    /// This object stores validation results log information.
    /// </summary>
    public readonly struct InspectorValidationResults
    {
        private readonly bool validated;
        private readonly string message;
        private readonly MessageType type;
        private readonly bool isWideHelpBox;

        /// <summary>
        /// Returns true is is valdated results.
        /// </summary>
        public bool Validated => validated;

        /// <summary>
        /// Returns a logged message. Ususally when the validation fails.
        /// </summary>
        public string Message => message;

        /// <summary>
        /// Returns the log type for the returned results.
        /// </summary>
        public MessageType Type => type;

        /// <summary>
        /// Defines whether the displayed helpbox should be wide or not.
        /// Returns true by defualt.
        /// </summary>
        public bool IsWideHelpBox => isWideHelpBox;

        public InspectorValidationResults(bool validated, string message, MessageType type, bool isWideHelpBox = true)
        {
            this.validated = validated;
            this.message = message;
            this.type = type;
            this.isWideHelpBox = isWideHelpBox;
        }
    }
}
