using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Cable : MonoBehaviour
{
    private LineRenderer LineRenderer;

    [SerializeField] private CablePlug StartPlug;
    [SerializeField] private CablePlug EndPlug;

    public bool IsFullyConnected { get; private set; }

    private void Awake()
    {
        LineRenderer = GetComponent<LineRenderer>();
        transform.position = Vector3.zero;
    }

    public void Setup(Vector3 start, Vector3 end)
    {
        StartPlug.Setup(start, false, null);
        EndPlug.Setup(end, true, (plug) => OnPlugPosChanged(1, plug));
        Vector3[] positions = new Vector3[] {start, end};

        LineRenderer.SetPositions(positions);
    }

    public CablePlug GetPlug(CablePlugType plugType)
    {
        return (plugType == CablePlugType.Start ? StartPlug : EndPlug);
    }

    public void ForceMovePlug(CablePlugType plugType, Vector3 pos)
    {
        GetPlug(plugType).SnapTo(pos);
    }

    private void OnPlugPosChanged(int pointIndexInLine, CablePlug plug)
    {
        LineRenderer.SetPosition(pointIndexInLine, plug.transform.position);
    }

    public void SetIsFullyConnected(bool value)
    {
        IsFullyConnected = true;
    }
}
