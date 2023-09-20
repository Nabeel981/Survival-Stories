using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillsSystem : MonoBehaviour
{
    public static SkillsSystem instance;
    public static int skillPoints;
    public TextMeshProUGUI totalSkillPoints;
    public SkillAchievmentUIHandler[] enableEventIssue;
    public Sprite[] backgrounds;

    private void OnEnable()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Awake()
    {
        //Debug.Log("i was called");
        foreach (var item in enableEventIssue)
        {
            item.SubscribeEvent();
        }
        SkillAchievmentUIHandler.skillPointEvent += ShowSKillPoints;
    }
    public void ShowSKillPoints()
    {
        totalSkillPoints.text = "Skill Points :" + SkillsSystem.skillPoints;
    }
    public static int NextAchievmentTier(int value)
    {
        switch (value)
        {
            case 0:
                return 1;
            case 1:
                return 10;
            case 10:
                return 100;
            case 100:
                return 1000;
            case 1000:
                return 100000;
            default:
                return 0;

        }


    }

    [ContextMenu("popsicle")]
    public void debugSkillPoints()
    {

        Debug.Log(skillPoints + " Total skill Points");
    }

    public static Sprite ReturnBackgroundColor(int value)
    {
        switch (value)
        {

            case 1:
                return  SkillsSystem.instance.backgrounds[0];
            case 10:
                return SkillsSystem.instance.backgrounds[1];
            case 100:
                return SkillsSystem.instance.backgrounds[2];
            case 1000:
                return SkillsSystem.instance.backgrounds[3];
            default:
                return SkillsSystem.instance.backgrounds[3]; 

        }
    }


    public static bool UnlockSkill(Skill _skill)
    {

        if (_skill.cost <= skillPoints)
        {
            skillPoints -= _skill.cost;
            SkillsSystem.instance.ShowSKillPoints();
            _skill.unlockTier(_skill);
            _skill.ProcessSkill();
            return true;
        }
        else
        {
            Debug.Log("not enough skill Points");
            return false;


        }
    }
}
