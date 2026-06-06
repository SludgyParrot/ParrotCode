using UnityEngine.InputSystem;

namespace ParrotCode.InputSystem
{ 
    public class InputActionEvent
    {
        private readonly InputScheme scheme;
        private readonly Action action;
        private readonly InputAction.CallbackContext callback;
        private readonly bool performed;

        public InputScheme Sheme => scheme;
        public Action Action => action;
        public InputAction.CallbackContext Callback => callback;
        public bool Performed => performed;

        public InputActionEvent(InputScheme scheme, Action action, InputAction.CallbackContext callback, bool performed)
        {
            this.scheme = scheme;
            this.action = action;
            this.callback = callback;
            this.performed = performed;
        }
    }
}
