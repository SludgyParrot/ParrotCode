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

namespace ParrotCode.Native.SharedEditor
{
    /// <summary>
    /// Base implementation for a configuration validation rule used in a
    /// rule-based validation pipeline.
    /// </summary>
    /// <typeparam name="T">
    /// The type of configuration object being validated.
    /// </typeparam>
    /// <remarks>
    /// Derive from this class to implement strongly-typed validation rules that can
    /// optionally use additional contextual data during validation.
    /// </remarks>
    public abstract class ConfigValidationRule<T> : IConfigValidationRule<T>
    {
        /// <summary>
        /// Gets the execution order of this validation rule within the validation pipeline.
        /// Lower values are executed first.
        /// </summary>
        public abstract int Order { get; }

        /// <summary>
        /// Validates the specified configuration instance using optional contextual data.
        /// </summary>
        /// <param name="config">
        /// The configuration object to validate.
        /// </param>
        /// <param name="data">
        /// Optional contextual data that can influence validation logic.
        /// This value may be <see langword="null"/>.
        /// </param>
        /// <returns>
        /// A <see cref="HelpBoxMessage"/> describing validation results.
        /// Return <see cref="HelpBoxMessage.Empty"/> when no issues are found.
        /// </returns>
        public abstract HelpBoxMessage Validate(T config, object data = null);
    }
}
