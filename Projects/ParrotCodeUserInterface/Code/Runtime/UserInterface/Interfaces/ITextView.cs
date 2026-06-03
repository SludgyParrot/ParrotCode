using UnityEngine.Localization;
using UnityEngine;
using TMPro;

namespace ParrotCode.UI
{
    public interface ITextView
    {
        void SetText(string text);
        void SetText(LocalizedString localizedText);
        void SetColor(Color color);
        void SetTextAlignment(TextAlignmentOptions alignmentOptions);
    }
}
