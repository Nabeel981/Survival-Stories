using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class ForagingSystem
{

    public delegate void ResourceGatherEvent(ObjectType type, float value, Transform parent);
    public static event ResourceGatherEvent resourceGather;
    // Make it the global function

    [SerializeField] private static float _duration = 0f;
    private static float _timer = 0f;
    public static float delay = 1f;
    public static float offset = 3f;
    public static bool makeObj = false;
    public static GameObject spawn;
    public static Vector2 pos;
    public static float totalElapsedTime;
    public static GameObject oldRes = null;
    public static int oldharvestSpeed = 0;

    public static bool GatherThisResource(Resource resource, int harvestSpeed)
    {
        ResourceData res = (ResourceData)resource.inventoryItem.data;
        //make sure that eveytime a new harvest speed is picked
        if (oldRes != resource.gameObject)
        {
            oldRes = resource.gameObject;
            _duration = 0f;
            _timer = 0f;
            totalElapsedTime = 0f;
        }


        _duration = res.harvestingSpeed / harvestSpeed;


        // Debug.Log(_duration + "time  needed");
        // Debug.Log("harvest speed of res " + res.harvestingSpeed + " tool speed" + harvestSpeed);
        _timer += Time.deltaTime;
        totalElapsedTime += Time.deltaTime;
        float sliderValue = HelperFunctions.NormalizeTotalTimeNFillSlider(resource.gameObject, _timer, resource.inventoryItem.stacksize, _duration);
        // Announcements.instance.StartFillingBar(ObjectType.Resource , sliderValue);
        resourceGather?.Invoke(ObjectType.Resource, sliderValue, resource.transform);
        // Debug.Log(_timer + "duration has passsed");
        if (_timer >= _duration)
        {

            // Debug.Log(_timer + "duration has passsed--------------------------------");
            _timer = 0f;









            return true;

            // will drink here but hasnt yet







        }

        return false;
    }

    public static void ShowResourceCollectedItemFall(Resource resource, GameObject spawn)
    {
        // GameObject spawn = new GameObject();
        // SpriteRenderer spriteR = spawn.AddComponent<SpriteRenderer>();

        SpriteRenderer spriteR = spawn.GetComponent<SpriteRenderer>();
        spriteR.sprite = resource.inventoryItem.data.icon;
        spriteR.sortingOrder = 1;


        spawn.transform.position = Vector2.Lerp(spawn.transform.position, pos, Time.deltaTime * .2f);
        if ((Vector2)spawn.transform.position == pos)
        {
            GameObject.Destroy(spawn);
            makeObj = false;
        }

        // 
    }

    public static void ResourceExpired(GameObject ResourceGameObject)
    {
        //resource expired
        ResourceGameObject.GetComponent<Collider2D>().enabled = false;
        ResourceGameObject.GetComponent<Renderer>().enabled = false;
        ResourceGameObject.GetComponent<Resource>().ResetValuesAfterTime(); ;
        /////  startc .  StartResetting(float delayInSeconds)

        // GameObject.Destroy(ResourceGameObject);
        //make sure to tell the map details that a resource was lost here so it can be respawned after a certain time
        //ask question as well
    }

    public static bool ProcessResource(GameObject resourceGameObject)
    {

        if (InventorySystem.HasSpaceAfterAdding((ResourceData)resourceGameObject.GetComponent<Resource>().inventoryItem.data))
        {

            return true;

        }
        else return false;


    }

    public static bool ProcessResource(Resource res)
    {
        if (!PlayerAttributesSystem.HasEnoughStamina())
        {
            return false;
        }
        ResourceData resData = ((ResourceData)res.inventoryItem.data);
        InventoryItem item = InventorySystem.GetToolForResource(((ResourceData)res.inventoryItem.data).resourcesType);
        Debug.Log("nom nom 1");
        if (resData.resourceBehaviour.HasFlag(ResourceBehaviour.Consumable) && DoYouNeedConsumption())
        {
            Debug.Log("nom nom 2");
            // stats system consume the resource
            // make a player statsSystem for this
            if (GatherThisResource(res, 1))
            {

                Debug.Log("nom nom 3");
                PlayerAttributesSystem.ConsumeResource(res);
                res.LoseOneStack();
                PlayerProfileSystem.AddXp(5);


            }
            return true;

        }
        else if (resData.resourceBehaviour.HasFlag(ResourceBehaviour.Containable))
        {
            Debug.Log("nom nom 4");
            if (item == null)
            {
                //do nothing
                Debug.Log("nom nom 5");
                return false;
            }
            else
            {
                Debug.Log("nom nom 6");

                if (GatherThisResource(res, ((ToolData)item.data).HarvestSpeed))
                {
                    Debug.Log("nom nom 7");
                    CraftingSystem.ProcessContainer(res, ((ToolData)item.data).HarvestSpeed, item);
                    PlayerProfileSystem.AddXp(5);
                }
                CraftingSystem.ToolUse(item);
                //contain the resource here
                return true;
            }
        }
        else if (resData.resourceBehaviour.HasFlag(ResourceBehaviour.Gatherable))
        {
            Debug.Log("nom nom 8");
            if (item == null)
            {
                Debug.Log("nom nom 9");
                //gather without tool here
                if (InventorySystem.HasSpaceAfterAdding(res.inventoryItem.data))
                {
                    if (GatherThisResource(res, 1))
                    {
                        Debug.Log("nom nom 10");
                        res.LoseOneStackAndAddInInventory();
                        PlayerProfileSystem.AddXp(5);
                        float randomValue = Random.Range(0f, 100f);
                        if (randomValue <= resData.doubleChance)
                        {
                            InventorySystem.AddItem(resData);
                        }

                    }
                    return true;
                }
                else
                {
                    return false;
                }

            }
            else
            {
                if (InventorySystem.HasSpaceAfterAdding(res.inventoryItem.data))
                {
                    if (GatherThisResource(res, ((ToolData)item.data).HarvestSpeed))
                    {
                        Debug.Log("nom nom 11");
                        res.LoseOneStackAndAddInInventory();
                    }

                    Debug.Log("nom nom 12");
                    CraftingSystem.ToolUse(item);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            //gather with tool here

        }

        else
        {
            return false;
        }
    }


    public static bool DoYouNeedConsumption()
    {
        //hard coded for thirst
        if (PlayerAttributesSystem.instance.thirst.currentQuantity < 700)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool CheckGatheringReruirments(ResourceData resource)
    {
        if (resource.gatheringRequirments.Length > 0)
        {
            return InventorySystem.HasGatheringItemInInventory(resource);
        }
        else
            return true;
    }
    public static void ResourceSpawned()
    {


    }

    public static void RemoveResource(Resource res)
    {
        res.gameObject.SetActive(false);

    }

}