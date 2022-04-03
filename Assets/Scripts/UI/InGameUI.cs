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
        GameStateChangedEvent.AddListener(OnGameStateChanged);
    }

    private void OnDisable()
    {
        GameStateChangedEvent.RemoveListener(OnGameStateChanged);
    }

    private void OnGameStateChanged(GameStateChangedEvent data)
    {
        if (data.State == GameState.GameStateEnum.InTerminalWindow)
            ActivateProgressSlider(false);
        else if (data.State == GameState.GameStateEnum.InProgress)
            ActivateProgressSlider(true);
    }


    private void ActivateProgressSlider(bool active)
    {
        _progressSlider.SetActive(active);
    }
}
