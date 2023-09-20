using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Skills/Harvester")]
public class Harvester : Skill
{
    //harvestingSpeed

    public Traits effectedTrait;

    public override void ProcessSkill()
    {
        if (Unlocked)
        {
            foreach (var item in InventorySystem.instance.allItemsLibrary.allResources)
            {
                item.harvestingSpeed = HelperFunctions.DecreaseByPercent(tiers[tierLevel], item.harvestingSpeed);
            }


        }
    }
}
