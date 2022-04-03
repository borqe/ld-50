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

    public ConsoleModule GetRandomConsoleNotInUse()
    {
        List<ConsoleModule> notInUse = new List<ConsoleModule>();
        for (int i = 0; i < Modules.Count; i++)
        {
            var m = Modules[i];
            if (m.gameObject.activeSelf && !m.IsConnectedOrBeingConnectedTo())
                notInUse.Add(m);
        }
        if (notInUse.Count == 0)
            return null;

        return notInUse.GetRandomInList();
    }

    public bool HasFreeModules()
    {
        for (int i = 0; i < Modules.Count; i++)
        {
            var m = Modules[i];
            if (m.gameObject.activeSelf && !m.IsConnectedOrBeingConnectedTo())
                return true;
        }
        return false;
    }

    public void Init(float amountOfModulesToRemove)
    {
        int modulesToRemoveCount = Mathf.FloorToInt(Modules.Count * amountOfModulesToRemove);
        for (int i = 0; i < modulesToRemoveCount; i++)
        {
            var m = Modules.GetRandomInList();
            Modules.Remove(m);
            m.gameObject.SetActive(false);
        }
    }
}
