using UnityEngine;
using ParrotCode.Native.Common;
using UnityEngine.Localization;

public class TestsScript : BaseMonoBehaviour
{
    [SerializeField]
   private LocalizedString localizedString;

    private void Init()
    {
        string value = localizedString.GetLocalizedString();
    }
}
