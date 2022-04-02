using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GameEventInvoker", menuName = "ScriptableObjects/GameEventInvoker", order = 1)] 
public class GameEventInvoker: ScriptableObject
{
    public static Action onStartGame;
    public static Action onPauseGame;
    public static Action onUnpauseGame;
    public static Action onEndGame;
    
    public void StartGame()
    {
        onStartGame?.Invoke();
    }

    public void PauseGame()
    {
        onPauseGame?.Invoke();
    }

    public void UnpauseGame()
    {
        onUnpauseGame?.Invoke();
    }

    public void EndGame()
    {
        onEndGame?.Invoke();
    }
}