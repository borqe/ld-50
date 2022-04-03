using System;
using System.Linq;
using TMPro;
using UnityEngine;

public class TextProgressSlider : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private float _progress;
    private void OnEnable()
    {
        AIDataTransferedEvent.AddListener(OnAIDataTransferred);
    }

    private void OnDisable()
    {
        AIDataTransferedEvent.RemoveListener(OnAIDataTransferred);
    }

    private void OnAIDataTransferred(AIDataTransferedEvent data)
    {
        _progress = (data.TotalDataTransfered / data.MaxData) * 100.0f;
        Refresh(Mathf.RoundToInt(_progress));
    }

    private void Refresh(int value)
    {
        string percentage = value + "%";
        // Because there can be only 50 symbols in the middle
        string progressString = String.Concat(Enumerable.Repeat("=", value / 2)) + String.Concat(Enumerable.Repeat(" ", 50 - value / 2));
        progressString = progressString.Substring(0, 25 - (percentage.Length / 2)) + percentage +
                         progressString.Substring(25 + (percentage.Length / 2));
        
        
        _text.text = "[" + progressString + "]";
    }
}
