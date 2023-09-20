using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerTraits/trait")]
public class Traits : ObjectData
{
    public PlayerTraits trait;
    public float maxQuantity;
    public float currentQuantity;
    public float ChangePerSecond;

    public Traits effectedTrait;


    private void Awake()
    {
        currentQuantity = maxQuantity;
    }
    private void OnEnable()
    {
        currentQuantity = maxQuantity;
    }



    public void AddHealthRegen(float newValue)
    {
        if(trait == PlayerTraits.Health)
        {
            ChangePerSecond = newValue;
        }
        
     

    }  
    public void SpeedChange(float newValue)
    {
        if(trait == PlayerTraits.Speed)
        {
            currentQuantity = newValue;
        }
        
     

    }
}
