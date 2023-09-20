using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "AI/Decisions/PlayerChaseCheck ")]
public class ChasePlayerDecision : Decisions
{
    public override bool Decide(StateController controller)
    {
        return SurroundingCheck(controller);
    }

    public bool SurroundingCheck(StateController controller)
    {

        if (controller.surroundCheck.EnemiesInTrigger.Count >= 1 )
        {
            controller.target = controller.surroundCheck.EnemiesInTrigger[0].transform;
            return true;

        }
        else
        {
            if (Vector2.Distance(controller.agent.transform.position, controller.lastKnownMyLocation) > 1f)
            {
                return true;
            }
            else
            {

                return false;
            }


        }
       

    }
}
