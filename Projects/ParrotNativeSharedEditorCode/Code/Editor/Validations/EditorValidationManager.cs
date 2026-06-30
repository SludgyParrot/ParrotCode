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

#region Included Systems Assemblies
using System;
using System.Collections.Generic;
#endregion

#region Included Unity Assemblies
using UnityEditor;
using UnityEngine;
#endregion

#region Included Parrot Code
using ParrotCode.EventSystem;
#endregion

namespace ParrotCode.Native.SharedEditor
{
    /// <summary>
    /// Provides a base implementation for validating editor <see cref="ScriptableObject"/> assets
    /// while caching validation results to avoid unnecessary repeated validation.
    /// </summary>
    /// <typeparam name="TValidator">
    /// The validator implementation responsible for validating editor assets.
    /// </typeparam>
    public abstract class EditorValidationManager<TValidator> : IDisposable where TValidator : IEditorAssetValidator
    {
        /// <summary>
        /// Stores cached validation results indexed by the asset GUID.
        /// </summary>
        protected readonly Dictionary<string, ValidationCache> _validationCache =
            new Dictionary<string, ValidationCache>();

        /// <summary>
        /// Gets the validator used to validate editor assets.
        /// </summary>
        protected abstract TValidator Validator { get; }

        private bool _disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditorValidationManager{TValidator}"/> class
        /// and subscribes to editor asset post-processing events so cached validation results can be
        /// invalidated when assets change.
        /// </summary>
        protected EditorValidationManager()
        {
            EventBus.AddListener<EditorAssetPostProcessorEvent>(OnEditorAssetPostProcessedEvent);
        }

        #region Validations

        /// <summary>
        /// Validates the specified configuration asset.
        /// </summary>
        /// <typeparam name="T">
        /// The type of <see cref="ScriptableObject"/> being validated.
        /// </typeparam>
        /// <param name="config">
        /// The configuration asset to validate.
        /// </param>
        /// <returns>
        /// A <see cref="HelpBoxMessage"/> containing the validation results.
        /// Cached results are returned whenever possible.
        /// </returns>
        public virtual HelpBoxMessage Validate<T>(T config) where T : ScriptableObject
        {
            string guid = GetAssetGuid(config);

            if (!_validationCache.TryGetValue(guid, out var validated))
            {
                var results = Validator.Validate(config);
                validated = new ValidationCache(results);
                _validationCache[guid] = validated;

                return validated.Results;
            }

            if (!validated.Dirty)
            {
                return validated.Results;
            }

            validated.Results = Validator.Validate(config);

            return validated.SetDirty().Results;
        }

        #endregion

        #region Invalidating Cache

        /// <summary>
        /// Marks every cached validation result as invalid, forcing assets to be
        /// revalidated the next time they are requested.
        /// </summary>
        public void InvalidateCache()
        {
            foreach (var cache in _validationCache)
                cache.Value.Clear();
        }

        /// <summary>
        /// Marks the cached validation result for the specified asset as invalid.
        /// </summary>
        /// <typeparam name="T">
        /// The type of <see cref="ScriptableObject"/>.
        /// </typeparam>
        /// <param name="config">
        /// The asset whose cached validation result should be invalidated.
        /// </param>
        public void InvalidateCache<T>(T config) where T : ScriptableObject
        {
            string guid = GetAssetGuid(config);

            if (_validationCache.TryGetValue(guid, out var cache))
                cache.Clear();
        }

        #endregion

        #region Clearing Cache

        /// <summary>
        /// Removes all cached validation results.
        /// </summary>
        public void ClearCache()
            => _validationCache.Clear();

        #endregion

        /// <summary>
        /// Gets the Asset Database GUID for the specified configuration asset.
        /// </summary>
        /// <typeparam name="T">
        /// The type of <see cref="ScriptableObject"/>.
        /// </typeparam>
        /// <param name="config">
        /// The asset whose GUID should be retrieved.
        /// </param>
        /// <returns>
        /// The Asset Database GUID associated with the asset.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="config"/> is <see langword="null"/>.
        /// </exception>
        private string GetAssetGuid<T>(T config) where T : ScriptableObject
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config), "Configuration asset cannot be null.");
            }

            return AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(config));
        }

        /// <summary>
        /// Handles editor asset post-processing events by invalidating all cached
        /// validation results.
        /// </summary>
        /// <param name="evt">
        /// The asset post-processing event.
        /// </param>
        private void OnEditorAssetPostProcessedEvent(EditorAssetPostProcessorEvent evt)
        {
            InvalidateCache();
        }

        /// <summary>
        /// Releases the resources used by this instance.
        /// </summary>
        /// <remarks>
        /// This method unsubscribes the validation manager from editor asset post-processing
        /// events and suppresses finalization. Derived classes should override
        /// <see cref="Dispose(bool)"/> to release additional managed resources.
        /// </remarks>
        public void Dispose()
        {
            Dispose(true); 
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the managed resources used by this instance.
        /// </summary>
        /// <param name="disposing">
        /// <see langword="true"/> to release managed resources; otherwise,
        /// <see langword="false"/>.
        /// </param>
        /// <remarks>
        /// Derived classes overriding this method should release their managed resources
        /// when <paramref name="disposing"/> is <see langword="true"/> and invoke the
        /// base implementation.
        /// </remarks>
        protected virtual void Dispose(bool disposing)
        {
            if(_disposed || !disposing)
            {
                return;
            }

            EventBus.RemoveListener<EditorAssetPostProcessorEvent>(OnEditorAssetPostProcessedEvent);
            _disposed = true;
        }
    }
}
