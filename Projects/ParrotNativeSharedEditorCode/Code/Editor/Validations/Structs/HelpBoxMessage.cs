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
    /// This objects stores a help box messages and a type
    /// for inspector validation logging.
    /// </summary>
    public readonly struct HelpBoxMessage
    {
        private readonly string message;
        private readonly MessageType messageType;
        private readonly bool isWideHelpBox;

        /// <summary>
        /// A message to display to the helpbox.
        /// </summary>
        /// <returns>
        /// Returns a message to display to the helpbox.
        /// </returns>
        public string Message => message;

        public MessageType MessageType => messageType;
        public bool IsWideHelpBox => isWideHelpBox;

        /// <summary>
        /// Constructor for building a <see cref="HelpBoxMessage"/>.
        /// </summary>
        /// <param name="message">The message to display on a helpbox.</param>
        /// <param name="messageType">The log message type <see cref="MessageType"/></param>.
        /// <param name="isWideHelpBox">Defines if the helpbox should be widely displayed.</param>
        public HelpBoxMessage(string message, MessageType messageType, bool isWideHelpBox = false)
        {
            this.message = message;
            this.messageType = messageType;             
            this.isWideHelpBox = isWideHelpBox;         
        }

        /// <summary>
        /// Constructor for building a <see cref="HelpBoxMessage"/> 
        /// from a <see cref="InspectorValidationResults"/>
        /// </summary>
        /// <param name="validationResults">Validation results to store as helpbox message.</param>
        public HelpBoxMessage(InspectorValidationResults validationResults)
        {
            this.message = validationResults.Message;
            this.messageType = validationResults.Type;
            this.isWideHelpBox = validationResults.IsWideHelpBox;
        }
    }
}
