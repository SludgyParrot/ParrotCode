using UnityEngine.Localization;
using UnityEngine;

namespace ParrotCode.UI
{
    public interface ITextView
    {
        void SetText(string text);
        void SetText(LocalizedString localizedText);
        void SetColor(Color color);
    }
}
