using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Cable : MonoBehaviour
{
    private LineRenderer LineRenderer;
    private void Awake()
    {
        LineRenderer = GetComponent<LineRenderer>();
        transform.position = Vector3.zero;
    }

    public void Setup(params Transform[] attachements)
    {
        Vector3[] positions = new Vector3[attachements.Length];
        for (int i = 0; i < attachements.Length; i++)
        {
            positions[i] = attachements[i].position;
        }

        LineRenderer.SetPositions(positions);
    }
}
