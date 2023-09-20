using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsOnPLayer : MonoBehaviour
{
    public SubEffects effect;
    public float effectCurrentValue;
    public bool routineFinished = false;

    private void Start()
    {

        effectCurrentValue = effect.ammount;


    }
    public void EffectPlayer()
    {
        foreach (var item in InventorySystem.instance._currentDefaults.pTraits)
        {
            Debug.Log("Value Changed wwwwwwwwwwwwwwwwwwwww");
            if (item.trait == effect.effectedTrait)
            {
                Debug.Log("Value Changed");
                if (effect.effectType.HasFlag(EffectType.ValueChange))
                {
                    Debug.Log("Value Changed 1");
                    if (effectCurrentValue > 0)
                    {


                        if (effect.Duration == 0)
                        {
                            item.currentQuantity += effect.ammount;


                            effectCurrentValue = 0;
                        }
                        else if (HelperFunctions.TimePasssed(1))
                        {
                            float ammPerSec = effect.ammount / effect.Duration;
                            item.currentQuantity += ammPerSec;
                            effectCurrentValue += ammPerSec;

                        }

                    }

                    if (item.currentQuantity >= item.maxQuantity)
                    {
                        item.currentQuantity = item.maxQuantity;


                    }
                    if (item.currentQuantity <= 0)
                    {
                        item.currentQuantity = 0;
                    }

                    if (effectCurrentValue == 0)
                    {

                        StopAllCoroutines();
                        // item.ChangePerSecond = .5f;
                        InventorySystem.effectsOnPlayer.Remove(this);
                        Destroy(gameObject);


                    }
                }
                if (effect.effectType.HasFlag(EffectType.RateChange))
                {
                    Debug.Log("Value Changed 2");
                    //float rate = item.ChangePerSecond;
                    //item.ChangePerSecond = effect.newDelepleteRate;
                    // StartCoroutine(ChangeChangePerSecond(rate, effect.DurationForChangePerSecond, item));
                }
            }
        }


    }

    private void Update()
    {

        EffectPlayer();

    }


    public IEnumerator ChangeChangePerSecond(float rate, float time, Traits trait)
    {

        yield return new WaitForSeconds(time);
        Debug.Log("it should be changed");

        routineFinished = true;

    }

}
