using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarketQuestSystem : MonoBehaviour , ISaveable  
{
    public int totalQuestsAllowed;
    //  public int maxTimeAllowed;
    public Market market;
    public GameObject[] quests;
    public delegate void RewardFromQuest();
    public static event RewardFromQuest questReward;
    private void Start()
    {

        questReward += market.updateUI;
        Debug.Log("reward should be shown");
        InvokeRepeating("TimeCheckForAllQuests", 0, 1);
        InvokeRepeating("makeQuest", 0, 100);
    }



    public void makeQuest()
    {
        Debug.Log("is the number1");
        foreach (var item in quests)
        {
            Debug.Log("is the number2");
            if (item.activeInHierarchy)
            {
                Debug.Log("is the number3");
            }
            else
            {
                int a = UnityEngine.Random.Range(0, 100);
                Debug.Log(a + " is the number");
                if (a < 200)
                {

                    item.GetComponent<MarketQuestUIHolder>().ShowNewQuest();
                    item.SetActive(true);

                }
            }
        }
    }
    public void IsSpaceAvailable()
    {

    }

    public void TimeCheckForAllQuests()
    {

        foreach (var item in quests)
        {
            if (item.GetComponent<MarketQuestUIHolder>().ShowTime())
            {
                // item.SetActive(true);
            }
            else
            {
                item.SetActive(false);
            }
        }

    }

    public static bool CheckRequirementsForCrafting(CraftableItemData item)
    {
        Debug.Log("called times 4");


        foreach (SubPrice pr in item.price)
        {
            if (!CraftingSystem.PriceTypeCheck(pr.objectData, pr.quantity))
            {
                Debug.Log("price check failed");
                return false;
            }

        }
        return true;
    }


    public static bool CollectRes(CraftableItemData item)
    {
        foreach (SubPrice price in item.price)
        {
            if (price.objectData.objectType == ObjectType.Resource && (price.objectData as ResourceData).resourceBehaviour.HasFlag(ResourceBehaviour.Containable))
            {
                InventorySystem.RemoveResInContainer(price.objectData as ResourceData, price.quantity);

            }
            else
            {
                InventorySystem.instance.Remove((ObjectData)price.objectData, price.quantity);
            }


        }
        CollectReward(item as MarketQuest);
        questReward?.Invoke();
        return true;
    }
    public static void CollectReward(MarketQuest questItem)
    {
        switch (questItem.rewardCurrency)
        {
            case Currency.none:
                break;
            case Currency.Gold:

                Market.gold += questItem.rewardAmmount;

                break;
            case Currency.ChronoCrystals:
                Market.chronoCrystals += questItem.rewardAmmount;

                break;
            default:
                break;
        }
        Debug.Log(Market.chronoCrystals + " " + Market.gold + "gold and crystals");
    }
    public static void GiveReward()
    {


    }

    public void SaveData()
    {
        throw new NotImplementedException();
    }

    public void LoadData()
    {
        throw new NotImplementedException();
    }
}

