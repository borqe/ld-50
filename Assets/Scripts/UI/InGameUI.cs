using System;
using UnityEngine;

public class InGameUI: MonoBehaviour
{
    [SerializeField] private GameObject _progressSlider;
    [SerializeField] private GameObject _otherStats;
    [SerializeField] private GameObject _statusIndicator;
    [SerializeField] private GameObject _terminalHint;

    private void Start()
    {
        ActivateUI(false);
    }

    private void OnEnable()
    {
        _terminalHint.SetActive(false);
        GameStateChangedEvent.AddListener(OnGameStateChanged);
    }

    private void OnDisable()
    {
        GameStateChangedEvent.RemoveListener(OnGameStateChanged);
    }

    private void OnGameStateChanged(GameStateChangedEvent data)
    {
        switch (data.State)
        {
            case GameState.GameStateEnum.InProgress:
                ActivateUI(true);
                _terminalHint.SetActive(true);
                break;
            case GameState.GameStateEnum.InTerminalWindow:
                ActivateUI(true);
                _terminalHint.SetActive(false);
                break;
            case GameState.GameStateEnum.GameOver:
                ActivateUI(true);
                _terminalHint.SetActive(false);
                break;
            case GameState.GameStateEnum.GameNotStarted:
                ActivateUI(false);
                _terminalHint.SetActive(false);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void ActivateUI(bool active)
    {
        _progressSlider.SetActive(active);
        _otherStats.SetActive(active);
        _statusIndicator.SetActive(active);
    }
}
