using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Skills/Durability")]
public class Durability : Skill
{



    public override void ProcessSkill()
    {
        if (Unlocked)
        {
            foreach (var item in InventorySystem.instance.allItemsLibrary.allTools)
            {
                item.durabilityLossRate = tiers[tierLevel];
            }
            // all tools in the game get a durability decrease


        }
    }
}
