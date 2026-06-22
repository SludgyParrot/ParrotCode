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

using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using ParrotCode.Native.SharedEditor;

namespace ParrotCode.Platforms
{
    /// <summary>
    /// This class contains rendering project configurators for cross platforms.
    /// </summary>
    /// <remarks>
    /// This is a scriptable object component that derives from <href>UnityEngine</href> 
    /// </remarks>
    [CreateAssetMenu(fileName = "Rendering Settings", menuName = ProjectSharedDirectory.PlatformConfigRootPath + "Rendering Settings")]
    public sealed class RenderingProjectBuildConfig : ProjectSpecificBuildConfig
    {
        #region General Rendering Settings
        [Header("General Settings")]

        [SerializeField, Space(5), Tooltip("Specify a list of graphics API " +
            "(DirectX, Vulkan, Metal, OpenGL, etc.) to use for the selected platform. If the order is not explicitly defined," +
            " Unity will automatically selects the graphics backend for the selected platform (DirectX, Vulkan, Metal, OpenGL, etc.)")]
        private GraphicsDeviceType[] graphicsAPI;

        public IReadOnlyList<GraphicsDeviceType> GraphicsAPI => graphicsAPI;

        #endregion

        #region Android Rendering Settings
        [SerializeField, Space(5)]
        private AndroidRenderingProjectBuildConfig androidSettings = new AndroidRenderingProjectBuildConfig();
        public AndroidRenderingProjectBuildConfig AndroidSettings => androidSettings;
        #endregion

        #region IOS Rendering Settings
        [SerializeField, Space(5)]
        private IOSRenderingProjectBuildConfig iosSettings = new IOSRenderingProjectBuildConfig();
        public IOSRenderingProjectBuildConfig IOSSettings => iosSettings;
        #endregion

        #region Windows Rendering Settings
        [SerializeField, Space(5)]
        private WindowsRenderingProjectBuildConfig windowsSettings = new WindowsRenderingProjectBuildConfig();
        public WindowsRenderingProjectBuildConfig WindowsSettings => windowsSettings;
        #endregion

        #region WebGL Rendering Settings
        [SerializeField, Space(5)]
        private WebGLRenderingProjectBuildConfig webGLSettings = new WebGLRenderingProjectBuildConfig();
        public WebGLRenderingProjectBuildConfig WebGLSettings => webGLSettings;
        #endregion

        #region Settings
        private readonly Dictionary<BuildTarget, IRenderingProjectBuildConfig> renderingSettings 
            = new Dictionary<BuildTarget, IRenderingProjectBuildConfig>();
        #endregion

        /// <summary>
        /// A build target for the active editor.
        /// </summary>
        /// <return>
        /// The active build target <see cref="BuildTarget"/>.
        /// </return>
        public override BuildTarget BuildTarget => EditorUserBuildSettings.activeBuildTarget;

        private void OnEnable()
        {
            InitializeRenderingSettings();
        }

        private void OnDisable() 
            => OnReset();

        private void InitializeRenderingSettings()
        {
            renderingSettings[BuildTarget.Android] = AndroidSettings;
            renderingSettings[BuildTarget.iOS] = IOSSettings;
            renderingSettings[BuildTarget.StandaloneWindows] = WindowsSettings;
            renderingSettings[BuildTarget.StandaloneWindows64] = WindowsSettings;
            renderingSettings[BuildTarget.WebGL] = WebGLSettings;
        }

        private void OnReset()
        {
            renderingSettings.Clear();
        }

