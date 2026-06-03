using System;
using UnityEngine;
using UnityEditor;
using ParrotCode.UI;
using UnityEngine.UI;
using TMPro;

namespace ParrotCode.UIEditor
{
    public class UIButtonContextMenu: Editor
    {
        private const string ButtonMenuName = EditorSharedStrings.ContextRootMenuName + "UI Button";
        private const int UIMenuPriority = 0;
        private const bool Validate = false;

        private static Vector2 ButtonSize = new Vector2(300.0f, 100.0f);


        [MenuItem(ButtonMenuName, validate = Validate, priority = UIMenuPriority)]
        private static void CreateUIButton(MenuCommand menu)
        {
            GameObject selection = menu.context as GameObject;
            GameObject uiButton;

            if(selection == null)
            {
                if(!TryCreateUIView(RenderMode.ScreenSpaceOverlay, out GameObject uiView))
                    throw new InvalidOperationException("[CreateUIButton] [CreateUIView] Operation failed to create UI VIew. UI View is null.");

                if (!TryCreateUIButton(out uiButton))
                    throw new InvalidOperationException("[CreateUIButton] [CreateUIView] Operation failed to create UI button. UI View is null.");

                GameObjectUtility.SetParentAndAlign(uiButton, uiView);
                Undo.RegisterFullObjectHierarchyUndo(uiView, "UI View");
            }
            else
            {
                if (selection.GetComponentInParent<Canvas>())
                {
                    if (!TryCreateUIButton(out uiButton))
                        throw new InvalidOperationException("[CreateUIButton] [CreateUIView] Operation failed to create UI button. UI View is null.");


                    GameObjectUtility.SetParentAndAlign(uiButton, selection);
                    Undo.RegisterFullObjectHierarchyUndo(uiButton, "UI Button");
                }
                else
                {
                    if (!TryCreateUIView(RenderMode.ScreenSpaceOverlay, out GameObject uiView))
                        throw new InvalidOperationException("[CreateUIButton] [CreateUIView] Operation failed to create UI VIew. UI View is null.");

                    if (!TryCreateUIButton(out uiButton))
                        throw new InvalidOperationException("[CreateUIButton] [CreateUIView] Operation failed to create UI button. UI View is null.");

                    GameObjectUtility.SetParentAndAlign(uiButton, uiView);
                    GameObjectUtility.SetParentAndAlign(uiView, selection);
                    Undo.RegisterFullObjectHierarchyUndo(uiView, "UI View");
                }
            }

            Selection.activeObject = uiButton ?? uiButton;
        }

        private static bool TryCreateUIView(RenderMode renderMode, out GameObject uiView)
        {
            uiView = new GameObject("UI View");
            Canvas canvas = uiView.AddComponent<Canvas>();
            uiView.AddComponent<CanvasScaler>();
            canvas.renderMode = renderMode;
            return uiView != null && (canvas != null && canvas.renderMode == renderMode);
        }

        private static bool TryCreateUIButton(out GameObject button)
        {
            // Button
            button = new GameObject("UI Button");
            UIButton buttonComponent = button.AddComponent<UIButton>();
            button.GetComponent<RectTransform>().sizeDelta = ButtonSize;

            // Text View
            GameObject textView = new GameObject("Title Displayer");
            TextView textViewComponent = textView.AddComponent<TextView>();
            textViewComponent.SetText("Button");
            textViewComponent.SetTextAlignment(TextAlignmentOptions.Center);
            textViewComponent.SetColor(Color.grey);
            buttonComponent.OverrideTitleDisplayer(textViewComponent);
            GameObjectUtility.SetParentAndAlign(textView, button);
            return (button != null) && (textView != null && textView.transform.parent == button.transform);
        }
    }
}
