using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CraftingSystem
{




    public static void CraftButtonClicked(ObjectData item)
    {
        Debug.Log("called times 2");
        if (CheckRequirementsForCrafting(item))
        {

            Debug.Log("item should craft");
            CraftItem(item as ToolData);
        }
        else
        {
            Debug.Log("item should not craft");
        }

    }



    public static void CraftItem(ToolData item)
    {
        foreach (SubPrice price in item.price)
        {
            if (price.objectData.objectType == ObjectType.Resource && (price.objectData as ResourceData).resourceBehaviour.HasFlag(ResourceBehaviour.Containable))
            {
                InventorySystem.RemoveResInContainer(price.objectData as ResourceData, price.quantity);
            }
            else
            {
                InventorySystem.instance.Remove((ObjectData)price.objectData, price.quantity);
            }


        }
        Debug.Log("item crafted");
        if (InventorySpaceCheck())
        {
            InventorySystem.AddItem(item);
            PlayerProfileSystem.AddXp(5);
            //  item.Crafted();
            item.IncreaseInteractedAmmount();
            InventorySystem.ChangeDefaults(item);
         
        }
        else
        {
            foreach (SubPrice price in item.price)
            {
                if (price.objectData.objectType == ObjectType.Resource && (price.objectData as ResourceData).resourceBehaviour.HasFlag(ResourceBehaviour.Containable))
                {
                    InventorySystem.AddResInContainer(price.objectData as ResourceData, price.quantity);
                }
                else
                {
                    InventorySystem.AddItem((ObjectData)price.objectData, price.quantity);
                }

            }
        }
    }



    /// <summary>
    /// check if there is space in inventory to craft this item 
    /// </summary>
    /// <returns></returns>
    public static bool InventorySpaceCheck()
    {

        if (InventorySystem.HasSpace())
        {
            Debug.Log("inventory has space");
            return true;
        }
        else
        {
            return false;
        }

    }
    public static bool CheckRequirementsForCrafting(ObjectData item)
    {
        Debug.Log("called times 4");
        CraftableItemData craftableItem = (CraftableItemData)item;

        foreach (SubPrice pr in craftableItem.price)
        {
            if (!PriceTypeCheck(pr.objectData, pr.quantity))
            {
                Debug.Log("price check failed");
                return false;
            }

        }
        foreach (BuildingData buil in craftableItem.buildingsAOE)
        {
            if (!PriceTypeCheck(buil as ObjectData))
            {
                Debug.Log("price check failed");
                return false;
            }

        }
        return true;


    }

    public static bool CheckAvailableSubPriceForCraftingInInventory(ObjectData item, int quantity)
    {



        if (InventorySystem.HasItemWithAmmount((ObjectData)item, quantity))
        {

            Debug.Log("true here");
            return true;
        }
        else
        {
            if (item.objectType == ObjectType.Resource)
            {
                if (((ResourceData)item).resourceBehaviour.HasFlag(ResourceBehaviour.Containable))
                {
                    if (InventorySystem.HasResInContainer(((ResourceData)item), quantity))
                    {
                        return true;
                    }
                }
                return false;
            }
            else
            {
                Debug.Log("false here");
                return false;
            }

        }
    }

    public static bool CheckPlayerTrait()
    {

        return false;
    }

    public static bool PriceTypeCheck(ObjectData obj, int quantity = 1)
    {

        switch (obj.objectType)
        {
            case ObjectType.Resource:
                return CheckAvailableSubPriceForCraftingInInventory(obj as ObjectData, quantity);

            case ObjectType.Tool:
                return CheckAvailableSubPriceForCraftingInInventory(obj as ObjectData, quantity);

            case ObjectType.Building:
                return ConstructionSystem.IsPlayerInRangeBool((BuildingData)obj);

            case ObjectType.PlayerTrait:
                return PlayerAttributesSystem.CheckAvailablityOfTrait(obj as Traits, quantity);

                //  default:
                //   return CheckAvailableSubPriceForCraftingInInventory(obj as ObjectData, quantity);




        }
        return false;


    }


    [SerializeField] private static float _duration = 0f;
    private static float _timer = 0f;
    private static float _timer1 = 0f;
    public static float delay = 1f;
    public static float offset = 3f;
    public static void ToolUse(InventoryItem item)
    {
        if (((ToolData)item.data).backgroundColors.Length > 0)
        {
            LosingDurability(item);
        }


        //   ProcessContainer(resource, ((ToolData)item.data).HarvestSpeed, item);
    }
    public static void LosingDurability(InventoryItem item, float delay = 1)
    {




        // Debug.Log(_duration + "time  needed");

        _timer += Time.deltaTime;
        // Debug.Log(_timer + "duration has passsed");

        if (_timer >= delay)
        {
            item.CurrentDurability -= (item.data as ToolData).durabilityLossRate;

            // Debug.Log(_timer + "duration has passsed--------------------------------");
            _timer = 0f;
            ItemHolderUi itemUI = InventorySystem.GetItemHolderUi(item);
            Debug.Log(_timer + "durability loss duration has passsed");
            itemUI.DurabilityChangeCheck();




        }


    }


    public static bool ContainThisResource(Resource resource, int harvestSpeed)
    {
        ResourceData res = (ResourceData)resource.inventoryItem.data;
        //make sure that eveytime a new harvest speed is picked
        if (res.harvestingSpeed / harvestSpeed != _duration)
        {
            _duration = 0f;
        }
        _duration = res.harvestingSpeed / harvestSpeed;

        // Debug.Log(_duration + "time  needed");

        _timer1 += Time.deltaTime;
        // Debug.Log(_timer + "duration has passsed");
        if (_timer1 >= _duration)
        {

            // Debug.Log(_timer + "duration has passsed--------------------------------");
            _timer1 = 0f;



            return true;

        }

        return false;
    }


    public static bool ProcessContainer(Resource resource, int harvestSpeed, InventoryItem currentItem)
    {
        //if (ContainThisResource(resource, harvestSpeed))
        //{
        //    //only done for one containee for now



        if (currentItem.stacksize < ((ToolData)currentItem.data).contains.maxQuantity)
        {

            Debug.Log("container processed 2");
            currentItem.AddtoContainer();
            ItemHolderUi itemholder = InventorySystem.GetItemHolderUiWithLowestContainerStacksForTool(currentItem);
            if (itemholder != null)
            {
                currentItem.AddtoContainer();
                itemholder.UpdateItemDataToUi(currentItem);
            }
            if (((ResourceData)resource.inventoryItem.data).resourceBehaviour.HasFlag(ResourceBehaviour.Containable))
            {
                if (currentItem != null)
                {
                    Debug.Log("container processed");
                    return resource.LoseOneStack();
                    

                }


            }
        }
        else
        {
            return false;
        }

        return false;
        //}



    }

}
