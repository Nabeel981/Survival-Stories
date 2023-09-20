using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ContainersFunctionality
{
    [SerializeField] private static float _duration = 0f;
    private static float _timer = 0f;
    public static float delay = 1f;
    public static float offset = 3f;
    public static bool makeObj = false;
    public static GameObject spawn;
    public static Vector2 pos;


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

        _timer += Time.deltaTime;
        // Debug.Log(_timer + "duration has passsed");
        if (_timer >= _duration)
        {

            // Debug.Log(_timer + "duration has passsed--------------------------------");
            _timer = 0f;



            return true;

        }

        return false;
    }

    public static void ProcessContainer(Resource resource, int harvestSpeed, InventoryItem currentItem)
    {
        //if (ContainThisResource(resource, harvestSpeed))
        //{
        //only done for one containee for now
        if (currentItem.stacksize < ((ToolData)currentItem.data).contains.maxQuantity)
        {
            Debug.Log("container processed 2");
            InventorySystem.GetItemHolderUi(currentItem);
            currentItem.AddtoContainer();
            if (((ResourceData)resource.inventoryItem.data).resourceBehaviour.HasFlag(ResourceBehaviour.Containable))
            {
                if (currentItem != null)
                {
                    Debug.Log("container processed");
                    resource.LoseOneStack();
                   
                }

              
            }
        }
        else
        {
            return;
        }
       

    }
}
