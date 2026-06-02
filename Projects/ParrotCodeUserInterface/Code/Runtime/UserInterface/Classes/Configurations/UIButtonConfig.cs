using UnityEngine;

namespace ParrotCode.UI
{
    public abstract class UIButtonConfig : ScriptableObject, IUIButtonConfig
    {
        public string Name => name;
    }
}
