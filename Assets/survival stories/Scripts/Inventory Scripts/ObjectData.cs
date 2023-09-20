using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

/// <summary>
/// has main info about objects and structs
/// </summary>
public class ObjectData : ScriptableObject
{
    
    public string StringID;
    public ObjectType objectType;
    public string displayName;
    public string info;

    [JsonIgnore] public Sprite icon;
    [JsonIgnore] public GameObject prefab;
    [JsonIgnore] public AnimationState animation;
    public int stackSize;
    public int totalTimesInteracted;

    public delegate void interactionCount();
    public event interactionCount countIncreased;
    private void OnEnable()
    {
        totalTimesInteracted = 0;
    }
    public void IncreaseInteractedAmmount()
    {

        totalTimesInteracted++;
        Debug.Log("this happened");
        countIncreased?.Invoke();

    }
}


