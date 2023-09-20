using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using Newtonsoft.Json;
public class ConstructionSystem : MonoBehaviour, ISaveable
{
    public static ConstructionSystem instance;
    public bool isDragging = false;
    public static GameObject currentBuilding;
    public InputAction screenPos = new InputAction();

    public Camera cam;
    public ToolData shovel;
    public delegate void BuildingDestructionTimerEvent(Building building);
    public static event BuildingDestructionTimerEvent BuildingBreakTimer;


    public List<Building> allBuildingsHolder = new List<Building>();
    [Header("Leave It Empty")]
    List<BuildingsWrapper> listwrapper = new List<BuildingsWrapper>();
    private bool isOverbuilding
    {
        get
        {
            Debug.Log("gets called isOverbuilding");
            if (currentBuilding != null)
            {
                Ray ray = Camera.main.ScreenPointToRay(curScreenPos);
                //     RaycastHit hit;

                RaycastHit2D[] hit = Physics2D.RaycastAll(ray.origin, ray.direction);
                foreach (var item in hit)
                {


                    if (item.collider != null)
                    {
                        Debug.Log(item.transform.name + " was hit ");
                        if (item.transform == currentBuilding.transform)
                        {

                            Debug.Log("nononononoooooooooooono");
                            return item.transform == currentBuilding.transform;
                        }
                        //Debug.Log("ppppppppppppppppppppppppppppppppppppppo");
                    }
                }
                return false;
            }
            return false;
        }
    }
    public static Vector2 curScreenPos;

    public Vector2 WorldPos
    {
        get
        {

            return cam.ScreenToWorldPoint(new Vector3(curScreenPos.x, curScreenPos.y, cam.nearClipPlane));
        }

    }

    public void ReduceBuildingsTime()
    {

        foreach (var item in allBuildingsHolder)
        {
            item.GetComponent<Building>().timeToDestroy--;
            if (item.GetComponent<Building>().timeToDestroy <= 0)
            {
                allBuildingsHolder.Remove(item);
                Destroy(item.gameObject);
                break;
            }
        }
    }

    private void OnEnable()
    {
        if (instance == null) instance = this;
        BuildingBreakTimer += Addbuilding;
    }


    /// <summary>
    /// disable this script later to save some memeory
    /// </summary>
    private void Awake()
    {
        cam = Camera.main;
        screenPos.Enable();
        InputActionManager.press.Enable();
        screenPos.performed += context =>
        {

            curScreenPos = context.ReadValue<Vector2>();

        };
        InputActionManager.press.performed += _ =>
         {
             Debug.Log("pressing done here");
             if (isOverbuilding) StartCoroutine(Drag());
         };
        InputActionManager.press.canceled += _ =>
         {
             isDragging = false;
             Debug.Log("is dragging should be false here" + isDragging);

         };


        InvokeRepeating("ReduceBuildingsTime", 1, 1);

    }
    private void Start()
    {
        LoadData();
        InvokeRepeating("SaveData", 1, 5);
    }

    public void Addbuilding(Building build)
    {
        allBuildingsHolder.Add(build);
    }

    public static void ConstructionButtonClicked(CraftableItemData item)
    {
        if (CheckRequirementsForConstruct(item))
        {
            Debug.Log("item should craft");
            ConstructItem(item as BuildingData);
        }
        else
        {
            Debug.Log("item should not craft");
        }
    }
    public static void ConstructionButtonClicked()
    {




    }
    public static void ConstructItem(BuildingData item)
    {

        Debug.Log("item crafted");

        //  InventorySystem.AddItem(item);
        ///    InventorySystem.ChangeDefaults(item);
        ///    
        StartPlacing(item);

    }

    private static bool InventorySpaceCheck()
    {
        return InventorySystem.HasSpace();



    }

    public static void ApplyBuildingEffectOnPlayer(BuildingData data)
    {
        foreach (var building in InventorySystem.instance._currentDefaults.currentBuildingsInfo)
        {
            if (building.building == data)
            {
                building.effectInRange = true;
                return;
            }
        }

    }
    public static void RemoveBuildingEffectOnPlayer(BuildingData data)
    {
        foreach (var building in InventorySystem.instance._currentDefaults.currentBuildingsInfo)
        {
            if (building.building == data)
            {
                building.effectInRange = false;
                return;

            }
        }

    }


