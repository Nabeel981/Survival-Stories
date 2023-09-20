using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
[CreateAssetMenu(menuName = "InventoryItems/Resources")]
public class ResourceData : ObjectData
{

    public ResourcesType resourcesType;
    public ResourceBehaviour resourceBehaviour;
    public float harvestingSpeed;
    public float doubleChance;
    [Header("Tools required to gather")]
    public ToolData[] gatheringRequirments;
    [Header("Further Classified into")]
    public ResourceData[] res;

    [Header("Effects of this resource on consumtion")]
    public SubEffects[] effects;

    public int totalAmmount;
    public void ChangeDropChance(float newChance)
    {
        doubleChance = newChance;


    }

    //private void Awake()
    //{
    //    if (resourcesType == ResourcesType.Bush)
    //    {
    //        int a = Random.Range(1, 5);
    //        switch (a)
    //        {
    //            case 1:
    //                resourcesType = ResourcesType.Glitterherb;
    //                break;
    //            case 2:
    //                resourcesType = ResourcesType.Featherherb;
    //                break;
    //            case 3:
    //                resourcesType = ResourcesType.Mintyherb;
    //                break;
    //            case 4:
    //                resourcesType = ResourcesType.Spicyherb;
    //                break;

    //        }


    //    }
    //}
}
// we can make a parent for the inventory item and use the same features used by inventory items to save data in the resource places
//stacks will be sent fron one place to another