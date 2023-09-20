using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;
using System.Reflection;
using Newtonsoft.Json;

//later think about seperating tools and resources to save some computation 

public class InventorySystem : MonoBehaviour, ISaveable
{
    [JsonIgnore]
    public static InventorySystem instance;

    [JsonIgnore] public static List<EffectsOnPLayer> effectsOnPlayer = new List<EffectsOnPLayer>();

    [JsonIgnore] public Announcements ann;
    [JsonIgnore] public CurrentDefaults _currentDefaults;

    [JsonIgnore] public GlobalItemHolder allItemsLibrary;


    [JsonIgnore] public bool isIdle = false;

    // for test
    [JsonIgnore] public ObjectData sword;
    [JsonIgnore] public ObjectData stone;
    [JsonIgnore] public ObjectData wood;
    [JsonIgnore] public int expandBy;
    // use this to save the stuff in the respective slots and cap the total slots for the inventory


    //
    /// <summary>
    /// for inventory UI
    /// </summary>
    /// 

    [JsonIgnore] public Transform inventoryContentHolder;
    [JsonIgnore] public GameObject contentPrefab;


    /// <summary>
    /// Main inventory functionality work
    /// </summary>
    [JsonIgnore] private static Dictionary<InventoryItem, ObjectData> itemDictionary;


    public static List<InventoryItem> inventory = new List<InventoryItem>();
    [JsonIgnore] public static List<ItemHolderUi> inventoryUiList = new List<ItemHolderUi>() { };
    [JsonIgnore] private static Dictionary<InventoryItem, ItemHolderUi> itemUIDictionary;
    public static int inventorySize;
    [JsonIgnore] public bool isfull = false;


    /// <summary>
    /// event to add resources in the crafting area
    /// </summary>
    /// 

    public delegate void ItemsAdded();
    public static event ItemsAdded itemsEvent;


    private void OnEnable()
    {
        TempResetSO();
        if (!instance) instance = this;
        LoadData();
        itemsEvent += SaveData;
    }
    private void Awake()
    {

        //remove later
        ExpandInventory();
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////

        // inventory = new List<InventoryItem>();
        itemDictionary = new Dictionary<InventoryItem, ObjectData>();

        itemUIDictionary = new Dictionary<InventoryItem, ItemHolderUi>();
        UpdateInventorySize();
    }
    public void TempResetSO()
    {
        foreach (var item in _currentDefaults.currentBuildingsInfo)
        {
            item.currentlyPlaced = 0;
            item.DurationPassed = 0;
            item.effectInRange = false;

        }
        foreach (var item in _currentDefaults.items)
        {
            item.tool = _currentDefaults.items[0].tool;
        }
    }

    private void Start()
    {

        // RefreshInventory();
    }
    [ContextMenu(" run this and check ")]
    public void RefreshInventory()
    {
        foreach (var item in itemUIDictionary)
        {
            Debug.Log("refresh inventory was cllaed");
            item.Value.HandleStackUpdated(); ;
        }
    }
    public void ExpandInventory()
    {
        inventorySize += expandBy;
        UpdateInventorySize();


    }
    public void AddItem(bool item)
    {

        if (item)
        {
            AddStackable(stone);
        }
        else
        {
            AddStackable(wood, 2);
        }

    }




    public void RemoveItem(bool item)
    {
        if (item)
        {
            Remove(stone);
        }
        else
        {
            Remove(wood, 2);
        }
    }
    public void IncreaseSlots()
    {



    }
    public void UpdateInventorySize()
    {
        int add = inventorySize - inventoryUiList.Count;
        for (int i = 0; i < add; i++)
        {
            //   Debug.Log("should have instansiated here");
            // instansaiated n component extracted and then added to list
            inventoryUiList.Add(Instantiate(contentPrefab, inventoryContentHolder).GetComponent<ItemHolderUi>());
        }

        // fo this in the saving part
        //get data from saved place 
        //turn data into inventory data before
        // foreach(inventory)
        // inventory.Add(inventory)
    }


