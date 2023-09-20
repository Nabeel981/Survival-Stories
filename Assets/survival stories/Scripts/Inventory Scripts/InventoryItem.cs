using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

[System.Serializable]
public class InventoryItem 
{// be carefull when integrating with inventory could cause complexity and refrence issues

    public string objectID;
    [JsonIgnore] public ObjectData data;
    public int stacksize;
    public float CurrentDurability;
    public int containerStacks = 0;
    public string strKey;
 



    public delegate void ValueChangedEventHandler();
    public event ValueChangedEventHandler ValueChanged;
    public InventoryItem(ObjectData source)
    {
        Debug.Log(source.displayName);
        data = source;
        objectID = source.StringID;
        if (data.objectType == ObjectType.Tool)
        {
            CurrentDurability = ((ToolData)data).durability;
        }
        //     if (source.changeable)
        //{
        //    // if its changeable make a new copy of it and edit it
        //    // ask for these issues
        //}


        AddToStack();

    }


    public void AddtoContainer(int ammount = 1)
    {
        //Debug.Log("container processed 3");
        if (containerStacks < data.stackSize)
        {
            containerStacks += ammount;
            ValueChanged?.Invoke();
        }


    }
    public void RemoveFromContainer(int ammount = 1)
    {
        Debug.Log("popopopopopo");
        if (stacksize >= ammount)
        {
            Debug.Log("popopopopopo2222222222222222222222");
            stacksize -= ammount;
            ValueChanged?.Invoke();
        }

    }


    public void AddToStack(int ammount = 1)
    {
        if (!data.objectType.HasFlag(ObjectType.Tool))
        {
            InventorySystem.instance.ann.PlayAnimation("Gained " + data.displayName.ToString(), AnnouncementType.withImage, data.icon);
        }

        stacksize += ammount;
        
        ValueChanged?.Invoke();


    }


    //public void RemoveFromStack()
    //{

    //    stacksize--;
    //    ValueChanged?.Invoke();
    //}

    /// <summary>
    /// maybe add a check here too incase value goes into minus
    /// </summary>
    /// <param name="ammount"></param>
    public void RemoveFromStack(int ammount = 1)
    {
        if (stacksize >= ammount)
        {


            stacksize -= ammount;
           
            ValueChanged?.Invoke();

        }

    }


    public void IfHasContainerStacks()
    {


    }

    public void IsConsumeableItem()
    {

    }


}
