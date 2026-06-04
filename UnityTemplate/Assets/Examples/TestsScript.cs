using UnityEngine;
using ParrotCode.Native.Common;
using UnityEngine.Localization;
using UnityEngine.EventSystems;

public class TestsScript : BaseMonoBehaviour
{
    [SerializeField]
   private LocalizedString localizedString;

    [SerializeField]
    private EventSystem eventSystem;

    private void Init()
    {
        string value = localizedString.GetLocalizedString();
    }
}
