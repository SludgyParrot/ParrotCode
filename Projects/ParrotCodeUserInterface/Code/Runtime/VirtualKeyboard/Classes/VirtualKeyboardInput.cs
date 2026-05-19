using System;
using UnityEngine;
using ParrotCode.Native.Common;

namespace ParrotCode.UI.Keyboard
{
    [DisallowMultipleComponent]
    public sealed class VirtualKeyboardInput : BaseMonoBehavior, IVirtualKeyboardInput
    {
        [SerializeField, Space(5)]
        private string primaryKeyValue, 
                       secondaryKeyValue;

        public void Config(IVirtualKeyboard keyboard)
        {
            
        }
    }
}
