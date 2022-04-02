using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CableManager : Singleton<CableManager>
{
    [SerializeField] private Cable CablePrefab;

    [SerializeField] private AIController AI;
    [SerializeField] private List<ConsoleModule> StartConnections;

    private void Start()
    {
        foreach (var module in StartConnections)
        {
            Instantiate(CablePrefab, transform).Setup(AI.transform, module.CableAttachementPosition);
        }
    }
}