    public InventoryItem Get(ObjectData refrenceObject)
    {
        //if (itemDictionary.TryGetValue(refrenceObject, out InventoryItem item))
        //{
        //    return item;
        //}
        return null;
    }

    public static void AddItem(ObjectData refrenceObject, InventoryItem item, int ammount = 1)
    {


    }
    public static void AddItem(ObjectData refrenceObject, int ammount = 1)
    {

        if (refrenceObject.objectType == ObjectType.Resource)
        {
            AddStackable(refrenceObject, ammount);

        }
        else if (refrenceObject.objectType == ObjectType.Tool)
        {
            AddNonStackable(refrenceObject, ammount);
        }
        itemsEvent?.Invoke();

    }
    public static void AddInContainer()
    {

    }
    public static void AddNonStackable(ObjectData refrenceObject, int ammount = 1)
    {

        MakeNewItemSlot(refrenceObject);

    }
    public void CanAdd()
    {


    }
    public static void AddStackable(ObjectData refrenceObject, int ammount = 1)
    {
        if (HasSpaceAfterAdding(refrenceObject))
        {


            bool makenew = true;
            /// this function needs refactoring
            /// 
            if (itemDictionary.ContainsValue(refrenceObject))
            {
                //var key = FindKeyByValue<InventoryItem, ObjectData>(itemDictionary, refrenceObject);

                int toAdd = 0;

                foreach (KeyValuePair<InventoryItem, ObjectData> keyValue in itemDictionary)
                {
                    if (keyValue.Value == refrenceObject)
                    {
                        if (refrenceObject.stackSize > keyValue.Key.stacksize)
                        {


                            ///empty slots
                            int reminingMax = refrenceObject.stackSize - keyValue.Key.stacksize;
                            //slots to be filled
                            toAdd = reminingMax - ammount;
                            if (toAdd >= 0)
                            {
                                makenew = false;
                                Debug.Log("1");
                                keyValue.Key.AddToStack(ammount);

                                itemsEvent?.Invoke();
                            }
                            else
                            {

                                makenew = false;
                                Debug.Log("2");
                                keyValue.Key.AddToStack(reminingMax);
                                itemsEvent?.Invoke();
                            }





                        }









                    }
                }
                if (toAdd < 0)
                {

                    MakeNewItemSlot(refrenceObject);
                    int newadd = toAdd * -1;
                    newadd -= 1;
                    AddItem(refrenceObject, newadd);
                    itemsEvent?.Invoke();

                }
                if (makenew)
                {
                    Debug.Log("3");
                    MakeNewItemSlot(refrenceObject);
                    int k = ammount - 1;
                    AddItem(refrenceObject, k);
                    itemsEvent?.Invoke();
                }


            }
            else
            {
                Debug.Log("4");
                MakeNewItemSlot(refrenceObject);
                int i = ammount - 1;
                AddItem(refrenceObject, i);
                itemsEvent?.Invoke();
            }
        }


    }



    public static bool HasGatheringItemInInventory(ResourceData resource)
    {
        foreach (ObjectData obj in resource.gatheringRequirments)
        {
            if (itemDictionary.ContainsValue(obj))
            {
                //  AddNonStackable(sword);
                Debug.Log("inventory does not have required Item :" + obj.name);

                return true;
            }
        }
        return false;

    }

    public static void MakeNewItemSlot(ObjectData refrenceObject)
    {


        if (inventory.Count < inventorySize)
        {

            foreach (ItemHolderUi itemUI in inventoryUiList)
            {
                if (!itemUI.inUse)
                {
                    Debug.Log("new InventoryItem Made in all the data Structures");
                    InventoryItem newItem = new InventoryItem(refrenceObject);

                    inventory.Add(newItem);

                    itemDictionary.Add(newItem, refrenceObject);
                    itemUI.UpdateItemDataToUi(newItem);
                    //  itemUI.item = newItem   
                    itemUIDictionary.Add(itemUI.item, itemUI);
                    InventorySystem.instance.ann.PlayAnimation("Gained " + refrenceObject.displayName.ToString(), AnnouncementType.withImage, refrenceObject.icon);
                    return;
                }
            }
        }
        else
        {
            Debug.Log("all the slots are taken in the inventory INCREASE SPACE");
        }

    }

