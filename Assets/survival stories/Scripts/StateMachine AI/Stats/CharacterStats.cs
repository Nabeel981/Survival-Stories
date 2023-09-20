using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "CharacterStats")]
public class CharacterStats : ScriptableObject
{
    public Traits health;
    public Traits[] characterTraits;
    public float baseAttackDamage;
    public float baseForagingSpeed;
    public float moveSpeed;
    public float visionRadius;
}
