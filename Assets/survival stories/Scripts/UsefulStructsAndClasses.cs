using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsefulStructsAndClasses : MonoBehaviour
{

}
/// <summary>
/// the price / resource requirments for an item
/// </summary>
[System.Serializable]

public class SubPrice
{

    public ObjectData objectData;
    public int quantity;

}

/// <summary>
/// the price / resource requirments for an item
/// </summary>
[System.Serializable]

public class ItemsForResources
{
    public ItemsForResources(ResourcesType resour, ToolData too = null)
    {
        resourcesType = resour;
        tool = too;
    }
    public ResourcesType resourcesType;
    public ToolData tool;

}

/// <summary>
/// effects that an item can have on the user
/// </summary>
[System.Serializable]
public struct Effects
{
    public ResourcesType resourcesType;
    public int Speed;

}

/// <summary>
/// items that can be contained inside an object ..e.g water in bottle
/// </summary>
[System.Serializable]
public class container
{

    public ObjectData data;
    public int maxQuantity;
    public List<Sprite> NewIcons;
}


/// <summary>
/// items that can be contained inside an object ..e.g water in bottle
/// </summary>
[System.Serializable]
public struct Effectss
{
    public List<SubEffects> effects;
}

/// <summary>
/// items that can be contained inside an object ..e.g water in bottle
/// </summary>
[System.Serializable]
public class BuildingsCount
{
    public BuildingData building;
    public int currentlyPlaced;
    public int DurationPassed;
    public ObjectType effectedThings;
    public bool effectInRange;
}


