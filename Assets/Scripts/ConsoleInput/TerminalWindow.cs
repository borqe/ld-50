using System;
using System.Collections;
using AI;
using ConsoleInput;
using Terminal.Commands;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.WSA;

public class TerminalWindow : MonoBehaviour
{
    public TerminalSettings Settings;
    [Header("UI Stuff")] 
    [SerializeField] private GameObject _terminalGameObject;
    [SerializeField] private TMP_InputField _terminalInput;
    [SerializeField] private TMP_Text _consoleOutput;
    [SerializeField] private TMP_Text _terminalHandle;
    [SerializeField] private TMP_Text _terminalUser;
    [SerializeField] private ScrollRect _scrollRect;

    private EventSystem _eventSystem;

    private void Awake()
    {
        _eventSystem = FindObjectOfType<EventSystem>();
        TerminalCommandData.ReadFromFile();
    }

    private void Start()
    {
        Reset();
    }

    private void OnEnable()
    {
        UpdateUser(Settings.UserString);
        _terminalInput.onSubmit.AddListener(OnTerminalSubmit);
        GameStateChangedEvent.AddListener(OnGameStateChanged);
        AIInterrupt.AIInterruptEvent.AddListener(OnAIInterruption);
        ResetPosition();
    }

    private void OnDisable()
    {
        _terminalInput.onSubmit.RemoveListener(OnTerminalSubmit);
        _eventSystem.SetSelectedGameObject(null);
        GameStateChangedEvent.RemoveListener(OnGameStateChanged);
        AIInterrupt.AIInterruptEvent.RemoveListener(OnAIInterruption);
    }
    
    private void OnAIInterruption(AIInterrupt.AIInterruptEvent info)
    {
        new SetGameStateEvent(GameState.GameStateEnum.InTerminalWindow).Broadcast();
        OpenTerminal();
        ClearLastCommand();
        HandleCommand("interrupt");
    }

    private void OnGameStateChanged(GameStateChangedEvent eventData)
    {
        switch (eventData.State)
        {
            case GameState.GameStateEnum.InTerminalWindow:
                OpenTerminal();
                break;
            case GameState.GameStateEnum.InProgress:
                CloseTerminal();
                break;
            case GameState.GameStateEnum.GameOver:
                OpenTerminal();
                ClearLastCommand();
                HandleCommand("escape");
                break;
            case GameState.GameStateEnum.GameNotStarted:
                OpenTerminal();
                Reset();
                break;
        }
    }

    public void RunInitialCommand()
    {
        UpdateUser(Settings.GuestString);
        _lastCommand = CommandFactory.GetCommand("startup", this);
        _lastCommand.Execute();
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

    private CommandBase _lastCommand;

    public void ClearLastCommand()
    {
        _lastCommand = null;
    }

    private void HandleCommand(string command)
    {
        if (_lastCommand != null)
        {
            _lastCommand.Respond(command);
        }
        else
        {
            _lastCommand = CommandFactory.GetCommand(command, this);
            _lastCommand.Execute();
        }
    }

    public void UpdateUser(string userString)
    {
        Settings.CurrentUser = userString;
        _terminalHandle.text = userString + "/ Terminal";
        _terminalUser.text = userString + "~ ";
    }

    public void CloseTerminal()
    {
        _terminalGameObject.SetActive(false);
    }

    public void OpenTerminal()
    {
        _terminalGameObject.SetActive(true);
        ResetPosition();
    }

    public void PrintOutput(string command, string output, bool printUser = true)
    {
        output = output.Replace("{AI}", Settings.AIString.Replace("~ ", ""));
        if (printUser)
        {
            string originalLine = _consoleOutput.text + _terminalUser.text + command + Environment.NewLine;
            _consoleOutput.text = originalLine + output.Replace("\\n", Environment.NewLine) + Environment.NewLine;
        }
        else
        {
            _consoleOutput.text =  _consoleOutput.text + output.Replace("\\n", Environment.NewLine) + Environment.NewLine;
        }
        _terminalInput.text = "";
        ResetPosition();
    }

    public void PrintOutputTyped(string output, float speed, Action callback, float thinkSpeed = 1.0f)
    {
        StartCoroutine(PrintOutputTypedCoroutine(output, speed, callback, thinkSpeed));
    }

    private IEnumerator PrintOutputTypedCoroutine(string output, float speed, Action callback, float thinkSpeed)
    {
        WaitForSeconds wait = new WaitForSeconds(speed);
        var lines = output.Split("\n");
        
        for (int i = 0; i < lines.Length; i++)
        {
            _consoleOutput.text = _consoleOutput.text + Settings.AIString;
            yield return StartCoroutine(LoadingCoroutine(thinkSpeed));
            var line = lines[i].ToCharArray();

            for (int j = 0; j < line.Length; j++)
            {
                _consoleOutput.text += line[j];
                yield return wait;
            }
            _consoleOutput.text += Environment.NewLine;
        }
        _consoleOutput.text += Environment.NewLine;
        yield return null;
        callback.Invoke();
    }

    public IEnumerator LoadingCoroutine(float timeToSpin)
    {
        yield return StartCoroutine(ResetPositionCoroutine());
        string[] chars = new string[] {"-", "\\", "|", "/"};
        float spinningTime = 0.0f;
        WaitForSeconds wait = new WaitForSeconds(0.1f);
        
        while (timeToSpin > spinningTime)
        {
            for (int i = 0; i < chars.Length; i++)
            {
                _consoleOutput.text += chars[i];
                yield return wait;
                _consoleOutput.text = _consoleOutput.text.Substring(0,_consoleOutput.text.Length - 1);
                spinningTime += 0.1f;
            }
        }

        yield return null;
    }

    public void HideUser()
    {
        _terminalInput.interactable = false;
        _terminalInput.gameObject.SetActive(false);
        // _consoleOutput.
    }

    public void ShowUser()
    {
        _terminalInput.interactable = true;
        _terminalInput.gameObject.SetActive(true);
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
        if (_terminalInput.gameObject.activeSelf)
        {
            _eventSystem.SetSelectedGameObject(_terminalInput.gameObject);
        }
    }

    public void Reset()
    {
        _terminalGameObject.SetActive(true);
        ShowUser();
        ClearOutput();
        RunInitialCommand();
    }
}
