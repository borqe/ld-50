using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIState_Confused : AIState
{
    [System.Serializable]
    public struct Settings
    {
        public float DataTransferHaltTime;
    }


    private float DelayTime;
    private AIStateType NextState;

    public AIState_Confused(AIController controller, AIStateType nextState) : base(controller)
    {
        NextState = nextState;
    }

    protected override void Construct()
    {
        new AIStateChangedEvent(AIStateType.Confused).Broadcast();
        DelayTime = AIController.Settings.ConfusedSettings.DataTransferHaltTime;
    }

    public override void Update()
    {
        DelayTime -= Time.deltaTime;
        if (DelayTime <= 0)
        {
            Transition();
        }
    }

    private void Transition()
    {
        new SetAIStateEvent(NextState).Broadcast();
    }
}