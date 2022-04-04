using System.Collections;
using System.Collections.Generic;
using AI;
using Unity.VisualScripting;
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

    public class ConnectingCable
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
        ConnectingCables.AddRange(AIController.ConnectingCables.ToArray());
        NextCableConnectionIn = 0;
        new AIStateChangedEvent(AIStateType.TransferingData).Broadcast();
        GameUI.Instance.CreateEmojiPopup(AIController.EmojiSpawnTransform.position, EmojiType.Angry);
        AIController.StartCoroutine(InterruptionCoroutine());
    }

    private IEnumerator InterruptionCoroutine()
    {
        float timeUntilInterruption = UnityEngine.Random.Range(8.0f, 12.0f);
        yield return new WaitForSeconds(timeUntilInterruption);
        new AIInterrupt.AIInterruptEvent().Broadcast();
        new SetAIStateEvent(AIStateType.Idle).Broadcast();
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

        Console console = GetRandomConsoleWithFreeModules();
        if (console == null)
            return;

        ConsoleModule consoleModule = console.GetRandomConsoleNotInUse();
        if (consoleModule == null)
            return;

        Vector3 startPos = AIController.transform.position;
        Vector3 endPos = consoleModule.CableAttachementPosition.position;
        float connectionSpeed = Vector3.Distance(startPos, endPos) / connectionTime;
        ConnectingCable cc = new ConnectingCable(connectionSpeed, CableManager.Instance.SpawnCable(startPos, startPos), consoleModule);

        cc.Module.OnCableInitiallyConnected += () => OnCableConnected(cc);
        ConnectingCables.Add(cc);
        AIController.ConnectingCables.Add(cc);
    }

    private Console GetRandomConsoleWithFreeModules()
    {
        List<Console> free = new List<Console>();
        for (int i = 0; i < AIController.Consoles.Count; i++)
        {
            var c = AIController.Consoles[i];
            if (c.HasFreeModules())
                free.Add(c);
        }
        if (free.Count == 0)
            return null;
        return free.GetRandomInList();
    }

    private void OnCableConnected(ConnectingCable cc)
    {
        ConnectingCables.Remove(cc);
        AIController.ConnectedCables.Add(cc.Cable);
        CablePlug endplug = cc.Cable.GetPlug(CablePlugType.End);
        endplug.SnapTo(cc.Module.CableAttachementPosition.position);

        cc.Cable.OnInitiallyConencted(CableFullyConnected, AIController.CableDisconnected, cc.Module);
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
        plug.SetDirection(moduleAttachmentPos);
    }
}
