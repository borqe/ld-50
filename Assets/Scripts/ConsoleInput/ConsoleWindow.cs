using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ConsoleWindow : MonoBehaviour
{
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

    private void OnConsoleSubmit(string arg0)
    {
        HandleCommand(arg0);
    }

    private void HandleCommand(string command)
    {
        StartCoroutine(PrintOutput(command));
    }

    private IEnumerator PrintOutput(string output)
    {
        _consoleOutput.text = _consoleOutput.text + Environment.NewLine + output;
        _consoleInput.text = "";
        Canvas.ForceUpdateCanvases();
        yield return null;
        _scrollRect.verticalScrollbar.value = 0f;
        _eventSystem.SetSelectedGameObject(null);
        _eventSystem.SetSelectedGameObject(_consoleInput.gameObject);
    }
}
