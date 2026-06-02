using UnityEngine;
using ParrotCode.Native.Common;
using ParrotCode.EventSystem;

namespace ParrotCode.UI
{
    [DisallowMultipleComponent]
    public sealed class UIButtonConfigEventHandler: BaseMonoBehaviour, IConfigurator
    {
        [SerializeField]
        private UIButtonConfig config;

        public void Config()
        {
            if(config == null)
            {
                Log($"Couldn't send config event for '{gameObject.name}'. There is no UI button config assigned in the inspector panel.", LogVerbosity.Warning, LogChannel.UI);
                return;
            }

            EventBus.InvokeEvent(config);
        }
    }
}
