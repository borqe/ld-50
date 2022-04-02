using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIData
{
    public float DataTransfered;
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

    public Action OnDataTransfteredChanged;

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
        GameEventInvoker.onStartGame += OnStartGame;
        GameEventInvoker.onPauseGame += OnPauseGame;
        GameEventInvoker.onUnpauseGame += OnUnpasueGame;
        GameEventInvoker.onEndGame += OnEndGame;
    }

    private void OnDisable()
    {
        GameEventInvoker.onStartGame -= OnStartGame;
        GameEventInvoker.onPauseGame -= OnPauseGame;
        GameEventInvoker.onUnpauseGame -= OnUnpasueGame;
        GameEventInvoker.onEndGame -= OnEndGame;
    }

    private void OnStartGame()
    {
        EnableLogic();
    }

    private void OnEndGame()
    {
        DisableLogic();
    }

    private void OnPauseGame()
    {
        DisableLogic();
    }

    private void OnUnpasueGame()
    {
        EnableLogic();
    }

    private void EnableLogic()
    {
        CurrentState = PreviousState != null ? PreviousState : new AIState_Starting(this);
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

    public void AddDataTransfer(float value)
    {
        AIData.DataTransfered += value;
        Debug.LogError(AIData.DataTransfered);
        OnDataTransfteredChanged?.Invoke();
    }
}
