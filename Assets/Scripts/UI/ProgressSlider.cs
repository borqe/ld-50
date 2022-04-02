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
        UITester.onChange += Refresh;
    }

    private void OnDisable()
    {
        UITester.onChange -= Refresh;
    }

    private void Refresh(float value)
    {
        _slider.value = Mathf.Clamp(value, 0.0f, 100.0f);
    }
}
