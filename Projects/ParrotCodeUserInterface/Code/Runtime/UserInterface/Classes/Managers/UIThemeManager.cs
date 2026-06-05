using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ParrotCode.Native.Common;
using ParrotCode.EventSystem;
using ParrotCode.Native.Inspector;

namespace ParrotCode.UI
{
    public sealed class UIThemeManager: SingletonInstance<UIThemeManager>
    {
        [SerializeField, Space(5)]
        private List<UITheme> themes = new List<UITheme>();

        [field: SerializeField, Space(5)]
        public Theme SelectedTheme {  get; private set; }

        public IReadOnlyList<UITheme> Themes => themes;

        protected override void Init()
        {
            if(themes?.Count == 0)
            {
                Log($"There are no themes assigned to '{gameObject.name}'.", LogVerbosity.Error, LogChannel.UI);
                return;
            }

            ChangeTheme();
        }

        [Button]
        private void ChangeTheme()
            => ChangeTheme(SelectedTheme);

        public void ChangeTheme(Theme selectedTheme)
        {
            UITheme theme = Themes.FirstOrDefault(theme => theme.Type.Equals(selectedTheme));

            if (theme == null)
            {
                Log($"[{gameObject.name}] Change theme failed. Couldn't find theme of type: {selectedTheme}.", LogVerbosity.Error, LogChannel.UI);
                return;
            }

            SelectedTheme = selectedTheme;
            EventBus.InvokeEvent(theme);
        }
    }
}
