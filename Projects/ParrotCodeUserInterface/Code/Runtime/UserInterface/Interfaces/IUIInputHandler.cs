
using System;

namespace ParrotCode.UI
{
    public interface IUIInputHandler
    {
        void AddListener(params Action<UIStateType>[] listeners);
        void RemoveListener(params Action<UIStateType>[] listeners);
    }
}
