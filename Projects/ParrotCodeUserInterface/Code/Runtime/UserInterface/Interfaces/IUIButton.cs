using UnityEngine;
using UnityEngine.Localization;

namespace ParrotCode.UI
{
    public interface IUIButton
    {
        void SetBackgroundColor(Color color);
        void SetBackgroundImage(Sprite image);
        void SetTitleText(string text);
        void SetTitleText(LocalizedString text);
        void SetTextColor(Color color);
    }
}