    /// <summary>
    /// might need to upgrade the function and add a quantity to remove
    /// </summary>
    /// <param name="refrenceObject"></param>
    //public void Remove(ObjectData refrenceObject)
    //{
    //    if (itemDictionary.ContainsValue(refrenceObject))
    //    {
    //        InventoryItem tempInventoryItem;
    //        tempInventoryItem = FindKeyByValue<InventoryItem, ObjectData>(itemDictionary, refrenceObject);
    //        foreach (KeyValuePair<InventoryItem, ObjectData> keyValue in itemDictionary)
    //        {
    //            if (keyValue.Value == refrenceObject && refrenceObject.stackSize >= keyValue.Key.stacksize)
    //            {


    //                if (tempInventoryItem.stacksize > keyValue.Key.stacksize)
    //                {
    //                    tempInventoryItem = keyValue.Key;
    //                }

    //            }
    //        }

    //        tempInventoryItem.RemoveFromStack();



    //        ///checking if all stacks are used up
    //        if (tempInventoryItem.stacksize <= 0)
    //        {
    //            inventory.Remove(tempInventoryItem);
    //            if (itemUIDictionary.TryGetValue(tempInventoryItem, out ItemHolderUi holderUi))
    //            {

    //                inventoryUiList.Remove(holderUi);
    //                Destroy(holderUi.gameObject);
    //                UpdateInventorySize();
    //                // GameObject uiItem = Instantiate(contentPrefab, inventoryContentHolder);
    //                // itemUIDictionary.Add(uiItem.GetComponent<ItemHolderUi>().item, uiItem.GetComponent<ItemHolderUi>());
    //            }
    //            itemDictionary.Remove(tempInventoryItem);

    //        }
    //    }


    //}

    //public void Remove(ObjectData refrenceObject, int ammount = 1)
    //{
    //    Debug.Log(ammount + " must rtrremove");
    //    if (itemDictionary.ContainsValue(refrenceObject))
    //    {
    //        int remains = 0;
    //        InventoryItem tempInventoryItem;
    //         tempInventoryItem = FindKeyByValue<InventoryItem, ObjectData>(itemDictionary, refrenceObject);

    //        foreach (KeyValuePair<InventoryItem, ObjectData> keyValue in itemDictionary)
    //        {

    //            if (keyValue.Value == refrenceObject)
    //            {
    //                Debug.Log(keyValue.Value.stackSize + refrenceObject.stackSize + " comparison");
    //                Debug.Log(keyValue.Value.stackSize + refrenceObject.stackSize + " comparison");

    //                if (refrenceObject.stackSize > keyValue.Key.stacksize)

    //                {

    //                    tempInventoryItem = keyValue.Key;




    //                    if (keyValue.Key.stacksize >= ammount)
    //                    {

    //                        Debug.Log("removed all " + ammount);
    //                        keyValue.Key.RemoveFromStack(ammount);
    //                    }
    //                    else
    //                    {

    //                        remains = ammount - keyValue.Key.stacksize;


    //                        Debug.Log("currently removing " + keyValue.Key.stacksize);
    //                        keyValue.Key.RemoveFromStack(keyValue.Key.stacksize);
    //                    }




    //                }
    //            }
    //            else if (refrenceObject.stackSize == keyValue.Key.stacksize)
    //            {


    //                tempInventoryItem = keyValue.Key;

    //                if (keyValue.Key.stacksize >= ammount)
    //                {

    //                    keyValue.Key.RemoveFromStack(ammount);
    //                }
    //                else
    //                {

    //                    remains = ammount - keyValue.Key.stacksize;



    //                    keyValue.Key.RemoveFromStack(keyValue.Key.stacksize);
    //                }




    //            }
    //            else if (refrenceObject.stackSize < keyValue.Key.stacksize)
    //            {



    //                tempInventoryItem = keyValue.Key;

    //                if (keyValue.Key.stacksize >= ammount)
    //                {


    //                    keyValue.Key.RemoveFromStack(ammount);
    //                }
    //                else
    //                {

