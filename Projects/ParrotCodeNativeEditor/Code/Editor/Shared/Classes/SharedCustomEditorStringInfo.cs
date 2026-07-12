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
using ParrotCode.Native.SharedEditor;
#endregion

namespace ParrotCode.Native.SharedEditor
{
    /// <summary>
    /// Defines shared string resources used throughout the Parrot Code custom editor framework.
    /// </summary>
    public static class SharedCustomEditorStringInfo
    {
        #region Settings Titles

        /// <summary>
        /// Title displayed for the Rendering Settings section.
        /// </summary>
        public static readonly string ProjectRenderingSettingsTitle = "Rendering Settings";

        #endregion

        #region Shared Project Titles

        /// <summary>
        /// Title displayed for the Project Settings section.
        /// </summary>
        public static readonly string ProjectSettingsTitle = "Project Settings";

        /// <summary>
        /// Title displayed for the project configuration confirmation popup.
        /// </summary>
        public static readonly string ProjectConfigurationPopupTitle = "Parrot Code: Apply";

        #endregion

        #region GUI Button Labels

        /// <summary>
        /// Label displayed on the button used to build the project.
        public static readonly string BuildProjectButtonLabel = "Build Project";

        /// <summary>
        /// Label displayed on the button used to apply project settings.
        /// </summary>
        public static readonly string ApplySettingsButtonLabel = "Apply Settings";

        /// <summary>
        /// Label displayed on a confirm action button.
        /// </summary>
        public const string ConfirmButtonTitle = "Yes Please!";

        /// <summary>
        /// Label displayed on a cancel action button.
        /// </summary>
        public const string CancelButtonTitle = "No Thanks!";

        #endregion

        #region Popup Messages

        /// <summary>
        /// Confirmation message displayed before applying platform-specific project settings.
        /// The placeholder expects the platform configuration name.
        /// </summary>
        public static readonly string ProjectConfigurationPopupMessage =
            "This operation will configure the Unity platform specific {0}. " +
            "This action will override existing settings and this action may not be undone. Do you wish to proceed?";

        /// <summary>
        /// Confirmation message displayed before applying a predefined project configuration.
        /// The placeholder expects the configuration name.
        /// </summary>
        public static readonly string ProjectConfigurationWarningPopupMessage =
            $"This operation will configure the Unity " +
            $"{0} project's settings to a predefined {1} configuration data. " +
            "This action will override existing settings and this action may not be undone. Do you wish to proceed?";

        #endregion

        #region Tools Bar Menu Paths

        /// <summary>
        /// Menu path used to apply the Development project configuration.
        /// </summary>
        public const string DevelopmentSettingsRootPath =
            SharedEditorToolMenusPath.ProjectSettingsToolsMenuRoot +
            Development +
            ParrotHotKeys.ProjectDevelopmentConfiguration;

        /// <summary>
        /// Menu path used to apply the Production project configuration.
        /// </summary>
        public const string ProductionSettingsRootPath =
            SharedEditorToolMenusPath.ProjectSettingsToolsMenuRoot +
            Production +
            ParrotHotKeys.ProjectProductionConfiguration;

        #endregion

        #region Project Assets Filtering Flags

        /// <summary>
        /// Asset search filter used to locate a generic assets.
        /// </summary>
        /// <typeparam name="T">The asset type to be converted into a filter type.</typeparam>
        /// <returns><see langword="string"/> with a type filter. i.e t:Type.</returns>
        public static string GetAssetDatabaseTypeFilter<T>()
            => $"t:{typeof(T)!.Name}";

        #endregion

        #region Keywords

        /// <summary>
        /// Development configuration keyword.
        /// </summary>
        public const string Development = "Development ";

        /// <summary>
        /// Production configuration keyword.
        /// </summary>
        public const string Production = "Production ";

        /// <summary>
        /// Type name used for project build configuration assets.
        /// </summary>
        public static readonly string ProjectBuildConfigGroup = "ProjectBuildConfigGroup"; 

        #endregion
    }
}