        /// <summary>
        /// Applies user specified configuration to the ploject.
        /// </summary>
        public override void ApplySettings()
        {
            if(!renderingSettings.TryGetValue(BuildTarget, out IRenderingProjectBuildConfig settings))
            {
                Debug.LogError($"[{name}] Apply project rendering settings for build target: {BuildTarget} failed. " +
                    $"Build target: {BuildTarget} is not currently supported on this version of Parrot Code framework.");
                return;
            }

            if(settings.UnsupportedGraphicsAPIFound(GraphicsAPI).Count > 0)
            {
                Debug.LogError($"[{name}] Apply project rendering settings for build target: {BuildTarget} failed. " +
                   $"Config contains unsupported graphics API for build target: {BuildTarget}. Please resolve any pending issues for '{name}' at path: {AssetDatabase.GetAssetPath(this)}.");
                return;
            }

            if(settings.DeprecatedGraphicsAPIFound(GraphicsAPI).Count > 0)
            {
                Debug.LogWarning($"[{name}] The applied project rendering settings for build target: {BuildTarget} contains deprected" +
                    $" graphics API for runtime platform: {BuildTarget}. Please check rendering config asset '{name}' at path: {AssetDatabase.GetAssetPath(this)}.");
            }

            // Apply general platform rendering settings.
            GraphicsDeviceType[] varifiedGraphicsAPI = GraphicsAPI.Where(graphicsAPI => !settings.DeprecatedGraphicsAPI.Contains(graphicsAPI) && settings.SupportedGraphicsAPI.Contains(graphicsAPI)).ToArray();
            ApplyGeneralSettings(varifiedGraphicsAPI);

            // Apply platform specific configurations. e.g Android, Windows, WebGL etc.
            settings.ApplySettings();
        }

        private void ApplyGeneralSettings(GraphicsDeviceType[] graphicsAPIs)
        {
            bool useAutoGraphicsAPI = graphicsAPIs?.Length == 0;
            PlayerSettings.SetUseDefaultGraphicsAPIs(BuildTarget, useAutoGraphicsAPI);

            if(!useAutoGraphicsAPI)
            {
                PlayerSettings.SetGraphicsAPIs(BuildTarget, graphicsAPIs.ToArray());
            }

            Debug.Log($"[{name}] Successfully applied project rendering settings for build target: {BuildTarget}.");
        }

        #region Configuration Validation API
        /// <summary>
        /// Validation for platform specific rendering settings.
        /// </summary>
        /// <param name="renderingProjectBuild">Specified platform settings.</param>
        /// <returns></returns>
        public InspectorValidationResults Validate(IRenderingProjectBuildConfig renderingProjectBuild)
        {
            #region Validate Assigned Platform Configurations
            var validateAssignedPlatformResults = ValidateAssignedPlatformConfigurations(renderingProjectBuild);
            if (!validateAssignedPlatformResults.Validated)
                return validateAssignedPlatformResults;
            #endregion

            #region Validate Unsupported Graphic APIs
            var ValidateUnsupportedGraphicsAPIResults = ValidateUnsupportedGraphicsAPIs(renderingProjectBuild);
            if (!ValidateUnsupportedGraphicsAPIResults.Validated)
                return ValidateUnsupportedGraphicsAPIResults;
            #endregion

            #region Validate Duplicated Graphic APIs
            var ValidateSupportedGraphicsAPIDuplicateResults = ValidateSupportedGraphicsAPIDuplicates();
            if (!ValidateSupportedGraphicsAPIDuplicateResults.Validated)
                return ValidateSupportedGraphicsAPIDuplicateResults;
            #endregion

            #region Validate Deprecated Graphic APIs
            var ValidateDeprecatedGraphicsAPIResults = ValidateDeprecatedGraphicsAPIs(renderingProjectBuild);
            if (!ValidateDeprecatedGraphicsAPIResults.Validated)
                return ValidateDeprecatedGraphicsAPIResults;
            #endregion

            return new InspectorValidationResults(true, string.Empty, MessageType.None);
        }
        #endregion

