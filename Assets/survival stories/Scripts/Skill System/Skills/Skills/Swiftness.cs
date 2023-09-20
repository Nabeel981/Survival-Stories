using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Swiftness")]
public class Swiftness : Skill
{
    public Traits effectedTrait;
    public CharacterStats stats;
    public override void ProcessSkill()
    {
        if (Unlocked)
        {
            Debug.Log("swiftness");
            // will impliment with new Ai
            stats.moveSpeed = HelperFunctions.IncreaseByPercent(tiers[tierLevel], stats.moveSpeed);
        }
    }
}
