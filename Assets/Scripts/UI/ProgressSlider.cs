using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class ProgressSlider : MonoBehaviour
{
    [SerializeField] private Slider _slider; 
    
    private void Awake() 
    {
        if (_slider == null)
        {
            _slider = GetComponent<Slider>();
        }
    }

    private void Start()
    {
        Refresh(0);
    }

    private void OnEnable()
    {
        AIDataTransferedEvent.AddListener(OnAIDataTransfered);
    }

    private void OnDisable()
    {
        AIDataTransferedEvent.RemoveListener(OnAIDataTransfered);
    }

    private void OnAIDataTransfered(AIDataTransferedEvent data)
    {
        Refresh((data.TotalDataTransfered / data.MaxData) * 100f);
    }

    [ExecuteAlways]
    private void Refresh(float value)
    {
        _slider.value = Mathf.Clamp(value, 0.0f, 100.0f);
    }
}
