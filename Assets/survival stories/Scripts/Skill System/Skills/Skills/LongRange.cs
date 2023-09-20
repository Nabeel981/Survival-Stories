using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(menuName = "Skills/LongRange")]
public class LongRange : Skill
{
    // Increase Building Ranges for all buildings 
    public ResourceData effectedData;

    public override void ProcessSkill()
    {
        if (Unlocked)
        {
            foreach (var item in InventorySystem.instance.allItemsLibrary.allBuildings)
            {

                float range = HelperFunctions.IncreaseByPercent(tiers[tierLevel], item.aoeRange);
                item.ChangeAoERange(range);
            }


        }
    }
}
