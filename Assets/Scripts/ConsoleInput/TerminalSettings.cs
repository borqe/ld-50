using UnityEngine;

[CreateAssetMenu(fileName = "TerminalSettings", menuName = "TerminalSettings", order = 0)]
public class TerminalSettings : ScriptableObject
{
    public string UserString;
    public string GuestString;
    public string AIString;
    public string CurrentUser;
}