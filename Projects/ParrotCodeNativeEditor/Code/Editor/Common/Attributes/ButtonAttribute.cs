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
using System;
#endregion

namespace ParrotCode.Native.SharedEditor
{
    /// <summary>
    /// Specifies that a method should be rendered as a clickable button in a custom
    /// editor or inspector.
    /// </summary>
    /// <remarks>
    /// This attribute can only be applied to methods and is not inherited by
    /// derived classes. An optional label may be supplied to override the default
    /// button text displayed in the user interface.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class ButtonAttribute : Attribute
    {
        /// <summary>
        /// Gets the text displayed on the button.
        /// </summary>
        /// <value>
        /// The custom button label specified when the attribute was constructed,
        /// or <see langword="null"/> if no label was provided.
        /// </value>
        public string Label { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ButtonAttribute"/> class.
        /// </summary>
        /// <remarks>
        /// When no label is specified, consumers of this attribute should typically
        /// use the name of the decorated method as the button text.
        /// </remarks>
        public ButtonAttribute() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ButtonAttribute"/> class
        /// with the specified button label.
        /// </summary>
        /// <param name="label">
        /// The text to display on the button.
        /// </param>
        public ButtonAttribute(string label)
            => Label = label;
    }
}
