using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CablePlug : MonoBehaviour
{
    public bool CanBeMoved { get; private set; } = true;

    private Action<CablePlug> OnPositionChanged;

    private const float MinDraggingUpdateMagnitude = 0.025f;
    private Vector3 LastUpdatePosition;
    private Vector3 OnMouseDownScreenPoint;
    private Vector3 DragOffset;

    public void Setup(Vector3 position, bool canBeMoved, Action<CablePlug> onPositionChanged = null)
    {
        transform.position = position;
        CanBeMoved = CanBeMoved;
        OnPositionChanged = onPositionChanged;
    }


    void OnMouseDown()
    {
        if (!CanBeMoved)
            return;
        OnMouseDownScreenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        DragOffset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, OnMouseDownScreenPoint.z));
    }

    void OnMouseDrag()
    {
        if (!CanBeMoved)
            return;

        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, OnMouseDownScreenPoint.z);
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + DragOffset;
        transform.position = curPosition;

        Vector3 delta = transform.position - LastUpdatePosition;
        float deltaMagnitudeSqr = delta.sqrMagnitude;
        if (deltaMagnitudeSqr > MinDraggingUpdateMagnitude)
        {
            LastUpdatePosition = transform.position;
            OnPositionChanged?.Invoke(this);
        }

    }
}
