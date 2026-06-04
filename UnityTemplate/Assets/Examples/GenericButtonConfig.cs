using ParrotCode.UI;
using UnityEngine;

[CreateAssetMenu(fileName = "Button Config", menuName = "Sludgy Parrot/Config/UI/Button Config")]
public sealed class GenericButtonConfig : UIButtonConfig
{
    [SerializeField]
    private string message;

    public string Message => message;
}
