using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CameraSettings", menuName = "Custom/CameraSettings")]

public class CameraSettings : ScriptableObject
{
    public Vector2 CameraSpeed;
    public float TurningRate;

    public float MinXAngle;
    public float MaxXAngle;
}
