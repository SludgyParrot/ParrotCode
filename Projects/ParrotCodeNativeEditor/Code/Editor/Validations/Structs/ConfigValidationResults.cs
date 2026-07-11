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

#region Included Unity Assemblies
using UnityEditor;
#endregion

namespace ParrotCode.Native.SharedEditor
{
    /// <summary>
    /// A validation component for <see cref="ConfigValidationResults<T>"/> config.
    /// </summary>
    /// <typeparam name="T">A config type to validate.</typeparam>
    public readonly struct ConfigValidationResults<T>
    {
        private readonly MessageType messageType;
        private readonly string message;
        private readonly T value;

        /// <summary>
        /// Results message type.
        /// </summary>
        public readonly MessageType MessageType => messageType;

        /// <summary>
        /// Results message.
        /// </summary>
        public readonly string Message => message;

        /// <summary>
        /// Results config value.
        /// </summary>
        public readonly T Value => value;

        /// <summary>
        /// <see cref="ConfigValidationResults<T>"/> constructor.
        /// </summary>
        /// <param name="message">The message to log.</param>
        /// <param name="messageType">The message type for logging</param>
        /// <param name="value">The config value.</param>
        public ConfigValidationResults(MessageType messageType, string message, T value)
        {
            this.messageType = messageType;
            this.message = message;
            this.value = value;
        }

        /// <summary>
        /// Returns a new default instance of <see cref="ConfigValidationResults<T>"/>.
        /// </summary>
        public static ConfigValidationResults<T> Empty = 
            new ConfigValidationResults<T>(MessageType.None, string.Empty, default);
    }
}
