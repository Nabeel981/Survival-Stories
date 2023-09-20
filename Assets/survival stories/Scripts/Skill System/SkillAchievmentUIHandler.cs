using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json;


public class SkillAchievmentUIHandler : MonoBehaviour, ISaveable
{
    [JsonIgnore] public ObjectData Data;
    [JsonIgnore] public Image icon;
    [JsonIgnore] public Image backGroundColor;
    [JsonIgnore] public Slider slid;
    [JsonIgnore] public TextMeshProUGUI text;
    [JsonIgnore] SkillAchievementWrapper wrap = new SkillAchievementWrapper();

    public int currentValue;
    public int currentGoal;
    public delegate void SkillPointChanged();
    public static event SkillPointChanged skillPointEvent;
    private void OnEnable()
    {
        // Debug.Log("i have been called1");
        // Data.countIncreased += IncreaseCurrentGoal;
        // Data.countIncreased += IncreaseCurrentGoal;
        if (currentGoal == 0)
        {
            currentGoal = SkillsSystem.NextAchievmentTier(currentGoal);
        }

        DisplayData();
        LoadData(); ;
    }

    private void Start()
    {
        //   Data.countIncreased += IncreaseCurrentGoal;
        //  Debug.Log("i have been called2");
        //  Data.countIncreased += IncreaseCurrentGoal;
        // currentGoal = SkillsSystem.NextAchievmentTier(currentGoal);
        // DisplayData();
    }
    private void Awake()
    {




    }
    public void SubscribeEvent()
    {
        Data.countIncreased += IncreaseCurrentGoal;
        if (currentGoal == 0)
        {
            currentGoal = SkillsSystem.NextAchievmentTier(currentGoal);
        }
        DisplayData();
    }
    public void IncreaseCurrentGoal()
    {
        Debug.Log("this happened 2");
        currentValue++;
        text.text = (currentValue + " / " + currentGoal);
        slid.value = HelperFunctions.Remap(currentValue, 0, currentGoal, 0, 1);
        NextAchievmentCheck();
        SaveData();
    }
    public void ShowLoadedGoal()
    {
        Debug.Log("this happened 2");

        text.text = (currentValue + " / " + currentGoal);
        slid.value = HelperFunctions.Remap(currentValue, 0, currentGoal, 0, 1);
        NextAchievmentCheck();
        SaveData();
    }
    [ContextMenu("RunTHis for achiev")]
    public void DisplayDataEditor()
    {
        icon.sprite = Data.icon;
    }
    public void DisplayData()
    {

        icon.sprite = Data.icon;
        text.text = (currentValue + " / " + currentGoal);
        slid.value = HelperFunctions.Remap(currentValue, 0, currentGoal, 0, 1);

    }


    public void ResetSlider()
    {
        slid.value = 0;
    }
    public void NextAchievmentCheck()
    {
        if (currentValue == currentGoal)
        {
            currentValue = 0;
            SkillsSystem.skillPoints++;
            skillPointEvent?.Invoke();
            backGroundColor.sprite = SkillsSystem.ReturnBackgroundColor(currentGoal);
            currentGoal = SkillsSystem.NextAchievmentTier(currentGoal);
            text.text = (currentValue + " / " + currentGoal);

            ResetSlider();
        }


    }

    public void SaveData()
    {
        wrap.currentGoalWrapper = currentGoal;
        wrap.currentValueWrapper = currentValue;
        string str = JsonConvert.SerializeObject(wrap);
        PlayerPrefs.SetString(gameObject.name + "achievSkill", str);
    }

    public void LoadData()
    {
        if (PlayerPrefs.HasKey("achievSkill"))
        {


            string str = PlayerPrefs.GetString("achievSkill");
            wrap = JsonConvert.DeserializeObject<SkillAchievementWrapper>(str);
            currentGoal = wrap.currentGoalWrapper;
            currentValue = wrap.currentValueWrapper;

        }
        ShowLoadedGoal();
    }
}
public class SkillAchievementWrapper
{
    public int currentValueWrapper;
    public int currentGoalWrapper;
}
