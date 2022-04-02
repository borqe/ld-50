using UnityEngine;

public class StartUI : MonoBehaviour
{
    private void OnEnable()
    {
        GameEventInvoker.onStartGame += OnStartGame;
    }

    private void OnDisable()
    {
        GameEventInvoker.onStartGame -= OnStartGame;
    }

    private void OnStartGame()
    {
        gameObject.SetActive(false);
    }
}
