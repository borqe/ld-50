using System;
using System.Collections;
using ConsoleInput;
using Terminal.Commands;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TerminalWindow : MonoBehaviour
{
    [Header("UI Stuff")]
    [SerializeField] private TMP_InputField _terminalInput;
    [SerializeField] private TMP_Text _consoleOutput;
    [SerializeField] private TMP_Text _terminalHandle;
    [SerializeField] private TMP_Text _terminalUser;
    [SerializeField] private TerminalSettings _settings;
    [SerializeField] private ScrollRect _scrollRect;

    private EventSystem _eventSystem;

    private void Awake()
    {
        _eventSystem = FindObjectOfType<EventSystem>();
        TerminalCommandData.ReadFromFile();
    }

    private void OnEnable()
    {
        _terminalInput.onSubmit.AddListener(OnTerminalSubmit);
        _eventSystem.SetSelectedGameObject(_terminalInput.gameObject);
        
        UpdateUser(_settings.UserString);
        
        GameStateChangedEvent.AddListener(OnGameStateChanged);
    }

    private void OnGameStateChanged(GameStateChangedEvent eventData)
    {
        switch (eventData.State)
        {
            case GameState.GameStateEnum.InTerminalWindow:
                break;
            case GameState.GameStateEnum.InProgress:
                CloseTerminal();
                break;
            case GameState.GameStateEnum.GameOver:
                break;
            default:
                break;
        }
    }
    
    public void ResetPosition()
    {
        StartCoroutine(ResetPositionCoroutine());
    }

    private void OnTerminalSubmit(string arg0)
    {
        HandleCommand(arg0);
    }

    public void MaximizeWindow()
    {
    }

    public void MinimizeWindow()
    {
        
    }

    private void HandleCommand(string command)
    {
        CommandFactory.GetCommand(command, this).Execute();
    }

    public void UpdateUser(string userString)
    {
        _terminalHandle.text = userString + "/ Terminal";
        _terminalUser.text = userString + "~ ";
    }

    private void StartGame()
    {
        Debug.Log("start game");
    }

    private void StartLoginSequence()
    {
        Debug.Log("start login");
    }

    public void CloseTerminal()
    {
        gameObject.SetActive(false);
        // print out the "are you sure", then wait for y/n
    }

    public void OpenTerminal()
    {
        gameObject.SetActive(true);
    }

    public void PrintOutput(string command, string output)
    {
        string originalLine = _consoleOutput.text + _terminalUser.text + command + Environment.NewLine;
        _consoleOutput.text = originalLine + output.Replace("\\n", Environment.NewLine) + Environment.NewLine;
        _terminalInput.text = "";
        ResetPosition();
    }

    public void ClearOutput()
    {
        _consoleOutput.text = "";
        _terminalInput.text = "";
        ResetPosition();
    }

    private IEnumerator ResetPositionCoroutine()
    {
        Canvas.ForceUpdateCanvases();
        yield return null;
        _scrollRect.verticalScrollbar.value = 0f;
        _eventSystem.SetSelectedGameObject(null);
        _eventSystem.SetSelectedGameObject(_terminalInput.gameObject);
    }
}
