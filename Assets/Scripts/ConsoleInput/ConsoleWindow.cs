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

    private void PrintOutput(string command, string output)
    {
        _consoleOutput.text = _consoleOutput.text + Environment.NewLine + command + Environment.NewLine + output.Replace("\\n", Environment.NewLine);
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
