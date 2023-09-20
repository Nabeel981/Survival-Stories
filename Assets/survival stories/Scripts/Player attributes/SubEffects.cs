using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = " SubEffectss")]
public class SubEffects : ScriptableObject
{
    public EffectType effectType;
    public PlayerTraits effectedTrait;
    public Traits[] effectedTraitsData;
    
    public float ammount;
    public float Duration;
    public ObjectType effectedThings;
    public float newDelepleteRate;
    public float DurationForChangePerSecond;
}

