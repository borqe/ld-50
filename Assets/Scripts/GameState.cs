using System;
using UnityEngine;

public class SetGameStateEvent : Event<SetGameStateEvent>
{
    public GameState.GameStateEnum NewState;

    public SetGameStateEvent(GameState.GameStateEnum state)
    {
        NewState = state;
    }
}

public class GameStateChangedEvent : Event<GameStateChangedEvent>
{
    public GameState.GameStateEnum PreviousState;
    public GameState.GameStateEnum State;

    public GameStateChangedEvent(GameState.GameStateEnum previousState, GameState.GameStateEnum state)
    {
        PreviousState = previousState;
        State = state;
    }
}

public class GameState: Singleton<GameState>
{
    public GameStateEnum CurrentState;
    
    private void OnEnable()
    {
        SetGameStateEvent.AddListener(OnSetGameStateEvent);
    }

    private void OnDisable()
    {
        SetGameStateEvent.RemoveListener(OnSetGameStateEvent);
    }

    private void OnSetGameStateEvent(SetGameStateEvent data)
    {
        var prevState = CurrentState;
        CurrentState = data.NewState;
        new GameStateChangedEvent(prevState, CurrentState).Broadcast();
    }

    public enum GameStateEnum
    {
        InProgress = 0,
        InTerminalWindow = 1, 
        GameOver = 2,
        GameNotStarted = 3,
    }
}