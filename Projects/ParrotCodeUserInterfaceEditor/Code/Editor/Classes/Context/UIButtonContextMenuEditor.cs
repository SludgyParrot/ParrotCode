using System;
using UnityEngine;
using UnityEditor;
using ParrotCode.UI;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

using UnityEventSystem = UnityEngine.EventSystems.EventSystem;

namespace ParrotCode.UIEditor
{
    public class UIButtonContextMenuEditor: Editor
    {
        private const string ButtonMenuName = EditorSharedStrings.ContextRootMenuName + "UI Button #%B";
        private const int UIMenuPriority = 0;
        private const bool Validate = false;

        private const string UIViewName = "UI View";
        private const string EventSystemName = "UI Event System";
        private const string ButtonTitle = "Button";
        private const string ButtonName = "UI Button";
        private const string ButtonTitleDisplayerName = "Title Displayer";

        private static Vector2 ButtonSize = new Vector2(300.0f, 100.0f);


        [MenuItem(ButtonMenuName, validate = Validate, priority = UIMenuPriority)]
        private static void CreateUIButton(MenuCommand menu)
        {
            GameObject selection = menu.context as GameObject;
            GameObject uiButton;

            if(selection == null | !selection?.GetComponent<Canvas>())
            {
                if(!TryCreateUIView(RenderMode.ScreenSpaceOverlay, out GameObject uiView))
                    throw new InvalidOperationException("[CreateUIButton] [CreateUIView] Operation failed to create UI VIew. UI View is null.");

                if (!TryCreateUIButton(out uiButton))
                    throw new InvalidOperationException("[CreateUIButton] [CreateUIView] Operation failed to create UI button. UI View is null.");

                GameObjectUtility.SetParentAndAlign(uiButton, uiView);
                Undo.RegisterFullObjectHierarchyUndo(uiView, UIViewName);
            }
            else
            {
                if (!TryCreateUIButton(out uiButton))
                    throw new InvalidOperationException("[CreateUIButton] [CreateUIView] Operation failed to create UI button. UI View is null.");

                GameObjectUtility.SetParentAndAlign(uiButton, selection);
                Undo.RegisterFullObjectHierarchyUndo(uiButton, ButtonName);
            }

            Selection.activeObject = uiButton ?? uiButton;
        }

        private static bool TryCreateUIView(RenderMode renderMode, out GameObject uiView)
        {
            uiView = new GameObject(UIViewName);
            UIView canvas = uiView.AddComponent<UIView>();
            canvas.SetRenderMode(renderMode);

            if (TryAddEventSystem(out GameObject eventSystem))
                Undo.RegisterCreatedObjectUndo(eventSystem, EventSystemName);

            return uiView.GetComponent<Canvas>() != null;
        }

        private static bool TryCreateUIButton(out GameObject button)
        {
            // Button
            button = new GameObject(ButtonName);
            UIButton buttonComponent = button.AddComponent<UIButton>();
            button.GetComponent<RectTransform>().sizeDelta = ButtonSize;

            // Text View
            GameObject textView = new GameObject(ButtonTitleDisplayerName);
            TextView textViewComponent = textView.AddComponent<TextView>();
            textViewComponent.SetText(ButtonTitle);
            textViewComponent.SetTextAlignment(TextAlignmentOptions.Center);
            textViewComponent.SetColor(Color.grey);
            buttonComponent.OverrideTitleDisplayer(textViewComponent);
            GameObjectUtility.SetParentAndAlign(textView, button);
            return (button != null) && (textView != null && textView.transform.parent == button.transform);
        }

        private static bool TryAddEventSystem(out GameObject eventSystem)
        {
            var eventSystemComponent = FindAnyObjectByType<UnityEventSystem>();

            if (eventSystemComponent == null)
            {
                eventSystem = new GameObject(EventSystemName);
                eventSystem.AddComponent<UnityEventSystem>();
                eventSystem.AddComponent<StandaloneInputModule>();
            }
            else
                eventSystem = eventSystemComponent.gameObject;
            

            return eventSystem != null;
        }
    }
}
