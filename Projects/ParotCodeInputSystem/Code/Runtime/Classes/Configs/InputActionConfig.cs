using UnityEngine;
using UnityEngine.InputSystem;

namespace ParrotCode.InputSystem
{
    [CreateAssetMenu(fileName = "Input Action", menuName = "Sludgy Parrot/Configs/Input Action #I")]
    public sealed class InputActionConfig: ScriptableObject
    {
        [SerializeField]
        private InputActionReference actionReference;

        [SerializeField, Space(5)]
        private InputActionType action;

        public InputActionReference ActionReference => actionReference;
        public InputActionType Action => action;
    }
}
