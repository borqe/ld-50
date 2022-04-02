using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CameraSettings CameraSettings;
    private bool IsMouse2Down;

    private Vector3 DragStartPosition;
    private Vector3 MousePosition;

    private Vector3 TargetEulers;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            IsMouse2Down = true;
            MousePosition = Input.mousePosition;
            DragStartPosition = MousePosition;
        }
        if (Input.GetMouseButtonUp(1))
        {
            IsMouse2Down = false;
        }

        if (IsMouse2Down)
            MoveCamera();
    }

    private void MoveCamera()
    {
        Vector3 delta = MousePosition - Input.mousePosition;
        MousePosition = Input.mousePosition;

        Vector3 deltaFromStart = DragStartPosition - MousePosition;

        MoveCameraHorizontally(-delta.x, deltaFromStart.x);
        MoveCameraVertically(delta.y, deltaFromStart.y);

        Quaternion targetRot = Quaternion.Euler(TargetEulers);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, CameraSettings.TurningRate * Time.deltaTime);
    }

    private void MoveCameraHorizontally(float delta, float deltaFromDragStart)
    {
        Vector3 targetEuler = transform.rotation.eulerAngles + new Vector3(0, delta * CameraSettings.CameraSpeed.x, 0);
        TargetEulers.y = targetEuler.y;
    }

    private void MoveCameraVertically(float delta, float deltaFromDragStart)
    {
        Vector3 targetEuler = transform.rotation.eulerAngles + new Vector3(delta * CameraSettings.CameraSpeed.y, 0, 0);

        if (targetEuler.x >= CameraSettings.MinXAngle && targetEuler.x <= CameraSettings.MaxXAngle) 
            TargetEulers.x = targetEuler.x;
    }
}
