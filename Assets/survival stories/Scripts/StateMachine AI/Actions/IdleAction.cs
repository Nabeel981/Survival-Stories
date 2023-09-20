using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(menuName = "AI/Actions/IdleAction")]
public class IdleAction : Actions
{
    public override void Act(StateController Controller)
    {
        StayIdle(Controller);
    }

    public void StayIdle(StateController Controller)
    {
       // Controller.agent.enabled = false;
       // Controller.agent.SetDestination(Controller.lastKnownMyLocation);
        // any idle behaviour if needed

    }
}
