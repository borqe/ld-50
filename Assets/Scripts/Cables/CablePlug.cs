using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CablePlugType { Start = 0, End = 2 }

public class CablePlug : MonoBehaviour
{
    public bool CanBeMoved { get; private set; } = true;
    public bool PendingGravity { get; private set; } = false;
    public bool CollisionEnabled { get; private set; } = true;

    private Action<CablePlug> OnPositionChanged;

    private const float MinDraggingUpdateMagnitude = 0.025f;
    private Vector3 LastUpdatePosition;
    private Vector3 OnMouseDownScreenPoint;
    private Vector3 DragOffset;

    private ConsoleModule ConsoleToSnapTo;

    private Vector3 FakeVelocityCache;

    public void Setup(Vector3 position, bool canBeMoved, Action<CablePlug> onPositionChanged = null)
    {
        transform.position = position;
        CanBeMoved = CanBeMoved;
        OnPositionChanged = onPositionChanged;
    }

    public void SnapToOnEndMouseDrag(ConsoleModule module)
    {
        ConsoleToSnapTo = module;
    }
    public void CancelSnapping()
    {
        ConsoleToSnapTo = null;
    }

    public void SnapTo(Vector3 position)
    {
        transform.position = position;
        OnPositionChanged?.Invoke(this);
    }

    private void OnMouseUp()
    {
        if (PendingGravity)
        {
            EnableGravity();
        }
        
        if (!CanBeMoved)
            return;

        if (ConsoleToSnapTo != null)
        {
            SnapTo(ConsoleToSnapTo.transform.position);
            ConsoleToSnapTo = null;
        }

    }

    void OnMouseDown()
    {
        if (!CanBeMoved)
            return;
        ConsoleToSnapTo = null;
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
    }

    public void SetPendingGravity()
    {
        PendingGravity = true;
        CollisionEnabled = false;
    }

    public void EnableGravity()
    {
        Rigidbody r = GetComponent<Rigidbody>();
        r.useGravity = true;
        r.velocity = FakeVelocityCache * 15f;
        BoxCollider bc = GetComponent<BoxCollider>();
        bc.isTrigger = false;
        PendingGravity = false;
        CollisionEnabled = false;
        CanBeMoved = false;
    }

    public void Update()
    {
        Vector3 delta = transform.position - LastUpdatePosition;
        float deltaMagnitudeSqr = delta.sqrMagnitude;
        if (deltaMagnitudeSqr > MinDraggingUpdateMagnitude)
        {
            FakeVelocityCache = transform.position - LastUpdatePosition;
            LastUpdatePosition = transform.position;
            OnPositionChanged?.Invoke(this);
        }
    }

    public void SetDirection(Vector3 target)
    {
        transform.LookAt(target);
    }
}