    //                    remains = ammount - keyValue.Key.stacksize;



    //                    keyValue.Key.RemoveFromStack(keyValue.Key.stacksize);
    //                }






    //            }
    //            else
    //            {

    //            }


    //        }

    //        if (tempInventoryItem.stacksize <= 0)
    //        {
    //            inventory.Remove(tempInventoryItem);
    //            if (itemUIDictionary.TryGetValue(tempInventoryItem, out ItemHolderUi holderUi))
    //            {

    //                inventoryUiList.Remove(holderUi);
    //                Destroy(holderUi.gameObject);
    //                UpdateInventorySize();
    //                // GameObject uiItem = Instantiate(contentPrefab, inventoryContentHolder);
    //                // itemUIDictionary.Add(uiItem.GetComponent<ItemHolderUi>().item, uiItem.GetComponent<ItemHolderUi>());
    //            }
    //            itemDictionary.Remove(tempInventoryItem);

    //        }
    //        if (remains != 0 && remains > 0)
    //        {
    //            Debug.Log("Remains to remove :" + remains);
    //            Remove(refrenceObject, remains);
    //        }

    //        ///checking if all stacks are used up


    //        else
    //        {
    //            Debug.Log(" didnt even go  in the if");
    //        }

    //    }

    //}

    public void Remove(ObjectData refrenceObject, int ammount = 1)
    {

        int remains = 0;
        if (itemDictionary.ContainsValue(refrenceObject))
        {
            Debug.Log("ammount to remove is now :" + ammount);
            InventoryItem tempInventoryItem;
            tempInventoryItem = FindKeyByValue<InventoryItem, ObjectData>(itemDictionary, refrenceObject);
            foreach (KeyValuePair<InventoryItem, ObjectData> keyValue in itemDictionary)
            {
                if (keyValue.Value == refrenceObject && refrenceObject.stackSize >= keyValue.Key.stacksize)
                {
                    Debug.Log("ammount to remove is now :" + ammount);

                    if (tempInventoryItem.stacksize > keyValue.Key.stacksize)
                    {
                        tempInventoryItem = keyValue.Key;
                    }

                }
            }
            if (tempInventoryItem.stacksize >= ammount)
            {
                Debug.Log("ammount to remove is now :" + ammount);
                tempInventoryItem.RemoveFromStack(ammount);
                ammount = 0;

            }
            else
            {
                Debug.Log("ammount to remove is now :" + ammount);
                remains = ammount - tempInventoryItem.stacksize;
                tempInventoryItem.RemoveFromStack(tempInventoryItem.stacksize);
            }




            ///checking if all stacks are used up
            if (tempInventoryItem.stacksize <= 0)
            {
                Debug.Log("ammount to remove is now :" + ammount);
                inventory.Remove(tempInventoryItem);
                if (itemUIDictionary.TryGetValue(tempInventoryItem, out ItemHolderUi holderUi))
                {
                    itemUIDictionary.Remove(tempInventoryItem);
                    Debug.Log("ammount to remove is now :" + ammount);
                    inventoryUiList.Remove(holderUi);
                    Destroy(holderUi.gameObject);
                    UpdateInventorySize();
                    // GameObject uiItem = Instantiate(contentPrefab, inventoryContentHolder);
                    // itemUIDictionary.Add(uiItem.GetComponent<ItemHolderUi>().item, uiItem.GetComponent<ItemHolderUi>());
                }
                itemDictionary.Remove(tempInventoryItem);

            }
            if (remains != 0)
            {
                Debug.Log("ammount to remove is now :" + ammount);
                Remove(refrenceObject, remains);
            }

        }
        itemsEvent?.Invoke();

    }



    public void Remove(InventoryItem item)
    {

        inventory.Remove(item);
        if (itemUIDictionary.TryGetValue(item, out ItemHolderUi holderUi))
        {

            inventoryUiList.Remove(holderUi);
            itemUIDictionary.Remove(item);

            Destroy(holderUi.gameObject);

            UpdateInventorySize();
            // GameObject uiItem = Instantiate(contentPrefab, inventoryContentHolder);
            // itemUIDictionary.Add(uiItem.GetComponent<ItemHolderUi>().item, uiItem.GetComponent<ItemHolderUi>());
        }
        itemDictionary.Remove(item);

    }


