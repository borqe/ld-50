using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CableManager : Singleton<CableManager>
{
    [SerializeField] private Cable CablePrefab;

    public Cable SpawnCable(Vector3 start, Vector3 end)
    { 
        Cable c = Instantiate(CablePrefab, transform);
        c.Setup(start, end);
        return c;
    }
}
