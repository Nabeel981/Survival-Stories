using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Chase")]
public class ChaseAction : Actions
{
    public float offsetDistance;
    public override void Act(StateController controller)
    {
        GoToDestination(controller);
    }

    public void GoToDestination(StateController controller)
    {
        Debug.Log("start walking");

        if (controller.surroundCheck.EnemiesInTrigger.Count > 0)
        {
            CharacterBehaviour.ChageDirection(controller.transform, controller.target.transform.position);
            if (Vector2.Distance(controller.agent.transform.position, controller.target.position) > .6f)
            {

                controller.agent.SetDestination(controller.target.position);


            }


        }
        else
        {
            CharacterBehaviour.ChageDirection(controller.transform, controller.lastKnownMyLocation);
            controller.agent.SetDestination(controller.lastKnownMyLocation);
        }

    }
}
