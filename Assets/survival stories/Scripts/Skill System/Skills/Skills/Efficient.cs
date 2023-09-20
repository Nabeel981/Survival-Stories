using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Efficient")]
public class Efficient : Skill
{
    // all construction costs decrease
    public ResourceData effectedData;

    public override void ProcessSkill()
    {
        if (Unlocked)
        {

          //no info yet
            //effectedData.doubleChance = tiers[tierLevel];


        }
    }
}
