using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SurroundingCheck : MonoBehaviour
{
    [SerializeField] public bool allowCheck = false;
    public ObjectInfo myObjectInfo;
    private CharacterBehaviour myCharacterBehaviour;
    private Animator UserAnimator;
    public List<GameObject> ResourcesInTrigger = new List<GameObject>();
    public List<GameObject> EnemiesInTrigger = new List<GameObject>();
    public List<GameObject> BuildingsInTrigger = new List<GameObject>();
    //bug if you spam up and down really fast you lose the detected item status :not fixed
    private void Start()
    {

        myObjectInfo = GetComponentInParent<ObjectInfo>();
        if (myObjectInfo.playerType == PlayerType.User)
        {
            UserAnimator = GetComponent<Animator>();
        }
        myCharacterBehaviour = GetComponentInParent<CharacterBehaviour>();


    }

    private void FixedUpdate()
    {
        if (!allowCheck)
        {
            Announcements.instance.HideAbleObjects.SetActive(false);
        }


        if (allowCheck)
        {
            Announcements.instance.HideAbleObjects.SetActive(true);
            //  UserAnimator.enabled = true;
            UserAnimator.SetBool("uiEffect", true);
            // Debug.Log("action decide reached");
            DecideAction();


        }
        else
        {
            UserAnimator.SetBool("uiEffect", false);
            /// UserAnimator.enabled = false;
            myCharacterBehaviour.allowed = false;
        }
    }
    public void CheckSurroundings()
    {


    }

    public void DecideAction()
    {
        float closestDistance = Mathf.Infinity;
        GameObject closestObj = null;
        if (myObjectInfo.playerType == PlayerType.User)
        {
            //enemeis first

            for (int i = 0; i < EnemiesInTrigger.Count; i++)
            {
                // Iterate through the objects in the enemy trigger list
                //  Debug.Log("it worked");
                // Calculate the distance between your character and the object
                if (EnemiesInTrigger[i] != null)
                {
                    float distance = Vector3.Distance(transform.position, EnemiesInTrigger[i].transform.position);

                    // Check if it's the closest object so far
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestObj = EnemiesInTrigger[i];
                    }
                    // send to get processed as an enemy
                }
                else
                {
                    EnemiesInTrigger.Remove(EnemiesInTrigger[i]);
                    myCharacterBehaviour.ResetAnimation();
                    return;
                }

            }
            if (closestObj)
            {
                // Debug.Log("came here");
                FinaliseTarget(closestObj);



                return;
            }
            if (BuildingsInTrigger.Count <= 0)
            {
                if (Announcements.instance) Announcements.DeactivateHoveringIcon();
            }

            for (int i = 0; i < BuildingsInTrigger.Count; i++)
            {


                // Iterate through the objects in the enemy trigger list

                // Calculate the distance between your character and the object
                if (BuildingsInTrigger[i] != null)
                {

                    if (BuildingsInTrigger[i].GetComponent<PlacementScript>())
                    {



                        float distance = Vector3.Distance(transform.position, BuildingsInTrigger[i].transform.position);

                        // Check if it's the closest object so far
                        if (distance < closestDistance)
                        {
                            closestDistance = distance;
                            closestObj = BuildingsInTrigger[i];
                        }
                    }

                }
                else
                {
                    BuildingsInTrigger.Remove(BuildingsInTrigger[i]);
                    myCharacterBehaviour.ResetAnimation();
                    return;
                }


            }
            if (closestObj)
            {
                ConstructionSystem.HasEffectedInInventory(closestObj.GetComponent<Building>());
                if (ConstructionSystem.ShouldPlayerGoToBuilding(closestObj.GetComponent<Building>()))
                {
                    FinaliseTarget(closestObj);
                }
                // Debug.Log("came here");

                



            }

            for (int i = 0; i < ResourcesInTrigger.Count; i++)
            {
                //    Debug.Log("it worked");
                // Iterate through the objects in the resource trigger list

                // Calculate the distance between your character and the object
                if (ResourcesInTrigger[i] != null)
                {
                    float distance = Vector3.Distance(transform.position, ResourcesInTrigger[i].transform.position);

                    // Check if it's the closest object so far
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestObj = ResourcesInTrigger[i];
                    }
                }
                else
                {
                    ResourcesInTrigger.Remove(ResourcesInTrigger[i]);
                    myCharacterBehaviour.ResetAnimation();
                    return;
                }


                // send to get processed as a resources

            }
            if (closestObj)
            {
                //  Debug.Log("came here");
                FinaliseTarget(closestObj);

                return;
            }
            else
            {
                //  Debug.Log("came hasasasere");
                //you have no enemies no resources *THORFIN you have no enemies
                //nothing to do Reset Cam and Stuff
            }


            // above me is a different approach
            //switch (objectInfo.objectType)
            //{
            //    //everything here is prio wise //do not touch unless changing priority of targets
            //    //send it to the character behaviour for further processing
            //    case ObjectType.Enemy:

            //        break;

            //    case ObjectType.Resource:
            //        //finalise Target Resource  
            //        break;


            //}
        }
        //else if (myObjectInfo.playerType == PlayerType.AiEnemy)
        //{
        //    if (objectInfo.playerType == PlayerType.User)
        //    {
        //        // do this later
        //    }
        //}

    }


    public void FinaliseTarget(GameObject targetObject)
    {
        if (RecheckAnyDisabledObjects(targetObject))
        {


            myCharacterBehaviour.currentTargetInfo = targetObject.GetComponent<ObjectInfo>();
            myCharacterBehaviour.allowed = true;
            if (transform.parent)
            {
                Vector2 positionInWorld = transform.parent.position;
                //  transform.parent = null;
                transform.SetParent(null);
                transform.position = positionInWorld;

            }
        }
        else
        {
            myCharacterBehaviour.ResetAnimation();
            myCharacterBehaviour.currentTargetInfo = null;
            myCharacterBehaviour.allowed = false;
        }



    }

    public bool RecheckAnyDisabledObjects(GameObject gam)
    {
        if (gam.GetComponent<Collider2D>().enabled)
        {
            return true;
        }
        else
        {
            
            RemoveFromLists(gam);
            return false;
        }
    }


    public void RemoveFromLists(GameObject gam)
    {
        if (gam.TryGetComponent<ObjectInfo>(out ObjectInfo objectInfo))
        {

            switch (objectInfo.objectType)
            {
                case ObjectType.Resource:
                    ResourcesInTrigger.Remove(gam);

                    break;
                case ObjectType.Building:
                    ConstructionSystem.RemoveBuildingEffectOnPlayer(gam.GetComponent<Building>().thisBuildingData);
                    BuildingsInTrigger.Remove(gam);

                    break;
                case ObjectType.Enemy:
                    if (objectInfo = myCharacterBehaviour.currentTargetInfo)
                    {
                        myCharacterBehaviour.allowed = false;
                        myCharacterBehaviour.currentTargetInfo = null;
                    }
                    EnemiesInTrigger.Remove(gam);
                    break;
            }




        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Debug.Log("trigger entered");

    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent<ObjectInfo>(out ObjectInfo objectInfo))
        {

            switch (objectInfo.objectType)
            {
                case ObjectType.Resource:
                    if (!ResourcesInTrigger.Contains(collision.gameObject))
                    {
                        ResourcesInTrigger.Add(collision.gameObject);
                    }
                    break;
                case ObjectType.Enemy:
                    if (!EnemiesInTrigger.Contains(collision.gameObject))
                    {
                        EnemiesInTrigger.Add(collision.gameObject);
                    }
                    break;
                case ObjectType.Building:
                    if (!BuildingsInTrigger.Contains(collision.gameObject))
                    {
                        ConstructionSystem.ApplyBuildingEffectOnPlayer(collision.GetComponent<Building>().thisBuildingData);
                        BuildingsInTrigger.Add(collision.gameObject);
                    }
                    break;
            }




        }

        //dont call decide action here cuz it will run muliples times 
        //fixed update or late update is a better option


    }


    private void OnTriggerExit2D(Collider2D collision)
    {

        // Debug.Log("trigger left");
        if (collision.TryGetComponent<ObjectInfo>(out ObjectInfo objectInfo))
        {

            switch (objectInfo.objectType)
            {
                case ObjectType.Resource:
                    ResourcesInTrigger.Remove(collision.gameObject);

                    break;
                case ObjectType.Building:
                    ConstructionSystem.RemoveBuildingEffectOnPlayer(collision.GetComponent<Building>().thisBuildingData);
                    BuildingsInTrigger.Remove(collision.gameObject);

                    break;
                case ObjectType.Enemy:
                    if (objectInfo = myCharacterBehaviour.currentTargetInfo)
                    {
                        myCharacterBehaviour.allowed = false;
                        myCharacterBehaviour.currentTargetInfo = null;
                    }
                    EnemiesInTrigger.Remove(collision.gameObject);
                    break;
            }




        }

    }





}
