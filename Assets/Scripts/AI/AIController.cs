using System;
using System.Collections;
using System.Collections.Generic;
using AI;
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
    [SerializeField] private Transform _EmojiSpawnTransform;
    public Transform EmojiSpawnTransform => _EmojiSpawnTransform;

    public AIData AIData { get; private set; }
    public List<Cable> ConnectedCables { get; private set; }
    public List<AIState_Charging.ConnectingCable> ConnectingCables { get; private set; }
    public List<Cable> DisconnectedCables { get; private set; }
    public List<Console> Consoles { get; private set; }

    private AIState PreviousState;
    private AIState CurrentState;

    private void Awake()
    {
        AIData = new AIData();
        ConnectedCables = new List<Cable>();
        ConnectingCables = new List<AIState_Charging.ConnectingCable>();
        Consoles = new List<Console>();
        DisconnectedCables = new List<Cable>();
        DisableLogic();
    }

    public void NewConsoleSpawned(Console console)
    {
        Consoles.Add(console);
    }

    public void RemoveConsole(Console console)
    {
        Consoles.Remove(console);
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
                StopAllCoroutines();
                DisableLogic();
                break;
            case GameState.GameStateEnum.GameOver:
                StopAllCoroutines();
                DisableLogic();
                ClearCaches();
                break;
            case GameState.GameStateEnum.GameNotStarted:
                DisableLogic();
                ClearCaches();
                break;
            default:
                Debug.LogError($"Not implemented: {data.State}");
                break;
        }
    }

    private void ClearCaches()
    {
        PreviousState = null;
        CurrentState = null;
        Consoles.Clear();

        while (ConnectedCables.Count != 0)
        {
            var c = ConnectedCables.Last();
            ConnectedCables.Remove(c);
            Destroy(c.gameObject);
        }
        while (ConnectingCables.Count != 0)
        {
            var c = ConnectingCables.Last();
            ConnectingCables.Remove(c);
            Destroy(c.Cable.gameObject);
        } 
        while (DisconnectedCables.Count != 0)
        {
            var c = DisconnectedCables.Last();
            DisconnectedCables.Remove(c);
            Destroy(c.gameObject);
        }
        AIData = new AIData();
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
        if (GameState.Instance.CurrentState == GameState.GameStateEnum.GameOver || GameState.Instance.CurrentState == GameState.GameStateEnum.GameNotStarted)
            return;

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
        DisconnectedCables.Add(cable);
    }
}
