using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json;

public class PlayerProfileSystem : MonoBehaviour, ISaveable
{

    public static int playerXp = 0; //5
    public static int playerXpCap = 20; //20
    public static int playerLevel = 0;
    [JsonIgnore] public static DateTime totalTimePassed;
    public static float totalSecondsPassed;
    [JsonIgnore] public TextMeshProUGUI timeDisplay;
    [JsonIgnore] public TextMeshProUGUI xpDisplay;


    [JsonIgnore] private float startTime;
    [JsonIgnore] public Slider xpSlider;
    private int remainder;
    [JsonIgnore] public ParticleSystem levelupEffect;
    public delegate void XPGain(int ammount);
    public static event XPGain XPGainEvent;



    ProfileWrapper profileWrapper = new ProfileWrapper();



    private void Start()
    {
        LoadData();
        XPGainEvent += PlayerGainXP;
        startTime = Time.time; // Record the start time
        StartCoroutine(UpdateTimeCoroutine());

        DisplayLevel();
        xpSlider.value = HelperFunctions.Remap(playerXp, 0, playerXpCap, 0, 1);

    }
    public void PlayerGainXP(int ammount)
    {
        playerXp += ammount;
        remainder = playerXp - playerXpCap;
        if (remainder < 0)
        {

            xpSlider.value = HelperFunctions.Remap(playerXp, 0, playerXpCap, 0, 1);

        }
        else if (remainder >= 0)
        {
            LevelUpPlayer(remainder);
        }
        SaveData();
    }
    public void TotalTimeSpent()
    {

    }
    public void LevelUpPlayer(int remainingXp)
    {

        playerLevel++;
        levelupEffect.Play();
        playerXp = 0;
        playerXp += Mathf.Abs(remainingXp);
        Announcements.instance.PlayAnimation("LEVEL UP!", AnnouncementType.withoutImage);
        playerXpCap = (int)HelperFunctions.IncreaseByPercent(5, playerXpCap);
        xpSlider.value = HelperFunctions.Remap(playerXp, 0, playerXpCap, 0, 1);
        DisplayLevel();

    }

    [ContextMenu("addXP")]
    public static void AddXp(int ammount)
    {
        XPGainEvent?.Invoke(ammount);
    }
    public void DisplayLevel()
    {
        xpDisplay.text = playerLevel.ToString();
    }
    public void ResetSlider()
    {
        xpSlider.value = 0;
    }
    public void ShowTime()
    {
        totalTimePassed = totalTimePassed.AddSeconds(1);
        timeDisplay.text = totalTimePassed.ToString();
    }














    private IEnumerator UpdateTimeCoroutine()
    {
        while (true)
        {
            float elapsedTime = Time.time - startTime;
            UpdateTimeText(elapsedTime);
            yield return null; // Wait for the next frame
        }
    }

    private void UpdateTimeText(float elapsedSeconds)
    {
        int days = Mathf.FloorToInt(elapsedSeconds / (24 * 3600));
        elapsedSeconds -= days * 24 * 3600;

        int hours = Mathf.FloorToInt(elapsedSeconds / 3600);
        elapsedSeconds -= hours * 3600;

        int minutes = Mathf.FloorToInt(elapsedSeconds / 60);
        int seconds = Mathf.FloorToInt(elapsedSeconds % 60);

        string formattedTime = string.Format("{0:D2} Days :{1:D2} Hours:{2:D2} Minutes:{3:D2} Seconds", days, hours, minutes, seconds);

        // Update the UI text to display the formatted time
        timeDisplay.text = "Elapsed Time: " + formattedTime;
    }

    public void SaveData()
    {
        profileWrapper.playerLevelSave = playerLevel;
        profileWrapper.playerXpCapSave = playerXpCap;
        profileWrapper.playerXpSave = playerXp;
        profileWrapper.totalSecondsPassedSave = totalSecondsPassed;
        string st = JsonConvert.SerializeObject(profileWrapper);
        PlayerPrefs.SetString(gameObject.name, st);


    }

    public void LoadData()
    {
        if (PlayerPrefs.HasKey(gameObject.name))
        {
            string str = PlayerPrefs.GetString(gameObject.name);
            profileWrapper = JsonConvert.DeserializeObject<ProfileWrapper>(str);
            playerLevel = profileWrapper.playerLevelSave;
            playerXpCap = profileWrapper.playerXpCapSave;
            playerXp = profileWrapper.playerXpSave;

        }
    }
}
[System.Serializable]
public class ProfileWrapper
{
    public int playerXpSave;
    public int playerXpCapSave;
    public int playerLevelSave;
    public float totalSecondsPassedSave;
}

