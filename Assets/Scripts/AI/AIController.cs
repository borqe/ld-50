using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIData
{
    public float DataTransfered;
}

public enum AIStateType
{ 
    Idle,
    TransferingData,
}

public class AIController : Singleton<AIController>
{
    [SerializeField] private AISettings AISettings;
    public AISettings Settings => AISettings;

    public AIData AIData { get; private set; }
    public List<Cable> ConnectedCables { get; private set; }
    public List<Console> Consoles { get; private set; }

    private AIState PreviousState;
    private AIState CurrentState;

    public Action<float> OnDataTransferedChanged;
    public Action OnDataTransferFull;
    public Action<AIStateType> OnAIStateChanged;

    private void Awake()
    {
        AIData = new AIData();
        ConnectedCables = new List<Cable>();
        Consoles = new List<Console>();
        DisableLogic();
    }

    public void NewConsoleSpawned(Console console)
    {
        Consoles.Add(console);
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
        switch (data.State)
        {
            case GameState.GameStateEnum.InProgress:
                EnableLogic();
                break;
            case GameState.GameStateEnum.InTerminalWindow:
                DisableLogic();
                break;
            case GameState.GameStateEnum.GameOver:
                DisableLogic();
                break;
            default:
                break;
        }
    }

    private void EnableLogic()
    {
        CurrentState = PreviousState != null ? PreviousState : new AIState_Charging(this);
    }

    private void DisableLogic()
    {
        PreviousState = CurrentState;
        CurrentState = null;
    }

    private void Update()
    {
        if (CurrentState != null)
            CurrentState.Update();
    }

    public void SetNewState(AIState state)
    {
        CurrentState = state;
    }

    public int GetFullyConenctedCableCount()
    {
        int count = 0;
        for (int i = 0; i < ConnectedCables.Count; i++)
        {
            if (ConnectedCables[i].IsFullyConnected)
                count++;
        }
        return count;
    }

    public void AddDataTransfer(float value)
    {
        AIData.DataTransfered += value;
        OnDataTransferedChanged?.Invoke(AIData.DataTransfered / 10f);

        if (AIData.DataTransfered >= Settings.MaxCharge)
            GameOver();
    }

    public void GameOver()
    {
        OnDataTransferFull?.Invoke();
        CurrentState = new AIState_Idle(this, supressEvent: true);
    }

    public void CableDisconnected(Cable cable)
    {
        ConnectedCables.Remove(cable);
    }
}
