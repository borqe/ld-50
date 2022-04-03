using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class ConsoleModule : MonoBehaviour
{
    [SerializeField] private Material ConnectedMaterial;
    [SerializeField] private Material DisconnectedMaterial;

    [SerializeField] private Transform _CableAttachementPosition;
    public Transform CableAttachementPosition => _CableAttachementPosition;

    private MeshRenderer MeshRenderer;

    public Action OnCableInitiallyConnected;
    public Action OnCableDisconnected;

    public CablePlug ConnectedCablePlug { get; private set; }
    private const string CablePlugTag = "CablePlug";
    private bool IsBeingConnectedTo;

    private void Awake()
    {
        MeshRenderer = GetComponent<MeshRenderer>();
        MeshRenderer.material = DisconnectedMaterial;
    }

    public void Disconnected()
    {
        MeshRenderer.material = DisconnectedMaterial;
        ConnectedCablePlug.CancelSnapping();
        OnCableDisconnected?.Invoke();
        IsBeingConnectedTo = false;
    }

    public void InitiallyConnected()
    {
        MeshRenderer.material = ConnectedMaterial;
        ConnectedCablePlug.SnapToOnEndMouseDrag(this);
        OnCableInitiallyConnected?.Invoke();
        IsBeingConnectedTo = true;
    }

    public bool IsConnectedOrBeingConnectedTo()
    {
        return ConnectedCablePlug != null && !IsBeingConnectedTo;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag(CablePlugTag))
            return;

        CablePlug plug = other.gameObject.GetComponent<CablePlug>();
        if (plug == null)
            return;

        if (ConnectedCablePlug != null || !plug.CollisionEnabled)
            return;

        ConnectedCablePlug = plug;
        InitiallyConnected();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag(CablePlugTag))
            return;

        if (ConnectedCablePlug == null)
            return;

        if (other.gameObject == ConnectedCablePlug.gameObject) 
        {
            Disconnected();
            ConnectedCablePlug = null;
        }
    }
}
