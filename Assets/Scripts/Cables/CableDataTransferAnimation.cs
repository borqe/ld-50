using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CableDataTransferAnimation : MonoBehaviour
{
    [SerializeField] private float Speed;

    private Transform Start;
    private Transform End;
    private float TimeToComplete;
    private float CurrentTime;

    public void Setup(Transform start, Transform end)
    {
        Start = start;
        End = end;

        var distance = Vector3.Distance(start.position, end.position);
        TimeToComplete = distance / Speed;
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(Start.position, End.position, CurrentTime / TimeToComplete);
        CurrentTime += Time.deltaTime;
        transform.LookAt(End.position);
        if (CurrentTime >= TimeToComplete)
            Destroy(gameObject);
    }
}