    #region Ease Of Life Functions

    // you can make your own data using the <> after the function ..the parameters will then use these
    private TKey FindKeyByValue<TKey, TValue>(Dictionary<TKey, TValue> dictionary, TValue value)
    {
        foreach (KeyValuePair<TKey, TValue> pair in dictionary)
        {
            if (EqualityComparer<TValue>.Default.Equals(pair.Value, value))
            {
                return pair.Key;
            }
        }

        // Value not found in the dictionary
        return default;
    }

    public static bool HasSpaceAfterAdding(ObjectData ItemData, int quantity = 1)
    {
        if (HasSpace())
        {
            return true;

        }
        else
        {


            if (itemDictionary.ContainsValue(ItemData))
            {

                foreach (KeyValuePair<InventoryItem, ObjectData> keyValue in itemDictionary)
                {
                    if (keyValue.Value == ItemData && ItemData.stackSize >= keyValue.Key.stacksize + quantity)
                    {
                        return true;
                    }
                }
                return false;
            }
            return false;
        }
    }

    public static bool HasSpace()
    {
        if (inventory.Count < inventorySize)
        {
            return true;
        }
        else
            return false;
    }
    public static ItemHolderUi GetItemHolderUi(InventoryItem item)
    {
        foreach (var pair in itemUIDictionary)
        {
            if (pair.Key == item)
            {
                return pair.Value;
            }
        }

        return null;

    }
    public static ItemHolderUi GetItemHolderUiWithLowestContainerStacksForTool(InventoryItem item)
    {
        foreach (var pair in itemUIDictionary)
        {
            if (pair.Key == item && ((ToolData)item.data).contains.maxQuantity > item.containerStacks)
            {
                return pair.Value;
            }
        }

        return null;

    }

    public static bool HasItemWithAmmount(ObjectData obj, int ammount)
    {

        if (!itemDictionary.ContainsValue(obj))
        {
            //  AddNonStackable(sword);
            Debug.Log("inventory does not have required Item :" + obj.name);
            return false;

        }
        int a = 0;
        foreach (KeyValuePair<InventoryItem, ObjectData> keyValue in itemDictionary)
        {
            if (keyValue.Value == obj)
            {
                a += keyValue.Key.stacksize;
            }


        }

        Debug.LogWarning("Asked ammount :" + ammount + " found ammount :" + a);
        if (a >= ammount)
        {
            return true;
        }
        else
        {
            return false;
        }


    }
    [ContextMenu("get wood quantity")]
    public void GetWoodQuantity()
    {
        Debug.Log("script ran");
        Debug.Log(GetItemQuantity(wood) + "wood available");



    }
    public static int gatheringSpeedNullCheck(InventoryItem invItem)
    {
        if (invItem == null)
        {
            return InventorySystem.instance._currentDefaults.items[0].tool.HarvestSpeed;
        }
        else
        {
            return ((ToolData)invItem.data).HarvestSpeed;
        }

    }
    public static int GetItemQuantity(ObjectData obj)
    {

        int ammount = 0;
        if (!itemDictionary.ContainsValue(obj))
        {

            //  Debug.Log("inventory does not have required Item :" + obj.name);
            return 0;
        }
        else
        {


            foreach (KeyValuePair<InventoryItem, ObjectData> keyValue in itemDictionary)
            {
                if (keyValue.Value == obj)
                {
                    ammount += keyValue.Key.stacksize;
                }


            }
            //   Debug.Log(obj.displayName + " found in inventory " + ammount);
            return ammount;
        }


    }
    #endregion
    public static int GetItemQuantityInContainers(ObjectData obj)
    {

        int ammount = 0;


        foreach (KeyValuePair<InventoryItem, ObjectData> keyValue in itemDictionary)
        {
            if (keyValue.Value.objectType == ObjectType.Tool)
            {
                if ((keyValue.Value as ToolData).contains.data == obj)
                {
                    ammount += keyValue.Key.containerStacks;
                }
            }


        }
        //   Debug.Log(obj.displayName + " found in inventory " + ammount);
        return ammount;



    }

