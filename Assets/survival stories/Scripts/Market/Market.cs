using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Xml.Serialization;
using UnityEngine.InputSystem.LowLevel;

public class Market : MonoBehaviour
{
    public static int gold;
    public TextMeshProUGUI goldUi;

    public static int chronoCrystals;
    public TextMeshProUGUI chronoCystalsUi;




    public void updateUI()
    {
        
        UpdateGoldUI();
        UpdateChronoCystalsUI();
    }
    public void UpdateGoldUI()
    {
        goldUi.text = "Gold: " + gold.ToString();
    }
    public void UpdateChronoCystalsUI()
    {
        chronoCystalsUi.text = "Chronos: " + chronoCrystals.ToString();

    }
   


    public void GoldIncrease(int ammount = 1)
    {

    }
    public void CronoCrystalsIncrease(int ammount = 1)
    {

    }

    public void GoldDecrease(int ammount = 1)
    {

    }
    public void CronoCrystalsDecrease(int ammount = 1)
    {

    }
}
