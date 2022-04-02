using UnityEngine;

public class InGameUI: MonoBehaviour
{
    [SerializeField] private GameObject _progressSlider;

    private void Start()
    {
        _progressSlider.SetActive(false);
    }

    private void OnEnable()
    {
        GameEventInvoker.onStartGame += OnGameStart;
    }

    private void OnDisable()
    {
        GameEventInvoker.onStartGame += OnGameStart;
    }
    
    private void OnGameStart()
    {
        _progressSlider.SetActive(true);
    }
}