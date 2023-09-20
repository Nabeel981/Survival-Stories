using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TraitnSlider : MonoBehaviour
{
    public Traits trait;
    public Slider slider;
    public float timePassed;

    private void OnEnable()
    {

    }

    public void DepleteTraitValue()
    {

        timePassed += 1f;
        if (checkRequirmentsforDepletion())
        {

            // Debug.Log("second keeps passing 2");
            TraitType();
            if (trait.currentQuantity > trait.maxQuantity)
            {
                trait.currentQuantity = trait.maxQuantity;
            }
            else if (trait.currentQuantity <= 0)
            {
                trait.currentQuantity = 0;
            }
            if (trait.currentQuantity < 300)
            {
                if (InventorySystem.AutoUseTheToolsInInventory(trait))
                {

                }
                else
                {
                    InventorySystem.AutoConsumeResInInventory(trait);
                }
            }

            UpdateTraitSlider();
        }


    }
    public void UpdateTraitSlider()
    {
        slider.value = HelperFunctions.Remap(trait.currentQuantity, 0, trait.maxQuantity, 0, 1);
    }

    public bool checkRequirmentsforDepletion()
    {


        return true;
    }

    public void TraitType()
    {

        switch (trait.trait)
        {

            case PlayerTraits.Hunger:
                trait.currentQuantity += trait.ChangePerSecond;
                break;

            case PlayerTraits.Thirst:
                trait.currentQuantity += trait.ChangePerSecond;
                break;

            case PlayerTraits.Stamina:
                if (InventorySystem.instance.isIdle)
                {
                    trait.currentQuantity += trait.ChangePerSecond;
                }
                else
                {
                    trait.currentQuantity -= trait.ChangePerSecond;
                }
                break;
            case PlayerTraits.Health:
                trait.currentQuantity += trait.ChangePerSecond;
                break;

        }


    }
}
