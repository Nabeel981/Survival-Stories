using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ToolsSystem
{
    public static void ProcessTool(InventoryItem toolItem, Resource resource)
    {
      
            ToolUse(toolItem);


            ToolData tool = (ToolData)toolItem.data;

            if (tool.toolType.HasFlag(ToolType.Consumable))
            {
                //goto consume
            }
            if (tool.toolType.HasFlag(ToolType.Container))
            {
                ContainersFunctionality.ProcessContainer(resource, ((ToolData)toolItem.data).HarvestSpeed, toolItem);
                //goto contain

            }
        
       


    }


 //   [SerializeField] private static float _duration = 0f;
    private static float _timer = 0f;
    public static float delay = 1f;
    public static float offset = 3f;
    public static void ToolUse(InventoryItem item)
    {
        if (item != null)
        {
            LosingDurability(item);
        }

    }
    public static void LosingDurability(InventoryItem item, float delay = 1)
    {




        // Debug.Log(_duration + "time  needed");

        _timer += Time.deltaTime;
        // Debug.Log(_timer + "duration has passsed");
        if (_timer >= delay)
        {
            item.CurrentDurability -= 1;

            // Debug.Log(_timer + "duration has passsed--------------------------------");
            _timer = 0f;
            ItemHolderUi itemUI = InventorySystem.GetItemHolderUi(item);
            itemUI.DurabilityChangeCheck();




        }


    }
}
