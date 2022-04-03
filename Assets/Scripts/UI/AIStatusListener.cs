using TMPro;
using UnityEngine;

public class AIStatusListener : MonoBehaviour
{
    [SerializeField] private TMP_Text _statusText;
    [SerializeField] private TerminalSettings _terminalSettings;
    
    private void OnEnable()
    {
        AIStateChangedEvent.AddListener(OnAIStateChange);
    }
    
    private void OnDisable()
    {
        AIStateChangedEvent.RemoveListener(OnAIStateChange);
    }

    private void OnAIStateChange(AIStateChangedEvent info)
    {
        _statusText.text = "[" + _terminalSettings.AIString.Replace("~ ", "") + "] STATUS: " + info.State.ToString().ToUpper();
    }
}
