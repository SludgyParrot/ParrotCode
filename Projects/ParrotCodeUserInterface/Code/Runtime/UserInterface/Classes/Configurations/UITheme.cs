using System.Collections.Generic;
using UnityEngine;

namespace ParrotCode.UI
{
    [CreateAssetMenu(fileName = "UITheme", menuName = "Parrot Code/UI/Theme")]
    public sealed class UITheme: ScriptableObject
    {
        [SerializeField]
        private List<UIState> states = new List<UIState>();

        public IReadOnlyList<UIState> States
        {
            get
            {
                if(states.Count == 0)
                    Debug.LogError($"States for: {name} cannot be null.");
                return states;
            }
        }
    }
}
