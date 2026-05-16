using ParrotCode.Native.Common;
using System;
using TMPro;
using UnityEngine;

namespace ParrotCode.UI
{
    [RequireComponent(typeof(TMP_Text))]
    public class UIText : UIStateComponent<TMP_Text, UITextState>
    {
        public override void SetColor(Color color)
            => Value.color = color;

        public void SetText(string text)
            => Value.text = text;

        public override void SetUIState(UIStateType stateType, Action<(UIState state, string errorMessage)> actionCallback = null)
        {
            var eventStateResults = GetUIStateOfType(stateType);

            if (!string.IsNullOrEmpty(eventStateResults.errorMessage))
            {
                Log(eventStateResults.errorMessage, LogVerbosity.Error, LogChannel.InputSystem);
                return;
            }

            var state = eventStateResults.state;
            SetColor(state.Color);

            if (state.UseOptionals)
            {
                Value.font = state.StateFontAsset;
                Value.fontSize = state.FontSize;
                Value.fontStyle = state.IsBold? FontStyles.Bold : FontStyles.Normal;

                if(!state.IsItalic && !state.IsBold)
                    Value.fontStyle = FontStyles.Normal;
                else
                {
                    if (state.IsItalic)
                        Value.fontStyle = FontStyles.Italic;

                    if (state.IsBold)
                        Value.fontStyle = FontStyles.Bold;
                }
            }
        }
    }
}