        #region Configuration Validations
        private InspectorValidationResults ValidateAssignedPlatformConfigurations(IRenderingProjectBuildConfig renderingProjectBuild)
        {
            if (GraphicsAPI != null && GraphicsAPI.Count > 0)
                return new InspectorValidationResults(true, string.Empty, MessageType.None);

            EditorGUILayout.Space();

            string supportedGraphicsAPIs = string.Join("\n", renderingProjectBuild.SupportedGraphicsAPI.Select(graphicsAPI => $"* {graphicsAPI.ToString()}"));

            string validationErrorMessage = $"There are no graphics APIs defined for build target: {BuildTarget}." +
                $" \nUnity will automatically select one of the following supported graphics API(s) for {BuildTarget}: \n\n{supportedGraphicsAPIs}\n";

            return new InspectorValidationResults(false, validationErrorMessage, MessageType.Info);
        }

        private InspectorValidationResults ValidateUnsupportedGraphicsAPIs(IRenderingProjectBuildConfig renderingProjectBuild)
        {
            GraphicsDeviceType[] unsupportedGraphicsAPIs = (renderingProjectBuild.UnsupportedGraphicsAPIFound(GraphicsAPI)).ToArray();

            if (unsupportedGraphicsAPIs.Length == 0)
                return new InspectorValidationResults(true, string.Empty, MessageType.None);

            EditorGUILayout.Space();

            string unsupportedGraphicsAPINames = string.Join("\n", unsupportedGraphicsAPIs.Select(graphicsAPI => $"* {graphicsAPI.ToString()}"));

            string validationErrorMessage = $"[Unsupported graphics API detected] This configuration contains unsupported graphic API(s) for build target:" +
                $" {BuildTarget}. \nPlease remove the following unsupported graphics API(s) from the graphics API list: \n\n{unsupportedGraphicsAPINames}\n";

            return new InspectorValidationResults(false, validationErrorMessage, MessageType.Error);
        }

        private InspectorValidationResults ValidateSupportedGraphicsAPIDuplicates()
        {
            var supportedGraphicsAPIDuplicates = GraphicsAPI.GroupBy(graphhicsAPI => graphhicsAPI).Where(graphicsAPIGroup => graphicsAPIGroup.Count() > 1).ToArray();

            if (supportedGraphicsAPIDuplicates.Length == 0)
                return new InspectorValidationResults(true, string.Empty, MessageType.None);

            EditorGUILayout.Space();

            string[] duplicatedGraphicsAPINameGroup = supportedGraphicsAPIDuplicates.Select(group => $"* {group.Key} [{group.Count()}]").ToArray();
            string duplicatedGraphicsAPINames = string.Join("\n", duplicatedGraphicsAPINameGroup);

            string validationErrorMessage = $"[Duplicate graphics API(s) detected] This configuration contains duplicated graphic API(s) for build target: " +
              $"{BuildTarget}. \nPlease remove the following graphics API(s) from the graphics API list: \n\n{duplicatedGraphicsAPINames}\n";

            return new InspectorValidationResults(false, validationErrorMessage, MessageType.Error);
        }

        private InspectorValidationResults ValidateDeprecatedGraphicsAPIs(IRenderingProjectBuildConfig renderingProjectBuild)
        {
            IReadOnlyList<GraphicsDeviceType> deprecatedGraphicsAPIs = renderingProjectBuild.DeprecatedGraphicsAPIFound(GraphicsAPI);

            if (deprecatedGraphicsAPIs.Count == 0)
                return new InspectorValidationResults(true, string.Empty, MessageType.None);

            string deprecatedGraphicsAPINames = string.Join("\n", deprecatedGraphicsAPIs.Select(graphicsAPI => $"* {graphicsAPI.ToString()}"));

            string validationErrorMessage = $"[Deprecated graphics API(s) detected] This configuration contains a deprecated graphic API(s) for build target: " +
                $"{BuildTarget}. \nPlease remove the following graphics API(s) from the graphics API list: \n\n{deprecatedGraphicsAPINames}\n";

            return new InspectorValidationResults(false, validationErrorMessage, MessageType.Warning);
        }
        #endregion
    }
}
