using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StartUI : MonoBehaviour
{
    [SerializeField] private Transform _buttonParent;
    [SerializeField] private GameObject _buttonPrefab;
    [SerializeField] private List<UnityEvent> buttonEvents;
    [SerializeField] private List<GameObject> _buttons;

    private void Awake()
    {
        _buttons = new List<GameObject>();
        
        foreach (UnityEvent buttonEvent in buttonEvents)
        {
            GameObject button = Instantiate(_buttonPrefab, _buttonParent);
            _buttons.Add(button);

            button.GetComponent<UIButtonActions>()
                .AddOnClickEvent(buttonEvent);
        }
    }

    private void OnEnable()
    {
        GameEventInvoker.onStartGame += OnStartGame;
    }

    private void OnDisable()
    {
        GameEventInvoker.onStartGame -= OnStartGame;
    }

    private void OnStartGame()
    {
        gameObject.SetActive(false);
    }
}
