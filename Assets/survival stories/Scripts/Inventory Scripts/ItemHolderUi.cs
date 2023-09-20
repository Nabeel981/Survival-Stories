using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

public class ItemHolderUi : MonoBehaviour
{
    public Image ItemBackground;
    public Image ItemIcon;
    public TextMeshProUGUI Stacks;
    public InventoryItem item;
    public bool inUse = false;


    public int currentDurabilityIndex = 0;


    private void OnEnable()
    {
        if (item != null)
        {
            item.ValueChanged += HandleStackUpdated;
        }

    }

    public void UpdateItemDataToUi(InventoryItem itemToDisplay)
    {
        Debug.Log("container processed 4");
        item = itemToDisplay;
        itemToDisplay.ValueChanged += HandleStackUpdated;

        ItemIcon.sprite = item.data.icon;
        if (item.stacksize > 1)
        {
            Stacks.text = item.stacksize.ToString();
        }
        inUse = true;

        ToolContainerandDurabilityCheck();

    }


    public void ToolContainerandDurabilityCheck()
    {
        if (item.data.objectType == ObjectType.Tool)
        {


            if (((ToolData)item.data).backgroundColors.Length > 0)
            {
                ItemBackground.color = ((ToolData)item.data).backgroundColors[((ToolData)item.data).backgroundColors.Length - 1];
                DurabilityChangeCheck();

            }
        }

        if (item.data.objectType == ObjectType.Tool)
        {
            if (((ToolData)item.data).toolType.HasFlag(ToolType.Container) || ((ToolData)item.data).toolType.HasFlag(ToolType.Consumable))
            {
                AddButtonandToolUseScript();
                Debug.Log("container processed 5");
                ContainerStacksChangeCheck();

            }
        }

    }
    public void AddButtonandToolUseScript()
    {
        if (!GetComponent<AutoUseTool>())
        {
            AutoUseTool tool = this.gameObject.AddComponent<AutoUseTool>();

            //  ItemBackground.gameObject.AddComponent<Button>().onClick.AddListener(tool.AutoUseThis);
            TapCountChecker tap = ItemBackground.gameObject.AddComponent<TapCountChecker>();
            tap.TapCountEvent += tool.DoubleTapped;

        }


    }
    private void OnDisable()
    {
        // item.ValueChanged -= HandleStackUpdated;
    }
    public void DurabilityChangeCheck()
    {  // the player wont see unless we open the inventory so idk if we need to change this constantly
        Debug.Log("durability 3  loss duration has passsed");
        ToolData tool = ((ToolData)item.data);
        if (tool.backgroundColors.Length > 0)
        {
            if (item.CurrentDurability > 0)
            {
                int portionSize = ((ToolData)item.data).durability / tool.backgroundColors.Length - 1;
                int a = (int)item.CurrentDurability;
                int portionIndex = a / portionSize;
                Debug.LogWarning(" durability index is " + portionIndex);
                if (portionIndex >= 0 && portionIndex != tool.backgroundColors.Length)
                {

                    ItemBackground.color = tool.backgroundColors[portionIndex];
                }
            }
            else
            {

                Debug.Log("durability 2 loss duration has passsed");
                InventorySystem.instance.Remove(item);

            }
        }
    }
    public void ContainerStacksChangeCheck()
    {  // the player wont see unless we open the inventory so idk if we need to change this constantly
        Debug.Log("res in container stacks50000006");
        if (item.containerStacks <= ((ToolData)item.data).contains.maxQuantity)
        {
            // Debug.Log("container processed 6");
            // Debug.Log(item.data.displayName + " loook here mate");

            //  Debug.Log(newtool.contains.maxQuantity + " up and down -1" + newtool.contains.NewIcons.Count);
            float portionSize = ((ToolData)item.data).contains.maxQuantity / ((ToolData)item.data).contains.NewIcons.Count;

            // Debug.Log(portionSize + " this is the portion size");
            int portionIndex = item.containerStacks / (int)portionSize;
            Debug.Log("res in container stacks56 size" + portionIndex);
            //   Debug.LogWarning(" durability index is " + portionIndex);
            if (portionIndex >= 0 && portionIndex <= ((ToolData)item.data).contains.NewIcons.Count)
            {
                Debug.Log("res in container stacks56 1");
                ///    Debug.Log("icon changed here");

                Debug.Log("res in container stacks56 2");

                if (portionIndex == 0)
                {
                    ItemIcon.sprite = ((ToolData)item.data).contains.NewIcons[portionIndex];
                }
                else
                {
                    ItemIcon.sprite = ((ToolData)item.data).contains.NewIcons[portionIndex - 1];
                }


            }

        }


    }

    public void HandleStackUpdated()
    {
        //  Debug.Log("refresh inventory1111");
        if (item.stacksize > 1)
        {
            //  Debug.Log("refresh inventory133333333333333333333333333333");
            Stacks.text = item.stacksize.ToString();
        }
        else if (item.stacksize == 1)
        {
            Stacks.text = "";
        }
        ToolContainerandDurabilityCheck();
    }


}