    public static void StartPlacing(BuildingData data)
    {
        Camera mainCamera = Camera.main;
        float halfHeight = mainCamera.orthographicSize;
        float halfWidth = halfHeight * mainCamera.aspect;
        Vector2 centerScreenPosition = mainCamera.transform.position;
        currentBuilding = GameObject.Instantiate(data.prefab);
        currentBuilding.GetComponent<SpriteRenderer>().sortingOrder = 4;
        currentBuilding.transform.position = centerScreenPosition;
        UIHandler.instance.BuildingPlacementModeStart();
        IsitViableToPlaceHere();

        //InputActionManager.press.started += PlacingBuilding;
        //InputActionManager.press.canceled += _ => { isDragging = false; };
        //disable all the hud

    }

    public static void PlacingBuilding(InputAction.CallbackContext ctx)
    {
        if (currentBuilding != null)
        {


            var screenPosition = Vector2.zero;
            if (ctx.control?.device is Pointer pointer)
                screenPosition = pointer.position.ReadValue();

            Vector2 mousePosition = Mouse.current.position.ReadValue();



        }
    }
    public IEnumerator Drag()
    {
        isDragging = true;

        while (isDragging)
        {
            Debug.Log("building should move here");
            currentBuilding.transform.position = WorldPos;


            yield return null;

        }

    }

