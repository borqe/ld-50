using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class UITester : MonoBehaviour
{
#if UNITY_EDITOR
    [Range(0, 100)]
    public float progress;
    public static Action<float> onChange;

    [SerializeField] private List<ConsoleModule> _consoleModules = new List<ConsoleModule>();

    private void OnValidate()
    {
        onChange?.Invoke(progress);
    }
    
    private float _timer = 0.0f;
    private bool _gameInProgress = false;

    private void Start()
    {
        _consoleModules = FindObjectsOfType<ConsoleModule>().ToList();
        GameEventInvoker.onStartGame += OnStartGame;
        GameEventInvoker.onEndGame += OnEndGame;
    }

    private void OnEndGame()
    {
        _gameInProgress = false;
    }

    private void OnStartGame()
    {
        _gameInProgress = true;
    }
    
#endif // UNITY_EDITOR
}
