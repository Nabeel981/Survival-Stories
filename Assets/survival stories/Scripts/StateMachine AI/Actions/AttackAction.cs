using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "AI/Actions/AttackAction")]
public class AttackAction : Actions
{
    public override void Act(StateController controller)
    {
        CharacterBehaviour.ChageDirection(controller.transform, controller.target.transform.position);
        AttackPerson(controller);
    }

    private void AttackPerson(StateController controller)
    {
        //player being attacked
    }
}
