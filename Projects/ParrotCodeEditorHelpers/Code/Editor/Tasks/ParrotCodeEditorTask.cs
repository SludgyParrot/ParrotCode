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

#region Included Unity Assemblies
using UnityEngine;
#endregion

#region Included Jet Brains Assemblies
using System.Threading.Tasks;
#endregion

namespace ParrotCode.Helpers
{
    /// <summary>
    /// Provides helper methods for executing asynchronous editor operations while
    /// logging any unhandled exceptions to the Unity Console.
    /// </summary>
    public static class ParrotCodeEditorTask
    {
        /// <summary>
        /// Executes an asynchronous operation that returns a value while logging
        /// any unhandled exceptions before rethrowing them.
        /// </summary>
        /// <typeparam name="T">
        /// The type of value returned by the asynchronous operation.
        /// </typeparam>
        /// <param name="task">
        /// The asynchronous operation to execute.
        /// </param>
        /// <returns>
        /// A task that completes with the value returned by the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="task"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="Exception">
        /// Rethrows any exception thrown by the asynchronous operation after it has been logged.
        /// </exception>
        public static async Task<T> Run<T>(Func<Task<T>> task)
        {
            try
            {
                return await task();
            }
            catch(Exception exception)
            {
                Debug.LogException(exception);
                throw;
            }
        }

        /// <summary>
        /// Executes an asynchronous operation while logging any unhandled exceptions
        /// before rethrowing them.
        /// </summary>
        /// <param name="task">
        /// The asynchronous operation to execute.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="task"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="Exception">
        /// Rethrows any exception thrown by the asynchronous operation after it has been logged.
        /// </exception>
        public static async Task Run(Func<Task> task)
        {
            try
            {
                await task();
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
                throw;
            }
        }
    }
}
