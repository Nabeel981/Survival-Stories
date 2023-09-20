using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Skills/Fighter")]
public class Fighter : Skill
{

    public Traits effectedTrait;
    public CharacterStats effectedStat;

    public override void ProcessSkill()
    {
        if (Unlocked)
        {
           
            effectedStat.baseAttackDamage = HelperFunctions.IncreaseByPercent(tiers[tierLevel], effectedStat.baseAttackDamage);

        }
    }
}
