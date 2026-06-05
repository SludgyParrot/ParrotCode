
using System;

namespace ParrotCode.UI
{
    public interface IUIInputHandler
    {
        void AddListener(params Action<State>[] listeners);
        void RemoveListener(params Action<State>[] listeners);
    }
}
