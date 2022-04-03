using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIState_Idle : AIState
{
    [System.Serializable]
    public struct Settings
    {
    }

    private bool SupressEvent = false;

    public AIState_Idle(AIController controller) : base(controller)
    {
    }
    public AIState_Idle(AIController controller, bool supressEvent) : base(controller)
    {
        SupressEvent = supressEvent;
    }

    protected override void Construct()
    {
        new AIStateChangedEvent(AIStateType.Idle).Broadcast();
    }

    public override void Update()
    {
    }
}
