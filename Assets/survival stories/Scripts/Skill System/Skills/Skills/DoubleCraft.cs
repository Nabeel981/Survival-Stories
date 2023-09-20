using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/DoubleCraft")]
public class DoubleCraft : Skill
{
    public ToolData effectedTrait;
    //later change to player double crafrt chance and use that

    public override void ProcessSkill()
    {
        if (Unlocked)
        {
            foreach (var item in InventorySystem.instance.allItemsLibrary.allTools)
            {
                item.doubleCraftChance = tiers[tierLevel];
            }
            //  effectedTrait.ChangePerSecond = tiers[tierLevel];

        }
    }
}
