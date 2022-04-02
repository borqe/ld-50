using System;
using UnityEngine;

public class UITester : MonoBehaviour
{
    [Range(0, 100)]
    public float progress;
    public static Action<float> onChange;

    private void OnValidate()
    {
        onChange?.Invoke(progress);
    }
}
