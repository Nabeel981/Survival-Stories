using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Builder")]
public class Builder : Skill
{

    // reduce building times for all 
    public ResourceData effectedData;

    public override void ProcessSkill()
    {
        if (Unlocked)
        {
            Debug.Log(InventorySystem.instance + "check instance");
            foreach (var item in InventorySystem.instance.allItemsLibrary.allBuildings)
            {
                Debug.Log("ran this ammount of times");
                item.constructionTime = HelperFunctions.DecreaseByPercent(tiers[tierLevel], item.constructionTime);
            }


        }
    }
}
