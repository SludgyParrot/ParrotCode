using System;
using UnityEngine;
using UnityEditor;
using ParrotCode.UI;

namespace ParrotCode.UIEditor
{
    public class UIButtonContextMenu: Editor
    {
        private const string ButtonMenuName = EditorSharedStrings.ContextRootMenuName + "UI Button";
        private const int UIMenuPriority = 0;
        private const bool Validate = false;


        [MenuItem(ButtonMenuName, validate = Validate, priority = UIMenuPriority)]
        private static void CreateUIButton(MenuCommand menu)
        {
            GameObject parent = menu.context as GameObject;

            if(parent == null)
            {
                Debug.Log("~There's no selected game object.");
            }
            else
            {
                Debug.Log($"~Selected parent game object called: {parent.name}.");
            }
        }

    }

}
