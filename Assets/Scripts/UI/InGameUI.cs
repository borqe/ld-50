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
        GameEventInvoker.onEndGame += OnGameEnd;
    }

    private void OnDisable()
    {
        GameEventInvoker.onStartGame -= OnGameStart;
        GameEventInvoker.onEndGame -= OnGameEnd;
    }

    private void OnGameEnd()
    {
        _progressSlider.SetActive(false);
    }

    private void OnGameStart()
    {
        _progressSlider.SetActive(true);
    }
}
