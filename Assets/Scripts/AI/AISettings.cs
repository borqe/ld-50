using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AISettings", menuName = "Custom/AISettings")]
public class AISettings : ScriptableObject
{
    public float MinCharge;
    public float MaxCharge;

    public AIState_Idle.Settings StartingSettings;
    public AIState_Charging.Settings ChargingSettings;


}
