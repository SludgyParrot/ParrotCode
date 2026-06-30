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
using System.Text.RegularExpressions;
#endregion

namespace ParrotCode.Extensions
{
    /// <summary>
    /// This class contains extension methods for <see cref="string"/>.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Adds white space between words that begins with an upper case letter,
        /// excluding the first word.
        /// </summary>
        /// <returns>A formatted string with white spaces between 
        /// words that begins with an upper case letter.</returns>
        public static string AddWhiteSpace(this string str)
        {
            string pattern = @"(?<!^)(?=[A-Z])";
            string results = Regex.Replace(str, pattern, " ");
            return results;
        }

        /// <summary>
        /// Removes white space between words that begins with an upper case letter, 
        /// excluding the first word.
        /// </summary>
        /// <returns>A formatted string with white spaces between 
        /// words that begins with an upper case letter removed.</returns>
        public static string RemoveWhiteSpace(this string str)
        {
            string pattern = @"(?<!^)(?=[A-Z])";
            string results = Regex.Replace(str, pattern, string.Empty);
            return results;
        }

        /// <summary>
        /// This function checks if this <see cref="string"/> is null or empty.
        /// </summary>
        /// <returns>True if this <see cref="string"/> is null or empty, else False.</returns>
        public static bool IsNullOrEmpty(this string str)
            => string.IsNullOrEmpty(str);

        /// <summary>
        /// This function checks if this <see cref="string"/> is null or consist entirely of white space.
        /// </summary>
        /// <returns>True if this <see cref="string"/> is null or consist entirely of white space, else False.</returns>
        public static bool IsNullOrWhiteSpace(this string str)
            => string.IsNullOrWhiteSpace(str);
    }
}
