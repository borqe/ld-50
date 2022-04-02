using System;
using UnityEngine;

public class GameState: MonoBehaviour
{
    public static GameStateEnum CurrentState;
    
    private void OnEnable()
    {
        GameEventInvoker.onStartGame += OnStartGame;
        GameEventInvoker.onPauseGame += OnPauseGame;
        GameEventInvoker.onUnpauseGame += OnUnpauseGame;
        GameEventInvoker.onEndGame += OnEndGame;
    }

    private void OnDisable()
    {
        GameEventInvoker.onStartGame -= OnStartGame;
        GameEventInvoker.onPauseGame -= OnPauseGame;
        GameEventInvoker.onUnpauseGame -= OnUnpauseGame;
        GameEventInvoker.onEndGame -= OnEndGame;
    }

    private void OnEndGame()
    {
        CurrentState = GameStateEnum.InMenu;
    }

    private void OnUnpauseGame()
    {
        CurrentState = GameStateEnum.InProgress;
    }

    private void OnPauseGame()
    {
        CurrentState = GameStateEnum.Paused;
    }

    private void OnStartGame()
    {
        CurrentState = GameStateEnum.InProgress;
    }

    public enum GameStateEnum
    {
        InProgress = 0,
        Paused = 1, 
        InMenu = 2
    }
}