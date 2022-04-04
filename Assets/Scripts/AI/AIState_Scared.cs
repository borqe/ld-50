using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIState_Scared : AIState
{
    [System.Serializable]
    public struct Settings
    {
        public float StateDuration;
        [Range(0f, 1f)] public float DroppedConnectionsPercentage;
        public int MinDroppedConnections;
    }

    private float StateTime;

    public AIState_Scared(AIController controller) : base(controller)
    {
    }

    protected override void Construct()
    {
        new AIStateChangedEvent(AIStateType.Scared).Broadcast();
        DropConnections();
        StateTime = AIController.Settings.ScaredSettings.StateDuration;
        AIController.StartCoroutine(TimeoutCoroutine());
        GameUI.Instance.CreateEmojiPopup(AIController.EmojiSpawnTransform.position, EmojiType.Confounded);
    }

    private IEnumerator TimeoutCoroutine()
    {
        yield return new WaitForSeconds(StateTime);
        new SetAIStateEvent(AIStateType.TransferingData).Broadcast();
    }

    private void DropConnections()
    {
        int minDrop = AIController.Settings.ScaredSettings.MinDroppedConnections;
        float dropPercent = AIController.Settings.ScaredSettings.DroppedConnectionsPercentage;

        int connectionCount = AIController.ConnectedCables.Count;
        int connectionsToDropCount = Mathf.Max(Mathf.FloorToInt(connectionCount * dropPercent), minDrop);

        for (int i = 0; i < connectionsToDropCount; i++)
        {
            if (AIController.ConnectedCables.Count == 0)
                break;

            Cable c = AIController.ConnectedCables.Last();
            AIController.ConnectedCables.Remove(c);
            c.DropEndPlug();
        }
    }

    public override void Update()
    {
        // StateTime -= Time.deltaTime;
        // if (StateTime <= 0)
        //     new SetAIStateEvent(AIStateType.TransferingData).Broadcast();
    }
}
