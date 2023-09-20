using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[System.Serializable]
public class GlobalItemHolder : ScriptableObject
{
    public ToolData[] allTools;
    public ResourceData[] allResources;
    public BuildingData[] allBuildings;
    public Skill[] allSKills;
    public SubEffects[] allEffects;
    public Traits[] allTraits;

}
