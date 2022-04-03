using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class ConsoleModule : MonoBehaviour
{
    [Serializable]
    public struct MaterialData
    {
        public Material Material;
        public int MaterialIndexInMesh;
        public Color LightColor;
        public float LightIntensity;
        public float LightRange;
    }

    [Serializable]
    public struct MaterialAnimationData
    {
        public MaterialData ConnectedMaterial;
        public MaterialData DisconnectedMaterial;
    }

    [SerializeField] private MaterialAnimationData MaterialAnimations;

    [SerializeField] private Light Light;
    [SerializeField] private Light LightSpot;
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
        AssignMaterialData(MaterialAnimations.ConnectedMaterial);
        AssignMaterialData(MaterialAnimations.DisconnectedMaterial);
    }

    public void Disconnected()
    {
        AssignMaterialData(MaterialAnimations.DisconnectedMaterial);
        ConnectedCablePlug.CancelSnapping();
        OnCableDisconnected?.Invoke();
        IsBeingConnectedTo = false;
    }

    public void InitiallyConnected()
    {
        AssignMaterialData(MaterialAnimations.ConnectedMaterial);
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

    private void AssignMaterialData(MaterialData data)
    {
        var mats = MeshRenderer.materials;
        mats[data.MaterialIndexInMesh] = data.Material;
        MeshRenderer.materials = mats;
        Light.color = data.LightColor;
        Light.intensity = data.LightIntensity;
        Light.range = data.LightRange;
        LightSpot.color = data.LightColor;
    }
}
