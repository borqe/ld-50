using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ConsolePlacerData", menuName = "Custom/ConsolePlacerData")]
public class ConsolePlacerData : ScriptableObject
{
    public Console ConsolePrefab;
    public int AmountOfConsoles;
    public float DistanceFromCenter;
}
