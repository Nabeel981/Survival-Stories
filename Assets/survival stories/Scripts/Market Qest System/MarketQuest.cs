using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "MarketQuestNpc")]
public class MarketQuest : CraftableItemData
{
    public Currency rewardCurrency;
    public int rewardAmmount;
    public TimeSpan time;
    public DateTime lastRecordedTime;
    public bool active = false;



    public void Randomizer()
    {
        if (!active)
        {
            price.Clear();
            int a = UnityEngine.Random.Range(0, InventorySystem.instance.allItemsLibrary.allResources.Length);
            SubPrice sub = new SubPrice();
            sub.objectData = InventorySystem.instance.allItemsLibrary.allResources[a];
            sub.quantity = UnityEngine.Random.Range(1, 12);
            price.Add(sub);

            time = new TimeSpan( 20, 30 , 10 );
            lastRecordedTime = DateTime.Now;
            active = true;
            RandomCurrencynAmmount();
        }
    }


    public void RandomCurrencynAmmount()
    {
        int a  = UnityEngine.Random.Range(1, 100);
        if(a < 3)
        {
            rewardCurrency = Currency.ChronoCrystals;
            rewardAmmount = 1;
        }
        else
        {
            rewardCurrency = Currency.Gold;
            rewardAmmount = UnityEngine.Random.Range(10, 50); 
        }
    }
    public bool TimeCheck()
    {


        TimeSpan timeDifference = DateTime.Now - lastRecordedTime;
        //time -= timeDifference;
        time = new TimeSpan(time.Hours - timeDifference.Hours, time.Minutes - timeDifference.Minutes, time.Seconds - timeDifference.Seconds);
        if (time.TotalSeconds <= 0)
        {
            active = false;
            return false;
        }
        else
        {
            return true;
        }

    }
    public void ReadyUpQuest()
    {

    }

    public bool IsQuestComplete()
    {
        foreach (var item in price)
        {
            if (!InventorySystem.HasItemWithAmmount(item.objectData, item.quantity))
            {
                return false;
            }
        }
        return true;
    }
    public string RemainingTime()
    {
        if (lastRecordedTime != null && active)
        {
            TimeSpan timeDifference = DateTime.Now - lastRecordedTime;
            return timeDifference.ToString(); ;

        }
        return "no Time Found";
    }
    public void DeActivateQuest()
    {
        active = false;
    }

}
