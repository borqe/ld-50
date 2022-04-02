using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIState_Charging : AIState
{
    [System.Serializable]
    public struct Settings
    {
        public float MinCableConnectionInterval;
        public float MaxCableConnectionInterval;
        public float MinCableConnectionTime;
        public float MaxCableConnectionTime;

        public float PerSecondDataTransfer;
        public float PerConnectionDataTransferMultiplier;

        public float DataTransferPerConnection;
    }

    private class ConnectingCable
    {
        public float ConnectionSpeed;
        public Cable Cable;
        public ConsoleModule Module;

        public ConnectingCable(float connectionSpeed, Cable cable, ConsoleModule module)
        {
            ConnectionSpeed = connectionSpeed;
            Cable = cable;
            Module = module;
        }
    }

    private float NextCableConnectionIn;

    private List<ConnectingCable> ConnectingCables;

    public AIState_Charging(AIController controller) : base(controller)
    {
    }

    protected override void Construct()
    {
        ConnectingCables = new List<ConnectingCable>();
        NextCableConnectionIn = 0;
        AIController.OnAIStateChanged?.Invoke(AIStateType.TransferingData);
    }

    public override void Update()
    {
        for (int i = 0; i < ConnectingCables.Count; i++)
        {
            ConnectingCable cc = ConnectingCables[i];
            MoveEndPlug(cc);
        }

        NextCableConnectionIn -= Time.deltaTime;
        if (NextCableConnectionIn <= 0)
        {
            StartNewCableConnection();
            NextCableConnectionIn = Random.Range(AIController.Settings.ChargingSettings.MinCableConnectionInterval, AIController.Settings.ChargingSettings.MaxCableConnectionInterval);
        }

        IncramentDataTransferred();
    }

    private void IncramentDataTransferred()
    {
        int fullyConnectedCables = AIController.GetFullyConenctedCableCount();

        float dataTransfer = Time.deltaTime * AIController.Settings.ChargingSettings.PerSecondDataTransfer * fullyConnectedCables * AIController.Settings.ChargingSettings.PerConnectionDataTransferMultiplier;
        AIController.AddDataTransfer(dataTransfer);
    }

    private void StartNewCableConnection()
    {
        float connectionTime = Random.Range(AIController.Settings.ChargingSettings.MinCableConnectionTime, AIController.Settings.ChargingSettings.MaxCableConnectionTime);
        ConsoleModule consoleModule = AIController.Consoles.GetRandomInList().GetRandomModule();

        Vector3 startPos = AIController.transform.position;
        Vector3 endPos = consoleModule.CableAttachementPosition.position;
        float connectionSpeed = Vector3.Distance(startPos, endPos) / connectionTime;
        ConnectingCable cc = new ConnectingCable(connectionSpeed, CableManager.Instance.SpawnCable(startPos, startPos), consoleModule);

        cc.Module.OnCableConnected += () => OnCableConnected(cc);
        ConnectingCables.Add(cc);
    }

    private void OnCableConnected(ConnectingCable cc)
    {
        ConnectingCables.Remove(cc);
        AIController.ConnectedCables.Add(cc.Cable);
        CablePlug endplug = cc.Cable.GetPlug(CablePlugType.End);
        endplug.SnapTo(cc.Module.CableAttachementPosition.position);

        GameUI.Instance.CreatePopup(cc.Module.CableAttachementPosition.position + new Vector3(0, 0.5f, 0)).onTimerEnd += () => CableFullyConnected(cc.Cable);
    }

    private void CableFullyConnected(Cable cable)
    {
        AIController.AddDataTransfer(AIController.Settings.ChargingSettings.DataTransferPerConnection);
        cable.SetIsFullyConnected(true);
    }

    private void MoveEndPlug(ConnectingCable connectingCable)
    {
        CablePlug plug = connectingCable.Cable.GetPlug(CablePlugType.End);
        Vector3 plugPos = plug.transform.position;

        Vector3 moduleAttachmentPos = connectingCable.Module.CableAttachementPosition.position;
        Vector3 newPos = plugPos + ((moduleAttachmentPos - plugPos).normalized * (connectingCable.ConnectionSpeed * Time.deltaTime));
        connectingCable.Cable.ForceMovePlug(CablePlugType.End, newPos);
    }
}
