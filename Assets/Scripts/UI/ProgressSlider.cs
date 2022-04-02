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

    private void OnEnable()
    {
        AIController.Instance.OnDataTransferedChanged += Refresh;
    }

    private void OnDisable()
    {
        AIController.Instance.OnDataTransferedChanged -= Refresh;
    }

    [ExecuteAlways]
    private void Refresh(float value)
    {
        _slider.value = Mathf.Clamp(value, 0.0f, 100.0f);
    }
}
