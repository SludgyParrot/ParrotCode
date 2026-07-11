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

using System;
using System.Collections.Generic;
using UnityEngine;

namespace ParrotCode.Native
{
    /// <summary>
    /// Base class for logging unity messages.
    /// </summary>
    public abstract class UnityConsoleLogger: MonoBehaviour
    {
        [SerializeField]
        protected LogVerbosity verbosity;

        public LogVerbosity Verbosity => verbosity;

        #region Initialization
        private void Awake()
            => Init();

        protected abstract void Init();
        #endregion

        #region Logging

        protected void Log(string message, LogVerbosity verbosity = LogVerbosity.Debug, LogChannel channel = LogChannel.General, params string[] args)
            => Log(message, verbosity, channel, null, args);

        protected void Log(string message, LogVerbosity verbosity = LogVerbosity.Debug, LogChannel channel = LogChannel.General, Func<bool> assert = null, params string[] args)
        {
            if(verbosity > this.verbosity)
                return;

            var messageFormatResults = GetFormattedLogMessage(message, verbosity, channel, args);
            string logMessage = string.Format(messageFormatResults.message, messageFormatResults.options);

            switch(verbosity)
            {
                case LogVerbosity.Debug:
                    Debug.Log(logMessage, this);
                    break;
                case LogVerbosity.Warning:
                    Debug.LogWarning(logMessage, this);
                    break;
                case LogVerbosity.Error:
                    Debug.LogError(logMessage, this);
                    break;
                case LogVerbosity.Exception:
                    Debug.LogException(new Exception(logMessage), this);
                    break;
                case LogVerbosity.Assert:
                    Debug.Assert(assert.Invoke(), logMessage, this);
                    break;
            }
        }

        private (string message, string [] options) GetFormattedLogMessage(string message, LogVerbosity verbosity = LogVerbosity.Debug, LogChannel channel = LogChannel.General, params string[] args)
        {
            string formattedMessage = $"Console log: [{GetVerbosityColor(verbosity.ToString(), verbosity)}] - Channel: [<color=magenta>{channel.ToString()}</color>] <color=white>{message}</color>";
            List<string> options = new List<string>();

            foreach(string arg in args)
            {
                string option = GetVerbosityColor(arg, verbosity);
                options.Add(option);
            }

            return (formattedMessage, options.ToArray());
        }

        private string GetVerbosityColor(string message, LogVerbosity verbosity, bool makeBold = true)
        {
            string formattedString = string.Empty;

            switch (verbosity)
            {
                case LogVerbosity.Debug:
                    formattedString = makeBold? $"<b><color=cyan>{message}</color></b>" : $"<color=white>{message}</color>";
                    break;
                case LogVerbosity.Warning:
                    formattedString = makeBold ? $"<b><color=yellow>{message}</color></b>" : $"<color=white>{message}</color>";
                    break;
                case LogVerbosity.Error:
                    formattedString = makeBold ? $"<b><color=red>{message}</color></b>" : $"<color=white>{message}</color>";
                    break;
                case LogVerbosity.Exception:
                    formattedString = makeBold ? $"<b><color=red>{message}</color></b>" : $"<color=white>{message}</color>";
                    break;
                case LogVerbosity.Assert:
                    formattedString = makeBold ? $"<b><color=black>{message}</color></b>" : $"<color=white>{message}</color>";
                    break;
            }

            return formattedString;
        }

        #endregion
    }
}
