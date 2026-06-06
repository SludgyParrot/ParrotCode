using System.Collections.Generic;
using UnityEngine;

namespace ParrotCode.InputSystem
{
    [CreateAssetMenu(fileName = "Input Action Scheme", menuName = "Sludgy Parrot/Configs/Input Action Scheme")]
    public sealed class InputActionScheme: ScriptableObject
    {
        [SerializeField]
        private InputScheme scheme;

        [SerializeField, Space(5)]
        private List<InputActionConfig> inputActionConfigs = new List<InputActionConfig>();

        public InputScheme Scheme => scheme;
        public IReadOnlyList<InputActionConfig> InputActionConfigs => inputActionConfigs;
    }
}
