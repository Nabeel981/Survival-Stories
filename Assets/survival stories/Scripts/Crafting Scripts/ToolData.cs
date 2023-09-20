using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(menuName = "InventoryItems/Tools")]
public class ToolData : CraftableItemData
{

    public int Damage;
    public int durability;
    public float durabilityLossRate;
    public float doubleCraftChance;
    public int HarvestSpeed;

    public ToolType toolType;
    public UseType useType;
    //   public bool isContainer; 
    //  public bool isFull; 
    public float doubleHarvestChance;

    [Header("Item can collect these")]
    public ResourcesType res;

    [Header("Effects of the tool")]
    public List<SubEffects> toolEffects;







    [Header("Can contain  inside")]
    public container contains;


    [Header("Item background colors")]
    public Color[] backgroundColors;

    public int totalAmmount;
    public void ChangeDoubleCraftChance()
    {

    }

    public void Crafted()
    {
        IncreaseInteractedAmmount();
    }

    public void doubleHarvestChanceFunc(int chance)
    {

        doubleCraftChance = chance;

    }


}


