using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftableItemData : ObjectData
{
    [Header("Needed Resources are set inside")]
    [SerializeField] public List<SubPrice> price = new List<SubPrice>();

    [Header("Needed AOE buildings are set inside")]
    [SerializeField] public List<BuildingData> buildingsAOE = new List<BuildingData>();
}
