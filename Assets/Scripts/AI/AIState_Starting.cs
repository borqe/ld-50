using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIState_Starting : AIState
{
    [System.Serializable]
    public struct Settings
    {
        [Range(0f, 1f)] public float TransitionToCharge;
        public float ChargeGainPerSec;
    }

    public AIState_Starting(AIController controller) : base(controller)
    {
    }

    protected override void Construct()
    {
    }

    public override void Update()
    {
        if (AIController.AIData.DataTransfered >= AIController.Settings.StartingSettings.TransitionToCharge * AIController.Settings.MaxCharge)
        {
            AIController.SetNewState(new AIState_Charging(AIController));
        }
        else
            AIController.AIData.DataTransfered += Time.deltaTime * AIController.Settings.StartingSettings.ChargeGainPerSec;
    }
}
