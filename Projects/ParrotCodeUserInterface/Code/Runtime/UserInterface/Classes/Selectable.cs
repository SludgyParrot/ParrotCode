using ParrotCode.Native.Common;
using UnityEngine;

namespace ParrotCode.UI
{
    /// <summary>
    /// Base class for selectable UI components.
    /// </summary>
    public abstract class Selectable : BaseMonoBehaviour, ISelectable
    {
        [SerializeField, Space(5)]
        private Navigation navigation;

        [SerializeField, Space(5)]
        private Selectable onSelectionUp,
                           onSelectionDown,
                           onSelectionLeft,
                           onSelectionRight;

        public Navigation Navigation => navigation;

        public Selectable OnSelectionUp => onSelectionUp;
        public Selectable OnSelectionDown => onSelectionDown;
        public Selectable OnSelectionLeft => onSelectionLeft;
        public Selectable OnSelectionRight => onSelectionRight;

        /// <summary>
        /// This function is triggered during a selection.
        /// </summary>
        public abstract void Select();
    }
}