    public static void IsitViableToPlaceHere()
    {


        currentBuilding.GetComponent<BoxCollider2D>().isTrigger = true;
        currentBuilding.AddComponent<Rigidbody2D>();
        currentBuilding.AddComponent<PlacementScript>();

    }
    public void ConfirmPlacingBuilding()
    {
        if (currentBuilding.GetComponent<PlacementScript>().allowPlacement)
        {
            Building bu = currentBuilding.GetComponent<Building>();

            IncreaseBuildingCount(bu);
            UIHandler.instance.BuildingPlacementModeEnd();
            bu.thisBuildingData.IncreaseInteractedAmmount();
            bu.timeToDestroy = bu.thisBuildingData.breakTime;
            currentBuilding.GetComponent<SpriteRenderer>().sortingOrder = 3;
            currentBuilding.GetComponent<PlacementScript>().InTriggerObjects.Clear();

            currentBuilding.AddComponent<ObjectInfo>().objectType = ObjectType.Building;

            currentBuilding.GetComponent<PlacementScript>().isPlaced = true;
            currentBuilding.GetComponent<Rigidbody2D>().isKinematic = false;

            currentBuilding.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, .5f);
            Debug.Log("color trans");

            //   currentBuilding.GetComponent<BoxCollider2D>().isTrigger = false;
            currentBuilding.layer = 0;

            foreach (SubPrice price in currentBuilding.GetComponent<Building>().thisBuildingData.price)
            {

                InventorySystem.instance.Remove((ObjectData)price.objectData, price.quantity);

            }
            currentBuilding = null;
            InventorySystem.instance.ann.PlayAnimation("Building placed awaiting construction!", AnnouncementType.withoutImage);

        }

    }





    public void CancelPlacingBuilding()
    {
        Destroy(currentBuilding.gameObject);
        currentBuilding = null;
        UIHandler.instance.BuildingPlacementModeEnd();


    }


    public static void RemoveResourcesInArea(List<Resource> res)
    {

        foreach (var _res in res)
        {
            ForagingSystem.RemoveResource(_res);
            // these resources need to respawn later again
        }
    }


    public static bool StartConstructionTimer(PlacementScript placement)
    {

        if (ConstructionSystem.ConstructTimePasssed(placement.GetComponent<Building>().thisBuildingData.constructionTime, placement.gameObject))
        {

            placement.BuildingCompletion();
            NavMeshSystem.instance.BakeEnvoirnment();
            Building bu = placement.GetComponent<Building>();
            bu.isFinishedBuilding = true;
            Debug.Log("counter 1");
            BuildingBreakTimer?.Invoke(bu);


            placement.GetComponent<BoxCollider2D>().isTrigger = false;
            //  Destroy(placement.GetComponent<BoxCollider2D>());
            GiveRadius(placement.gameObject);

            // placement.GetComponent<BoxCollider2D>().size = new Vector2(placement.GetComponent<BoxCollider2D>().size.x * .5f, placement.GetComponent<BoxCollider2D>().size.y * .5f);
            placement.gameObject.layer = 0;
            placement.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            InventorySystem.instance.ann.PlayAnimation(placement.GetComponent<Building>().thisBuildingData.name + " construction Completed!", AnnouncementType.withoutImage);

            foreach (Transform item in placement.transform)
            {
                item.gameObject.SetActive(true);
                // item.gameObject.layer = 0;
            }
            Destroy(placement);



            return false;
        }
        return true;
    }

    public static void GiveRadius(GameObject obj)
    {

        GameObject radius = new GameObject("radius");
        radius.layer = 18;

        radius.transform.SetParent(obj.transform);
        radius.AddComponent<BoxCollider2D>().size = obj.GetComponent<BoxCollider2D>().size;
        obj.GetComponent<BoxCollider2D>().isTrigger = true;
        radius.transform.position = obj.transform.position;
    }

    public static bool ProcessBuilding(Building build)
    {
        if (!PlayerAttributesSystem.HasEnoughStamina())
        {
            return false;
        }
        if (build.isFinishedBuilding)
        {
            // process resource here
            ConstructionSystem.HasEffectedInInventory(build);
            return false;
        }
        else
        {
            return StartConstructionTimer(build.GetComponent<PlacementScript>());
        }

    }


    public static bool ShouldPlayerGoToBuilding(Building build)
    {
        if (!build.isFinishedBuilding)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public static bool CheckRequirementsForConstruct(ObjectData item)
    {

        CraftableItemData craftableItem = (CraftableItemData)item;
        foreach (var _building in InventorySystem.instance._currentDefaults.currentBuildingsInfo)
        {
            if (item == _building.building && ((BuildingData)item).maxAllowed > _building.currentlyPlaced)
            {
                Debug.Log("price check failed 2");
                foreach (SubPrice pr in craftableItem.price)
                {
                    if (!CraftingSystem.PriceTypeCheck(pr.objectData, pr.quantity))
                    {
                        Debug.Log("price check failed");
                        return false;
                    }
                }
                return true;
            }


        }
        Debug.Log("price check failed 3");
        return false;



    }


    public static void IncreaseBuildingCount(Building build)
    {

        foreach (var item in InventorySystem.instance._currentDefaults.currentBuildingsInfo)
        {
            if (build.thisBuildingData == item.building)
            {
                item.currentlyPlaced++;
            }
        }
    }

    public static int GetBuildingQuantity(BuildingData build)
    {





        foreach (var item in InventorySystem.instance._currentDefaults.currentBuildingsInfo)
        {
            if (build == item.building)
            {
                return item.currentlyPlaced;
            }
        }
        return 0;

    }

    public static bool IsPlayerInRangeBool(BuildingData build)
    {
        Debug.Log("constructoin bool");
        foreach (var item in InventorySystem.instance._currentDefaults.currentBuildingsInfo)
        {
            if (build == item.building && item.effectInRange)
            {
                Debug.Log("constructoin bool 1");
                return true;
            }
        }
        Debug.Log("constructoin bool 2");
        return false;
    }


    public static int IsPlayerInRangeInt(BuildingData build)
    {
        foreach (var item in InventorySystem.instance._currentDefaults.currentBuildingsInfo)
        {
            if (build == item.building && item.effectInRange)
            {
                return 1;
            }
        }
        return 0;
    }



    [SerializeField] private static float _duration = 0f;
    private static float _timer = 0f;


    public static float totalElapsedTime;

    public static GameObject oldObj = null;


    public delegate void TimePassedHelperFunction(ObjectType objType, float value, Transform parent);
    public static event TimePassedHelperFunction TimePassed;
    public static bool ConstructTimePasssed(float contructionTime, GameObject gam)
    {
        if (oldObj != gam)
        {
            oldObj = gam;
            _duration = contructionTime;
            totalElapsedTime = 0;
            _timer = 0f;
        }






        _timer += Time.deltaTime;
        totalElapsedTime += Time.deltaTime;



        float a = HelperFunctions.Remap(totalElapsedTime, 0, contructionTime, 0, 1);
        Debug.Log("total time " + totalElapsedTime);
        TimePassed?.Invoke(ObjectType.Building, a, gam.transform);



        if (totalElapsedTime >= contructionTime)
        {
            totalElapsedTime = 0;

            _timer = 0f;



            return true;

        }

        return false;
    }

    public void BuildingsDeathCountDown()
    {


    }
    public static void HasEffectedInInventory(Building build)
    {
        if (InventorySystem.HasItemWithAmmount(ConstructionSystem.instance.shovel, 1))
        {
            Debug.Log("building hovering");
            Announcements.instance.ShowHoveringIcon(ConstructionSystem.instance.shovel, build.transform, build);
        }
    }

    [ContextMenu("Force Save")]
    public void SaveData()
    {
        if (allBuildingsHolder.Count > 0)
        {



            foreach (var item in allBuildingsHolder)
            {
                BuildingsWrapper bu = new BuildingsWrapper();
                bu.buildingID = item.thisBuildingData.StringID;
                bu.TimeLeft = item.timeToDestroy;

                bu.PosX = item.gameObject.transform.position.x;
                bu.PosY = item.gameObject.transform.position.y;

                listwrapper.Add(bu);

            }
            Debug.Log("reached here without error");
            string str = JsonConvert.SerializeObject(listwrapper);
            Debug.Log("reached here without error 2");
            PlayerPrefs.SetString("constructionsystem", str);
            listwrapper.Clear();
        }

    }
    [ContextMenu("Force Load")]
    public void LoadData()
    {
        if (PlayerPrefs.HasKey("constructionsystem"))
        {
            string str = PlayerPrefs.GetString("constructionsystem");
            List<BuildingsWrapper> listwrapper = JsonConvert.DeserializeObject<List<BuildingsWrapper>>(str);

            ReConstructBuildings(listwrapper);
        }

    }


    public void ReConstructBuildings(List<BuildingsWrapper> listwrapper)
    {

        foreach (var item in listwrapper)
        {
            foreach (var item1 in InventorySystem.instance.allItemsLibrary.allBuildings)
            {
                if (item.buildingID == item1.StringID)
                {
                    if (item.TimeLeft > 10)
                    {


                        GameObject gam = Instantiate(item1.prefab);
                        gam.transform.position = new Vector3(item.PosX, item.PosY);
                        gam.GetComponent<Building>().isFinishedBuilding = true;
                        gam.GetComponent<Building>().timeToDestroy = item.TimeLeft;
                        ReconstructionHelper(gam.GetComponent<Building>());
                    }

                }
            }
        }

    }

    public void ReconstructionHelper(Building bu)
    {
        NavMeshSystem.instance.BakeEnvoirnment();


        Debug.Log("counter 1");
        BuildingBreakTimer?.Invoke(bu);


        bu.GetComponent<BoxCollider2D>().isTrigger = false;
        //  Destroy(placement.GetComponent<BoxCollider2D>());
        GiveRadius(bu.gameObject);

        // placement.GetComponent<BoxCollider2D>().size = new Vector2(placement.GetComponent<BoxCollider2D>().size.x * .5f, placement.GetComponent<BoxCollider2D>().size.y * .5f);
        bu.gameObject.layer = 0;
        bu.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        //InventorySystem.instance.ann.PlayAnimation(bu.GetComponent<Building>().thisBuildingData.name + " construction Completed!", AnnouncementType.withoutImage);

        foreach (Transform item in bu.transform)
        {
            item.gameObject.SetActive(true);
            // item.gameObject.layer = 0;
        }

    }
}


public class BuildingsWrapper
{
    public String buildingID;
    public float TimeLeft;
    public float PosX;
    public float PosY;
    public float PosZ;



}

