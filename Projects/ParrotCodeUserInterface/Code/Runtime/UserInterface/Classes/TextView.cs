using System;
using UnityEngine.Localization;
using ParrotCode.Native.Common;
using UnityEngine;
using TMPro;

namespace ParrotCode.UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    [DisallowMultipleComponent]
    public sealed class TextView : BaseMonoBehaviour, ITextView, IDisposable
    {
        private TextMeshProUGUI textRenderer;

        public TextMeshProUGUI TextRenderer
        {
            get
            {
                if (textRenderer == null)
                    textRenderer = GetComponent<TextMeshProUGUI>();
                return textRenderer;
            }
        }

        public void SetColor(Color color)
            => TextRenderer.color = color;

        public void SetText(string text)
            => TextRenderer.text = text;

        public void SetText(LocalizedString localizedText)
        {
            // Add reference to UnityEngine.UIElementsModule
            //TextRenderer.SetText(localizedText.GetLocalizedString());
        }

        public void Dispose()
        {

        }
    }
}
