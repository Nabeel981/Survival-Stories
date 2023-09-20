using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Buildings/building")]
public class BuildingData : CraftableItemData
{
    public float aoeRange;
    public int maxAllowed;
    public float breakTime;
    public float constructionTime;
    public List<SubEffects> aoeEffects;
    public int totalAmmount;
    public delegate void buildingRangeIncrease();
    public event buildingRangeIncrease rangeIncrease;
    public void ChangeAoERange(float newRange)
    {


        aoeRange = newRange;
        rangeIncrease?.Invoke();

    }

    public void ChangeconstructionTime(float newTime)
    {

        constructionTime = newTime;

    }

    //percentagge
    public void ReduceCost(int value)
    {
        foreach (var item in price)
        {
            item.quantity -= value;
        }

    }

}
