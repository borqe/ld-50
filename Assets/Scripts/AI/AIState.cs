using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIState
{
    protected AIController AIController;
    public AIState(AIController controller)
    {
        AIController = controller;
        Construct();
    }

    protected abstract void Construct();
    public abstract void Update();
}
