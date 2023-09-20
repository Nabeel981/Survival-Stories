using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;


public class PlayerAttributesSystem : MonoBehaviour, ISaveable
{

    public static PlayerAttributesSystem instance;
    public Traits health;
    public Traits hunger;
    public Traits thirst;
    public Traits stamina;
    public Traits movementSpeed;

    public TraitnSlider[] traitsnSliders;
    public delegate void TimePassedEvent();
    public static event TimePassedEvent SecondPassedEvent;
    public static event TimePassedEvent UpdateSliderEvent;

    AttributeSystemWrapper atrwrapper = new AttributeSystemWrapper();
    private void OnEnable()
    {
        if (instance == null) instance = this;
    }

    private void Start()
    {

        SubscribetheclassestotheEvent();
        LoadData();
        InvokeRepeating("SecondPassed", 0f, 1f);

    }

    public void CheckHealthDepletion()
    {
        if (hunger.currentQuantity <= 0)
        {
            health.currentQuantity += health.ChangePerSecond;
        }
        if (thirst.currentQuantity <= 0)
        {
            health.currentQuantity += health.ChangePerSecond;
        }
    }
    public void SubscribetheclassestotheEvent()
    {
        foreach (var item in traitsnSliders)
        {
            PlayerAttributesSystem.SecondPassedEvent += item.DepleteTraitValue;
            PlayerAttributesSystem.UpdateSliderEvent += item.UpdateTraitSlider;
        }

    }
    public void SecondPassed()
    {
        Debug.Log("second keeps passing");
        CheckHealthDepletion();
        SaveData();
        SecondPassedEvent?.Invoke();

    }

    public static bool HasEnoughStamina()
    {
        if (PlayerAttributesSystem.instance.stamina.currentQuantity <= 0)
        {
            return false;
        }
        return true;
    }
    public static void DepleteStamina()
    {

        PlayerAttributesSystem.instance.stamina.currentQuantity -= 10;
        UpdateSliderEvent?.Invoke();
    }
    public static void RecoverStamina()
    {

        PlayerAttributesSystem.instance.stamina.currentQuantity += 10;
        UpdateSliderEvent?.Invoke();
    }

    public static bool CheckAvailablityOfTrait(Traits obj, int quantity)
    {

        foreach (var item in InventorySystem.instance._currentDefaults.pTraits)
        {
            //  Debug.Log("1111111111111111111111");
            if (item == obj)
            {
                //  Debug.Log("aa1111111111111111111111");
                if (item.currentQuantity > quantity)
                {
                    //  Debug.Log("bb1111111111111111111111");
                    item.currentQuantity -= quantity;
                    return true;
                }
            }


        }
        //  Debug.Log("cc1111111111111111111111");
        return false;
    }

    //public static void UseAutoItemForTrait(Traits obj , float quantity)
    //{
    //    if(obj.)
    //}

    public static void ConsumeResource(Resource data)
    {
        Debug.Log("kazam");
        foreach (var item in InventorySystem.instance._currentDefaults.pTraits)
        {
            Debug.Log("kazam 1");

            foreach (var item1 in (data.inventoryItem.data as ResourceData).effects[0].effectedTraitsData)
            {
                Debug.Log(item.displayName + " parara" + item1.displayName);
                if (item == item1)
                {
                    Debug.Log("kazam 2");
                    item.currentQuantity += (data.inventoryItem.data as ResourceData).effects[0].ammount;
                }
            }

        }
    }

    public void SaveData()
    {
        atrwrapper.healthValue = health.currentQuantity;
        atrwrapper.hungerValue = hunger.currentQuantity;
        atrwrapper.thirstValue = thirst.currentQuantity;
        string str = JsonConvert.SerializeObject(atrwrapper);
        PlayerPrefs.SetString(gameObject.name, str);
    }

    public void LoadData()
    {
        if (PlayerPrefs.HasKey(gameObject.name))
        {
            string str = PlayerPrefs.GetString(gameObject.name);
            atrwrapper = JsonConvert.DeserializeObject<AttributeSystemWrapper>(str);
            health.currentQuantity = atrwrapper.healthValue;
            hunger.currentQuantity = atrwrapper.hungerValue;
            thirst.currentQuantity = atrwrapper.thirstValue;
        }

    }


    //public float GetTraitValue(Traits trait)
    //{

    //    foreach (var item in InventorySystem.instance.allItemsLibrary.allTraits)
    //    {
    //        if (item == trait)
    //        {

    //        }
    //    }
    //}
}

public class AttributeSystemWrapper
{
    public float healthValue;
    public float hungerValue;
    public float thirstValue;
}

