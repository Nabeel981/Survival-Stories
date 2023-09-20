using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class CraftingItemUIHolder : MonoBehaviour
{
    public Image ItemIcon;
    public CraftableItemData item;

    public Button craft;
    public TextMeshProUGUI info;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI ownedAmount;
    public GameObject PricePrefab;
    public GameObject Content;
    public List<GameObject> priceGameObj = new List<GameObject>();


    private void OnEnable()
    {
        //   GetWoodQuantity();
        UpdatePriceAmmounts();
        InventorySystem.itemsEvent += UpdatePriceAmmounts;
    }
    private void Start()
    {


    }

    private void OnDisable()
    {
        InventorySystem.itemsEvent -= UpdatePriceAmmounts;
    }
    //private void Start()
    //{///i didnt do it here to save memory when game starts  connect button in editor
    //   // craft.onClick.AddListener(CraftItem);
    //    // ShowData();
    //}

    /// <summary>
    /// complete it tomorow
    /// </summary>
    [ContextMenu("show new data ")]
    public void ShowData()
    {
        ItemIcon.sprite = item.icon;
        info.text = item.info;
        itemName.text = item.name;



        foreach (SubPrice p in item.price)
        {
            GameObject gam = Instantiate(PricePrefab, Content.transform);

            if (gam.TryGetComponent<Price>(out Price pri))
            {
                pri.SubPrice = new SubPrice();
                pri.SubPrice = p;
            }
            else
            {
                Price pr = gam.AddComponent<Price>();
                pr.SubPrice = new SubPrice();
                pr.SubPrice = p;
            }

            priceGameObj.Add(gam);
            gam.GetComponentInChildren<Image>().sprite = p.objectData.icon;
            gam.GetComponentInChildren<TextMeshProUGUI>().text = (/*InventorySystem.GetItemQuantity((ObjectData)p.resourceData).ToString()*/ /*+*/ "0/" + p.quantity);
        }
        foreach (BuildingData p in item.buildingsAOE)
        {
            GameObject gam = Instantiate(PricePrefab, Content.transform);

            if (gam.TryGetComponent<BuildingAOEPrice>(out BuildingAOEPrice pri))
            {
                pri.buildingReq = new BuildingData();
                pri.buildingReq = p;
            }
            else
            {
                BuildingAOEPrice pr = gam.AddComponent<BuildingAOEPrice>();
                pr.buildingReq = new BuildingData();
                pr.buildingReq = p;
            }

            priceGameObj.Add(gam);
            gam.GetComponentInChildren<Image>().sprite = p.icon;
            gam.GetComponentInChildren<TextMeshProUGUI>().text = p.displayName;
        }

    }

    [ContextMenu("destroy old data ")]
    public void DestroyOldData()
    {
        for (int i = 0; i < 5; i++)
        {
            foreach (GameObject gam in priceGameObj)
            {

                DestroyImmediate(gam.gameObject);
            }
        }
        priceGameObj.Clear();
        //   int c = Content.transform.childCount;

    }

    public void CraftItem()
    {
        Debug.Log("called times 1");
        if (item.objectType == ObjectType.Tool)
        {
            CraftingSystem.CraftButtonClicked(item);
        }
        else if (item.objectType == ObjectType.Building)
        {
            ConstructionSystem.ConstructionButtonClicked(item);
        }
        UpdatePriceAmmounts();


    }
    public void GetItemQuanitityFromInventory()
    {


    }
    public void UpdatePriceAmmounts()
    {
        //update using switch

        foreach (GameObject t in priceGameObj)
        {
            if (t.TryGetComponent<Price>(out Price p))
            {

                t.transform.GetComponentInChildren<TextMeshProUGUI>().text = (PriceTypeCheck(p.SubPrice.objectData).ToString() + "/" + p.SubPrice.quantity);

                //   Debug.Log(PriceTypeCheck(p.SubPrice.objectData).ToString() + " :" + p.SubPrice.objectData.displayName);
            }
            else if (t.TryGetComponent<BuildingAOEPrice>(out BuildingAOEPrice B))
            {
                if (ConstructionSystem.IsPlayerInRangeBool(B.buildingReq))
                {
                    t.GetComponentInChildren<Image>().color = new Color(0, .5f, 0, 1);
                }
                else
                {
                    t.GetComponentInChildren<Image>().color = new Color(.5f, 0, 0, 1);
                }

            }


        }
        if (item.objectType == ObjectType.Tool)
        {
            ownedAmount.text = "You currently own :" + InventorySystem.GetItemQuantity(item);
        }
        else if (item.objectType == ObjectType.Building)
        {
            ownedAmount.text = "You currently own :" + ConstructionSystem.GetBuildingQuantity((BuildingData)item) + "/" + ((BuildingData)item).maxAllowed;
        }


    }
    [ContextMenu("get wood quantity")]
    public void GetWoodQuantity()
    {

        //Debug.Log("script raeeeeeeeeeeeeeeeeeeeeen");
        //  Debug.Log(PriceTypeCheck(priceGameObj[0].GetComponent<Price>().SubPrice.objectData) + "wood availableeeeeeeeeeeeeeeeeeeee");

    }


    public static int PriceTypeCheck(ObjectData obj)
    {
        obj.GetType();



        switch (obj.objectType)
        {
            case ObjectType.Resource:
                return InventorySystem.GetItemQuantity((ObjectData)obj) + InventorySystem.GetItemQuantityInContainers((ObjectData)obj);


            case ObjectType.Tool:
                return InventorySystem.GetItemQuantity((ObjectData)obj);

            case ObjectType.Building:
                return ConstructionSystem.IsPlayerInRangeInt((BuildingData)obj);


            case ObjectType.PlayerTrait:

                return (int)((Traits)obj).currentQuantity;


            default:
                return 0;





        }
        // Debug.Log("returns 0 here");



    }

}
