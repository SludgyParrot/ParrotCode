using UnityEngine;
using UnityEditor;
using System;

namespace ParrotCode.UI.Inspector
{
    public static class UIComponentEditor
    {
        private static Vector2 ButtonSize = new Vector2(300.0f, 100.0f);

        [MenuItem("Sludgy Parrot/Create/UI/Canvas/Screen Space")]
        private static void CreateScreenSpaceCanvas()
            => CreateUICanvas();

        [MenuItem("Sludgy Parrot/Create/UI/Canvas/World Space")]
        private static void CreateWorldSpaceCanvas()
            => CreateUICanvas(worldSpace: true);

        [MenuItem("Sludgy Parrot/Create/UI/Button")]
        private static void CreateUIButton()
        {
            var button = new GameObject("UI Button").AddComponent<UIButton>();
            button.GetComponent<RectTransform>().sizeDelta = ButtonSize;
            button.InitializeButton();

            Transform buttonParrent = null;

            if (Selection.activeGameObject?.transform?.root?.GetComponent<Canvas>())
                buttonParrent = Selection.activeGameObject.transform;

            if (buttonParrent == null)
                CreateUICanvas(createdCanvasCallback: canvas => { canvas?.Add(button); });
            else
                button.transform.SetParent(buttonParrent, false);

            Selection.activeGameObject = button.gameObject;
        }

        private static void CreateUICanvas(string label = "__UI Root", bool worldSpace =  false, Action<UICanvas> createdCanvasCallback = null)
        {
            string formattedLabel = worldSpace? $"{label} [World Space]": $"{label} [Screen Space]";
            UICanvas canvas = new GameObject(formattedLabel).AddComponent<UICanvas>();
            canvas.Value.renderMode = worldSpace? RenderMode.WorldSpace: RenderMode.ScreenSpaceOverlay;
            createdCanvasCallback?.Invoke(canvas);
        }
    }
}
