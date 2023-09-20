using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Skills/Healthy")]
public class Healthy : Skill
{
    public Traits effectedTrait;

    public override void ProcessSkill()
    {
        if (Unlocked)
        {
            //  will impliment with  new Ai
            foreach (var item in InventorySystem.instance.allItemsLibrary.allBuildings)
            {
                effectedTrait.ChangePerSecond = tiers[tierLevel];

            }

        }
    }
}
