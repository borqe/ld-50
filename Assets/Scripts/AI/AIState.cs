using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIState
{
    protected AIController AIController;
    public AIState(AIController controller)
    {
        Debug.LogError($"Entering {this.GetType().Name} state");
        AIController = controller;
        Construct();
    }

    protected abstract void Construct();
    public abstract void Update();
}
