using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Decisions/AttackDecision")]
public class AttackPlayerDecision : Decisions
{
    public override bool Decide(StateController controller)
    {
        return AttackPerson(controller);
    }

    private bool AttackPerson(StateController controller)
    {
        if (controller.target != null && Vector2.Distance(controller.transform.position, controller.target.position) <= .5f)
        {
            Debug.Log("start attacking");

            return true;
        }
        else
        {
            return false;

        }
    }
}
