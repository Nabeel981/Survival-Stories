using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class Resource : MonoBehaviour, ISaveable
{
    [JsonIgnore] public InventoryItem inventoryItem;
    // public GameObject prefabRefreance;
    public int originalStackSize;
    public float resetTime;
    public float timeLeftForReset;
    [JsonIgnore] string strKey;

    private void OnEnable()
    {
        originalStackSize = inventoryItem.stacksize;
        strKey = gameObject.name.ToString() + this.GetType().Name;
        LoadData();

    }

    private void OnDisable()
    {
        SaveData();
    }
    private void Start()
    {


        if (timeLeftForReset > 0)
        {
            ResourceExpired();

        }
    }

    public void ResourceExpired()
    {
        //resource expired
        SaveData();
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Renderer>().enabled = false;
        StartCoroutine(StartResetting(resetTime));

    }
    public bool LoseOneStackAndAddInInventory()
    {
        Debug.Log("came in item");

        if (inventoryItem.data.objectType == ObjectType.Resource)
        {
            Debug.Log("Added item");
            ResourceData data = (ResourceData)inventoryItem.data;
            if (data.res.Length > 0)
            {
                ObjectData a = data.res[Random.Range(0, data.res.Length)];

                InventorySystem.AddItem(a);
                a.IncreaseInteractedAmmount();
            }
            else
            {
                InventorySystem.AddItem(inventoryItem.data);


                inventoryItem.data.IncreaseInteractedAmmount();

            }

        }
        SaveData();
        inventoryItem.RemoveFromStack();
        if (inventoryItem.stacksize <= 0)
        {
            ForagingSystem.ResourceExpired(gameObject);
            return false;
        }
        return true;
    }
    public void ResetValuesAfterTime()
    {
        timeLeftForReset = resetTime;
        StartCoroutine(StartResetting(resetTime));
    }
    private IEnumerator StartResetting(float delayInSeconds = 1)
    {
        while (timeLeftForReset > 0)
        {
            timeLeftForReset--;
            yield return new WaitForSeconds(1);
            SaveData();
        }
        gameObject.GetComponent<Renderer>().enabled = true;
        gameObject.GetComponent<Collider2D>().enabled = true;
        inventoryItem.stacksize = originalStackSize;

    }

    public bool LoseOneStack()
    {
        inventoryItem.RemoveFromStack();
        inventoryItem.data.IncreaseInteractedAmmount();
        SaveData();
        if (inventoryItem.stacksize <= 0)
        {
            ForagingSystem.ResourceExpired(gameObject);
            return false;
        }
        return true;
    }

    [ContextMenu("SaveThisData")]
    public void SaveData()
    {
        

        ResourceWrapper res = new ResourceWrapper();

        res.resetTime = resetTime;
        res.originalStackSize = originalStackSize;
        res.timeLeftForReset = timeLeftForReset;
        res.stacksize = inventoryItem.stacksize;
        string strData = JsonConvert.SerializeObject(res);
        Debug.Log(strData);
        PlayerPrefs.SetString(strKey, strData);

    }

    [ContextMenu("LoadThisData")]
    public void LoadData()
    {
        if (PlayerPrefs.HasKey(strKey))
        {
            string strData = PlayerPrefs.GetString(strKey);

            ResourceWrapper res = JsonConvert.DeserializeObject<ResourceWrapper>(strData);
            resetTime = res.resetTime;
            originalStackSize = res.originalStackSize;
            timeLeftForReset = res.timeLeftForReset;
            inventoryItem.stacksize = res.stacksize;
            Debug.LogWarning("loaded data");

        }

    }
    public class ResourceWrapper
    {


        public int originalStackSize;
        public float resetTime;
        public float timeLeftForReset;
        public int stacksize;

    }

}
