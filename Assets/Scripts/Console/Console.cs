using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Console : MonoBehaviour
{
    [SerializeField] private List<ConsoleModule> Modules;

    private void Start()
    {
        AIController.Instance.NewConsoleSpawned(this);
    }

    private void OnDestroy()
    {
        AIController.Instance.RemoveConsole(this);
    }

    public ConsoleModule GetRandomModule()
    {
        return Modules.GetRandomInList();
    }
}
