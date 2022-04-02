using System;
using System.Collections;
using ConsoleInput;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ConsoleWindow : MonoBehaviour
{
    [SerializeField] private ConsoleCommands commands;

    [Header("UI Stuff")]
    [SerializeField] private TMP_InputField _consoleInput;
    [SerializeField] private TMP_Text _consoleOutput;
    [SerializeField] private TMP_Text _consoleHandle;
    [SerializeField] private TMP_Text _consoleUser;
    [SerializeField] private ConsoleSettings _settings;
    [SerializeField] private ScrollRect _scrollRect;

    private EventSystem _eventSystem;

    private void Awake()
    {
        _eventSystem = FindObjectOfType<EventSystem>();
    }

    private void OnEnable()
    {
        _consoleInput.onSubmit.AddListener(OnConsoleSubmit);
        _eventSystem.SetSelectedGameObject(_consoleInput.gameObject);
        
        UpdateUser(_settings.UserString);
    }
    
    public void ResetPosition()
    {
        StartCoroutine(ResetPositionCoroutine());
    }

    private void OnConsoleSubmit(string arg0)
    {
        HandleCommand(arg0);
    }

    private void HandleCommand(string command)
    {
        if (commands.commandDictionary.ContainsKey(command))
        {
            switch (command)
            {
                case "clear":
                    ClearOutput();
                    return;
                case "login":
                    StartLoginSequence();
                    return;
                case "start":
                    StartGame();
                    return;
                default:
                    PrintOutput(command, commands.commandDictionary[command]);
                    return;
            }
        }
        else
        {
            PrintOutput(command, commands.commandDictionary["help"]);
        }
    }

    public void UpdateUser(string userString)
    {
        _consoleHandle.text = userString + "/ Terminal";
        _consoleUser.text = userString + "~ ";
    }

    private void StartGame()
    {
        Debug.Log("start game");
    }

    private void StartLoginSequence()
    {
        Debug.Log("start login");
    }

    public void CloseConsole()
    {
        gameObject.SetActive(false);
        // print out the "are you sure", then wait for y/n
    }

    private void PrintOutput(string command, string output)
    {
        _consoleOutput.text = _consoleOutput.text + _consoleUser.text + command + Environment.NewLine + output.Replace("\\n", Environment.NewLine);
        _consoleInput.text = "";
        ResetPosition();
    }

    private void ClearOutput()
    {
        _consoleOutput.text = "";
        _consoleInput.text = "";
        ResetPosition();
    }

    private IEnumerator ResetPositionCoroutine()
    {
        Canvas.ForceUpdateCanvases();
        yield return null;
        _scrollRect.verticalScrollbar.value = 0f;
        _eventSystem.SetSelectedGameObject(null);
        _eventSystem.SetSelectedGameObject(_consoleInput.gameObject);
    }
}
