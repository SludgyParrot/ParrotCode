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

#region Included Parrot Code Assemblies
using ParrotCode.EventSystem;
#endregion

namespace ParrotCode.Native.SharedEditor
{
    /// <summary>
    /// Unity asset post-processor that broadcasts asset change events through the
    /// Parrot Code event system.
    /// </summary>
    /// <remarks>
    /// This class listens to Unity's <see cref="AssetPostprocessor.OnPostprocessAllAssets"/> callback
    /// and forwards asset change information (added, removed, moved, and moved-from paths)
    /// via <see cref="EditorAssetPostProcessorEvent"/>.
    /// </remarks>
    public sealed class EditorAssetPostProcessor : AssetPostprocessor
    {
        /// <summary>
        /// Called by Unity after assets have been imported, deleted, or moved.
        /// </summary>
        /// <param name="addedAssetPaths">
        /// Paths of newly imported assets.
        /// </param>
        /// <param name="removedAssetPaths">
        /// Paths of assets that were deleted.
        /// </param>
        /// <param name="newAssetPaths">
        /// Paths of assets that were moved to new locations.
        /// </param>
        /// <param name="oldAssetPaths">
        /// Previous paths of assets that were moved.
        /// </param>
        /// <remarks>
        /// This method forwards asset change information to the <c>EventBus</c> by invoking
        /// an <see cref="EditorAssetPostProcessorEvent"/>.
        /// </remarks>
        private static void OnPostprocessAllAssets(
            string[] addedAssetPaths,
            string[] removedAssetPaths,
            string[] newAssetPaths,
            string[] oldAssetPaths)
        {
            EventBus.InvokeEvent(new EditorAssetPostProcessorEvent(
                addedAssetPaths,
                removedAssetPaths,
                newAssetPaths,
                oldAssetPaths));
        }
    }
}
