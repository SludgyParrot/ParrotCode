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

using JetBrains.Annotations;

namespace ParrotCode.Native.SharedEditor
{
    /// <summary>
    /// Represents a validation rule used in a configuration validation pipeline.
    /// </summary>
    /// <typeparam name="T">
    /// The type of configuration object being validated.
    /// </typeparam>
    /// <remarks>
    /// Validation rules are executed in order defined by <see cref="Order"/>.
    /// Each rule evaluates the provided configuration and returns a
    /// <see cref="HelpBoxMessage"/> describing validation results.
    /// </remarks>
    public interface IConfigValidationRule<T>
    {
        /// <summary>
        /// Gets the execution order of this validation rule within the validation pipeline.
        /// Lower values are executed first.
        /// </summary>
        int Order { get; }

        /// <summary>
        /// Validates the specified configuration instance using optional contextual data.
        /// </summary>
        /// <param name="config">
        /// The configuration object to validate.
        /// </param>
        /// <param name="data">
        /// Optional contextual data that can influence validation logic.
        /// This parameter may be <see langword="null"/>.
        /// </param>
        /// <returns>
        /// A <see cref="HelpBoxMessage"/> describing validation results.
        /// Returns <see cref="HelpBoxMessage.Empty"/> when no issues are found.
        /// </returns>
        HelpBoxMessage Validate(T config, [CanBeNull] object data = null);
    }
}
