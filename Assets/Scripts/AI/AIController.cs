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
    Confused,
    Scared,
}

public class AIStateChangedEvent : Event<AIStateChangedEvent>
{
    public AIStateType State;

    public AIStateChangedEvent(AIStateType state)
    {
        State = state;
    }
}

public class SetAIStateEvent : Event<SetAIStateEvent>
{
    public AIStateType NewState;

    public SetAIStateEvent(AIStateType newState)
    {
        NewState = newState;
    }
}

public class AIDataTransferedEvent : Event<AIDataTransferedEvent>
{
    public float TotalDataTransfered;
    public float MaxData;

    public AIDataTransferedEvent(float totalDataTransfered, float maxData)
    {
        TotalDataTransfered = totalDataTransfered;
        MaxData = maxData;
    }
}

public class AIController : Singleton<AIController>
{
    [SerializeField] private AISettings AISettings;
    public AISettings Settings => AISettings;

    public AIData AIData { get; private set; }
    public List<Cable> ConnectedCables { get; private set; }
    public List<AIState_Charging.ConnectingCable> ConnectingCables { get; private set; }
    public List<Console> Consoles { get; private set; }

    private AIState PreviousState;
    private AIState CurrentState;

    private void Awake()
    {
        AIData = new AIData();
        ConnectedCables = new List<Cable>();
        ConnectingCables = new List<AIState_Charging.ConnectingCable>();
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
        SetAIStateEvent.AddListener(OnSetAIState);
    }

    private void OnDisable()
    {
        GameStateChangedEvent.RemoveListener(OnGameStateChanged);
        SetAIStateEvent.RemoveListener(OnSetAIState);
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
                Debug.LogError($"Not implemented: {data.State}");
                break;
        }
    }

    private void OnSetAIState(SetAIStateEvent data)
    {
        switch (data.NewState)
        {
            case AIStateType.Idle:
                CurrentState = new AIState_Idle(this);
                break;
            case AIStateType.TransferingData:
                CurrentState = new AIState_Charging(this);
                break;
            case AIStateType.Confused:
                CurrentState = new AIState_Confused(this, AIStateType.TransferingData);
                break;
            case AIStateType.Scared:
                CurrentState = new AIState_Scared(this);
                break;
            default:
                Debug.LogError($"Not implemented: {data.NewState}");
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
        new AIDataTransferedEvent(AIData.DataTransfered, Settings.MaxCharge).Broadcast();

        if (AIData.DataTransfered >= Settings.MaxCharge)
            GameOver();
    }

    public void GameOver()
    {
        new SetGameStateEvent(GameState.GameStateEnum.GameOver).Broadcast();
        CurrentState = new AIState_Idle(this, supressEvent: true);
    }

    public void CableDisconnected(Cable cable)
    {
        ConnectedCables.Remove(cable);
    }
}
