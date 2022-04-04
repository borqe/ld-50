using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Cable : MonoBehaviour
{
    private LineRenderer LineRenderer;

    [SerializeField] private float CableDataTransferAnimationInterval;
    [SerializeField] private CableDataTransferAnimation CableDataTransferAnimationPrefab;
    [SerializeField] private CablePlug StartPlug;
    [SerializeField] private CablePlug EndPlug;

    private ConsoleModule ConsoleModule;
    private PopupBase ConnectingPopup;
    private Action<Cable> OnDisconnected;
    public bool IsFullyConnected { get; private set; }

    private Coroutine DataAnimationRoutine;

    private bool AnimateDataTransfer;

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

    public void OnInitiallyConencted(Action<Cable> onFullyConencted, Action<Cable> onDisconnected, ConsoleModule module)
    {
        ConsoleModule = module;
        OnDisconnected = onDisconnected;

        module.OnCableDisconnected += Disconnected;
        PopupBase popup = GameUI.Instance.CreatePopup(ConsoleModule.CableAttachementPosition.position + new Vector3(0, 0.5f, 0));
        popup.onTimerEnd += () => onFullyConencted(this);
        ConnectingPopup = popup;
    }

    private void Disconnected()
    {
        OnDisconnected?.Invoke(this);
        if(ConnectingPopup != null)
            Destroy(ConnectingPopup.gameObject);
        EndPlug.SetPendingGravity();
        SetIsFullyConnected(false);
    }

    public void DropEndPlug()
    {
        OnDisconnected?.Invoke(this);
        if (ConnectingPopup != null)
            Destroy(ConnectingPopup.gameObject);
        EndPlug.EnableGravity();
        SetIsFullyConnected(false);
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
        // dirty fix for when game state ends and this begins to flood null references
        if (this == null)
            return;


        IsFullyConnected = value;
        EnableDataTransferAnimation(value);
    }

    public void EnableDataTransferAnimation(bool enable)
    {
        AnimateDataTransfer = enable;
        if (AnimateDataTransfer)
        {
            if (DataAnimationRoutine == null)
                DataAnimationRoutine = StartCoroutine(DataTransferAnimation());
        }
    }

    private IEnumerator DataTransferAnimation()
    {
        var delay = new WaitForSeconds(CableDataTransferAnimationInterval);
        while (AnimateDataTransfer && this != null)
        {
            var animation = Instantiate(CableDataTransferAnimationPrefab, transform);
            animation.Setup(StartPlug.transform, EndPlug.transform);
            yield return delay;
        }
    }
}
