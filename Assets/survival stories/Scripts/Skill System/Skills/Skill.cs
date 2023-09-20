using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public abstract class Skill : ObjectData
{
    public SkillType skilltype;
    public UseType use;
    public int cost = 1;
    public int tierLevel;
    public bool Unlocked;
    public float[] tiers;
    private void Awake()
    {
        cost = 1;
        tierLevel = 0;
        Unlocked = false;

    }
    private void OnEnable()
    {
        cost = 1;
        tierLevel = 0;
        Unlocked = false;

    }
    public abstract void ProcessSkill();






    public void unlockTier(Skill skill)
    {
        Unlocked = true;

        if (tierLevel > tiers.Length - 1)
        {
            //max tier reached;
        }
        if (tierLevel <= tiers.Length - 1)
        {
            // SkillsSystem.skillPoints -= skill.cost;
            cost++;
            tierLevel++;

            Unlocked = true;
        }

    }
}
