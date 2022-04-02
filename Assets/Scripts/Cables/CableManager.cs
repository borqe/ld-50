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
            SpawnCable(AI.transform.position, module.CableAttachementPosition.position);
        }
    }

    public Cable SpawnCable(Vector3 start, Vector3 end)
    { 
        Cable c = Instantiate(CablePrefab, transform);
        c.Setup(start, end);
        return c;
    }
}
