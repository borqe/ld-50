using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsoleModule : MonoBehaviour
{
    [SerializeField] private Transform _CableAttachementPosition;
    public Transform CableAttachementPosition => _CableAttachementPosition;
}