    public static void ChangeDefaults(ToolData tool = null)
    {
        if (tool != null)
        {
            if (tool.res.HasFlag(ResourcesType.none))
            {
                return;
            }
            foreach (var eq in InventorySystem.instance._currentDefaults.items)
            {

                Debug.LogWarning(eq.resourcesType);

                if ((tool.res.HasFlag(eq.resourcesType)))
                {

                    if (tool.HarvestSpeed > eq.tool.HarvestSpeed)
                    {
                        eq.tool = tool;
                    }
                }

            }
        }
        else
        {
            foreach (KeyValuePair<InventoryItem, ObjectData> item in itemDictionary)
            {
                if (item.Value.objectType == ObjectType.Tool)
                {
                    ToolData newTool = (ToolData)item.Value;
                    foreach (var eq in InventorySystem.instance._currentDefaults.items)
                    {
                        if (eq.resourcesType != ResourcesType.none)
                        {


                            if (newTool.res.HasFlag(eq.resourcesType))
                            {
                                if (!HasItemWithAmmount(eq.tool, 1))
                                {

                                    eq.tool = newTool;

                                }
                                else
                                {
                                    if (newTool.HarvestSpeed > eq.tool.HarvestSpeed)
                                    {
                                        eq.tool = newTool;
                                    }

                                }

                            }
                        }
                    }
                }


            }
        }
    }

    public static InventoryItem GetToolForResource(ResourcesType res)
    {
        foreach (var item in InventorySystem.instance._currentDefaults.items)
        {
            if (res == item.resourcesType)
            {
                InventoryItem itemInventory;
                if (item.tool.toolType.HasFlag(ToolType.Container))
                {
                    itemInventory = GetItemWithLowestContainerStacks(item.tool);
                }
                else
                {
                    itemInventory = GetToolWithLowestDurability(item.tool);
                }


                if (itemInventory == null)
                {
                    return null;
                }
                else
                {
                    return itemInventory;
                }

            }


        }

        return null;
    }

    private static InventoryItem GetToolWithLowestDurability(ToolData item)
    {
        //  InventoryItem inventoryItem = new InventoryItem(item);
        if (!itemDictionary.ContainsValue(item))
        {

            Debug.Log("inventory does not have required Item :" + item.name);
            InventorySystem.ChangeDefaults();
            return null;
        }
        else
        {


            foreach (KeyValuePair<InventoryItem, ObjectData> keyValue in itemDictionary)
            {
                if (keyValue.Value == item)
                {

                    if (keyValue.Key.CurrentDurability < ((ToolData)keyValue.Value).durability)
                    {
                        return keyValue.Key;
                    }




                }


            }
            foreach (KeyValuePair<InventoryItem, ObjectData> keyValue in itemDictionary)
            {
                if (keyValue.Value == item)
                {


                    return keyValue.Key;




                }


            }



        }
        return null;
    }
    private static InventoryItem GetItemWithLowestContainerStacks(ToolData item)
    {
        InventoryItem inventoryItem = new InventoryItem(item);
        if (!itemDictionary.ContainsValue(item))
        {

            Debug.Log("inventory does not have required Item :" + item.name);

            InventorySystem.ChangeDefaults();


            return null;
        }
        else
        {


            foreach (KeyValuePair<InventoryItem, ObjectData> keyValue in itemDictionary)
            {
                if (keyValue.Value == item)
                {

                    if (keyValue.Key.containerStacks < ((ToolData)keyValue.Value).contains.maxQuantity)
                    {
                        return keyValue.Key;
                    }




                }


            }




        }
        return null;
    }

    public static T DuplicateScriptableObject<T>(T original) where T : ScriptableObject
    {
        T duplicate = ScriptableObject.CreateInstance<T>();

        // Get all the fields of the ScriptableObject type
        FieldInfo[] fields = typeof(T).GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

        // Copy the values from the original to the duplicate
        foreach (FieldInfo field in fields)
        {
            field.SetValue(duplicate, field.GetValue(original));
        }

        // Return the duplicate
        return duplicate;

    }

