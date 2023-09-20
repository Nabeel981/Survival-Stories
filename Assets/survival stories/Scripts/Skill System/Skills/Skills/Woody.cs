using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Skills/Woody")]
public class Woody : Skill
{

    public ResourceData effectedData;

    public override void ProcessSkill()
    {
        if (Unlocked)
        {
            effectedData.doubleChance = tiers[tierLevel];
        }
    }

   


}
