using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Cable : MonoBehaviour
{
    private LineRenderer LineRenderer;

    [SerializeField] private CablePlug StartPlug;
    [SerializeField] private CablePlug EndPlug;

    private void Awake()
    {
        LineRenderer = GetComponent<LineRenderer>();
        transform.position = Vector3.zero;
    }

    public void Setup(Transform start, Transform end)
    {
        StartPlug.Setup(start.position, false, null);
        EndPlug.Setup(end.position, true, (plug) => OnPlugPosChanged(1, plug));
        Vector3[] positions = new Vector3[] {start.position, end.position};

        LineRenderer.SetPositions(positions);
    }

    private void OnPlugPosChanged(int pointIndexInLine, CablePlug plug)
    {
        LineRenderer.SetPosition(pointIndexInLine, plug.transform.position);
    }
}