    public void RemoveFromContainer(InventoryItem data, int ammount = 1)
    {
        if (data.containerStacks > 0)
        {
            data.containerStacks -= ammount;
            GetItemHolderUi(data);
            GetItemHolderUi(data).ToolContainerandDurabilityCheck();
        }




    }
    public static bool HasResInContainer(ResourceData res, int ammount)
    {
        int resFound = 0;
        foreach (var item in itemUIDictionary)
        {
            if (item.Key.data.objectType == ObjectType.Tool)
            {
                ToolData tool = item.Key.data as ToolData;
                if (tool.toolType.HasFlag(ToolType.Container))
                {
                    if (tool.contains.data == res)
                    {
                        resFound += item.Key.containerStacks;
                        if (resFound >= ammount)
                        {
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }


    public static void AddResInContainer(ResourceData res, int ammount = 1)
    {

        Debug.Log("res in container");


        int toAdd = 0;

        foreach (var item in itemUIDictionary)
        {
            if (item.Key.data.objectType == ObjectType.Tool)
            {
                Debug.Log("res in container 1 ");
                ToolData tool = item.Key.data as ToolData;
                if (tool.toolType.HasFlag(ToolType.Container))
                {
                    Debug.Log("res in container2");
                    if (tool.contains.data == res)
                    {
                        Debug.Log("res in container3");
                        if (item.Key.data.stackSize > item.Key.containerStacks)
                        {
                            Debug.Log("res in container 4");

                            ///empty slots
                            int reminingMax = item.Key.data.stackSize - item.Key.containerStacks;
                            //slots to be filled
                            toAdd = reminingMax - ammount;
                            if (toAdd >= 0)
                            {

                                //Debug.Log("1");
                                item.Key.AddtoContainer(ammount);

                                itemsEvent?.Invoke();
                            }
                            else
                            {


                                //Debug.Log("2");
                                item.Key.AddtoContainer(reminingMax);
                                itemsEvent?.Invoke();
                            }


                            item.Value.ContainerStacksChangeCheck();



                        }









                    }
                }
            }
        }









    }

    public static InventoryItem HasSword()
    {
        if (HasItemWithAmmount(InventorySystem.instance.sword, 1))
        {
            return InventorySystem.GetToolWithLowestDurability(InventorySystem.instance.sword as ToolData);
        }
        return null;
    }
    public static bool RemoveResInContainer(ResourceData res, int ammount)
    {
        int resRemoved = ammount;
        foreach (var item in itemUIDictionary)
        {
            if (item.Key.data.objectType == ObjectType.Tool)
            {
                ToolData tool = item.Key.data as ToolData;
                if (tool.toolType.HasFlag(ToolType.Container))
                {
                    if (tool.contains.data == res)
                    {
                        if (item.Key.containerStacks > 0)
                        {
                            if (item.Key.containerStacks >= resRemoved)
                            {
                                item.Key.containerStacks -= resRemoved;
                                resRemoved = 0;
                                return true;
                            }
                            else if (item.Key.containerStacks <= resRemoved)
                            {
                                int rem = item.Key.containerStacks;
                                item.Key.containerStacks -= rem;
                                resRemoved -= rem;
                            }

                            if (resRemoved == 0)
                            {
                                return true;
                            }


                        }
                    }
                }
            }
        }

        return false;
    }


    public static bool AutoUseTheToolsInInventory(Traits trait)
    {
        foreach (var item in itemUIDictionary)
        {
            if (item.Key.data.objectType == ObjectType.Tool)
            {
                if (item.Value.TryGetComponent<AutoUseTool>(out AutoUseTool tool))
                {
                    if ((item.Key.data as ToolData).useType.HasFlag(UseType.AutoUse))
                    {
                        if ((item.Key.data as ToolData).toolEffects[0].effectedTraitsData[0] == trait)
                        {
                            tool.DoubleTapped();
                            return true;
                        }
                    }
                }
            }

        }
        return false;
    }
    public static bool AutoConsumeResInInventory(Traits trait)
    {
        foreach (var item in itemUIDictionary)
        {
            if (item.Key.data.objectType == ObjectType.Resource)
            {
                if ((item.Key.data as ResourceData).effects != null && (item.Key.data as ResourceData).resourceBehaviour.HasFlag(ResourceBehaviour.Consumable))
                {



                    if ((item.Key.data as ResourceData).effects[0].effectedTraitsData[0] == trait)
                    {
                        ConsumeResourceForTrait(item.Key.data as ResourceData, trait);
                        return true;
                    }
                }

            }

        }
        return false;
    }
    public static void ConsumeResourceForTrait(ResourceData data, Traits trait)
    {



        InventorySystem.instance.Remove(data, 1);
        trait.currentQuantity += data.effects[0].ammount;




    }


    [ContextMenu("save")]
    public void SaveData()
    {
        List<InventoryItemData> inv = new List<InventoryItemData>();
        foreach (var item in inventory)
        {
            InventoryItemData newItem = new InventoryItemData();
            newItem.containerStacks = item.containerStacks;
            newItem.CurrentDurability = item.CurrentDurability;
            newItem.data = item.data;
            newItem.objectID = item.objectID;
            newItem.stacksize = item.stacksize;
            inv.Add(newItem);
        }

        string st = JsonConvert.SerializeObject(inv);
        Debug.Log(st);

        PlayerPrefs.SetString("inventory", st);
        PlayerPrefs.SetInt("inventorySize", inventorySize);



    }


    [ContextMenu("load")]
    public void LoadData()
    {
        if (PlayerPrefs.HasKey("inventory") && PlayerPrefs.HasKey("inventorySize"))
        {
            string st = PlayerPrefs.GetString("inventory");
            inventorySize = PlayerPrefs.GetInt("inventorySize");

            List<InventoryItemData> inv = JsonConvert.DeserializeObject<List<InventoryItemData>>(st);
            Debug.Log(inv);
            UpdateInventorySize();
            MakeInventory(inv);
        }
    }

    //[Serializable]
    //public class data {
    //    public List<dataIndise> x = new List<dataIndise>();
    //}
    //[Serializable]

    //public class dataIndise {
    //    public int a = 0;
    //    public dataIndise(int j)
    //    {
    //        a = j;
    //    }
    //}

    //[ContextMenu("test")]
    //public void Test()
    //{
    //    data d = new data();
    //    d.x = new List<dataIndise>();
    //    d.x.Add(new dataIndise(3));
    //    string s = JsonConvert.SerializeObject(d);
    //    data f = JsonConvert.DeserializeObject<data>(s);
    //    Debug.Log(f.x[0].a);
    //}


    public void MakeInventory(List<InventoryItemData> inv)
    {
        foreach (var item in inv)
        {
            switch (item.objectID.Substring(0, 1))
            {
                case "R":
                    foreach (var res in allItemsLibrary.allResources)
                    {
                        if (res.StringID == item.objectID)
                        {
                            Debug.Log(res.StringID + " " + item.objectID);
                            Debug.Log(res);
                            Debug.Log(res.displayName);
                            AddItem(res, item.stacksize);


                            //  item.data = res;
                        }
                    }
                    break;
                case "T":
                    foreach (var toolData in allItemsLibrary.allTools)
                    {
                        if (toolData.StringID == item.objectID)
                        {
                            Debug.Log(toolData.StringID + " " + item.objectID);
                            Debug.Log(toolData);
                            Debug.Log(toolData.displayName);
                            item.data = toolData;
                            AddItem(toolData, item.stacksize);
                            if (item.containerStacks > 0)
                            {
                                AddResInContainer((toolData.contains.data as ResourceData), item.containerStacks);
                            }
                        }
                    }
                    break;
                default:
                    break;
            }

        }

    }


}
[System.Serializable]
public class InventoryItemData
{
    public string objectID;
    [JsonIgnore] public ObjectData data;
    public int stacksize;
    public float CurrentDurability;
    public int containerStacks;
}