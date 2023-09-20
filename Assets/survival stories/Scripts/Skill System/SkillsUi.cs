using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillsUi : MonoBehaviour
{
    public Skill skillData;
    public Image icon;
    public TextMeshProUGUI Cost;
    public TextMeshProUGUI skillName;
    public TextMeshProUGUI info;
    public Button unlock;
    public Image[] tiers;

    private void Awake()
    {
        ShowData();
    }
    [ContextMenu("Show skill data")]
    public void ShowData()
    {
        icon.sprite = skillData.icon;
        skillName.text = skillData.displayName;
        info.text = skillData.info;
        Cost.text = "Price : " + skillData.cost.ToString();
        // icon.sprite = skillData.icon


    }

    public void OnCompletion()
    {
        for (int i = 0; i < skillData.tierLevel; i++)
        {

            tiers[i].color = Color.green;

        }
    }

    public void UnlockSkillBtnClicked()
    {
        if (SkillsSystem.UnlockSkill(skillData))
        {
            OnCompletion();
        }
        Cost.text = "Price is : " + skillData.cost.ToString();
    }
}
