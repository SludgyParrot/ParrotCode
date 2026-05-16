using System;
using UnityEngine;
using TMPro;

namespace ParrotCode.UI
{
    public class UITextState: UIState
    {
        [SerializeField, Space(5)]
        private TMP_FontAsset stateFontAsset;

        [SerializeField, Space(5)]
        private int fontSize;

        [SerializeField, Space(5)]
        private bool isBold;

        [SerializeField, Space(5)]
        private bool isItalic;

        public TMP_FontAsset StateFontAsset
        {
            get
            {
                if (stateFontAsset == null)
                    throw new NullReferenceException($"StateFontAsset for state id: {StateId} cannot be null for UIState.");
                return stateFontAsset;
            }
        }

        public int FontSize => fontSize;
        public bool IsBold => isBold;
        public bool IsItalic => isItalic;
    }
}
