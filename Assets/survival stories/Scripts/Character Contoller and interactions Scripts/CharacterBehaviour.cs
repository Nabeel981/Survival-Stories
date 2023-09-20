using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterBehaviour : MonoBehaviour
{
    public ObjectInfo currentTargetInfo;
    public bool allowed;
    public NavMeshAgent navAgent;
    public Animator mainCharaterAnimator;
    public float offsetDistance;
    public SurroundingCheck surrounding;
    public bool InteractWithTarget;
    public bool ToolAvailable = false;
    public AICharacterBehaviour aiChar;

    private void Start()
    {
        surrounding = GetComponentInChildren<SurroundingCheck>();
        navAgent = GetComponent<NavMeshAgent>();
        mainCharaterAnimator = GetComponent<Animator>();
        InventorySystem.instance._currentDefaults.player = this.gameObject;
        aiChar = GetComponent<AICharacterBehaviour>();
    }
    private void Update()
    {
        if (aiChar.CurrentTarget != null && surrounding.allowCheck && aiChar.CurrentTarget.GetComponent<Collider2D>().enabled)
        {
            ResetAnimation();
            currentTargetInfo = aiChar.CurrentTarget.GetComponent<ObjectInfo>();
        }
        //int a = 0;
        //a-= 5 - 7;
        //Debug.Log(a);
        if (!currentTargetInfo)
        {
            Announcements.instance._slider.gameObject.SetActive(false);
            InventorySystem.instance.isIdle = true;

        }
        if (allowed & currentTargetInfo)
        {

            if (!navAgent.enabled)
            {

                navAgent.enabled = true;
            }

            //   Vector3 offset = (currentTargetInfo.transform.position - transform.position).normalized * offsetDistance;
            // Vector3 targetPosition = currentTargetInfo.transform.position - new Vector3(offset.x, 0f, 0);
            //  Vector3 targetPosition = currentTargetInfo.transform.position - offset;

            // Set the destination for the NavMesh Agent
            // navMeshAgent.SetDestination(targetPosition);

            ChageDirection(transform, currentTargetInfo.transform.position);
            navAgent.SetDestination(currentTargetInfo.transform.position);



            if (Vector2.Distance(currentTargetInfo.transform.position, transform.position) <= .6)
            {


                //  MainCharaterAnimator.SetBool("walking", false);
                ProcessAnimation();
                InteractWithTarget = true;
            }
            else if (Vector2.Distance(currentTargetInfo.transform.position, transform.position) <= .6)
            {
                navAgent.isStopped = true;

                //  MainCharaterAnimator.SetBool("walking", false);
                ProcessAnimation();
                InteractWithTarget = true;

            }
            else
            {
                Announcements.instance._slider.gameObject.SetActive(false);
                mainCharaterAnimator.SetBool("walking", true);
                InteractWithTarget = false;
                ChageDirection(transform, currentTargetInfo.transform.position);
                // MainCharaterAnimator.SetBool("walking", true);
            }
            // ChageDirection(targetPosition);
        }


        if (InteractWithTarget && currentTargetInfo)
        {
            if (currentTargetInfo.objectType == ObjectType.Resource)
            {
                ProcessResource();
            }
            else if (currentTargetInfo.objectType == ObjectType.Enemy)
            {
                // interact with enemy in a different way
                ProcessEnemy();
            }
            else if (currentTargetInfo.objectType == ObjectType.Building)
            {
                // interact with enemy in a different way
                ProcessBuilding();
            }
        }

    }

    private void ProcessBuilding()
    {
        if (ConstructionSystem.ProcessBuilding(currentTargetInfo.GetComponent<Building>()))
        {


            InventorySystem.instance.isIdle = false;



        }
        else
        {
            if (Announcements.instance) Announcements.DeactivateHoveringIcon();
            surrounding.ResourcesInTrigger.Remove(currentTargetInfo.gameObject);
            currentTargetInfo = null;
            allowed = false;
            mainCharaterAnimator.SetBool("walking", false);
            mainCharaterAnimator.SetBool("foraging", false);
            InventorySystem.instance.isIdle = true;
        }

    }

    //public bool Isitworthwhile()
    //{
    //    InventoryItem itemInventory = InventorySystem.GetToolForGathering(((ResourceData)currentTargetInfo.GetComponent<Resource>().inventoryItem.data).resourcesType);
    //    if (((ResourceData)currentTargetInfo.GetComponent<Resource>().inventoryItem.data).resourceBehaviour.HasFlag(ResourceBehaviour.Containable))
    //    {
    //       if(itemInventory == null)
    //        {
    //            return false;
    //        }
    //    }
    //    return true;
    //}
    //public void ProcessResource()
    //{
    //    /// reminder to fix this null issue and turn null into bare hands (REMINDER)
    //    if (ForagingSystem.ProcessResource(currentTargetInfo.gameObject) && Isitworthwhile())
    //    {
    //        if (ForagingSystem.CheckGatheringReruirments((ResourceData)currentTargetInfo.GetComponent<Resource>().inventoryItem.data))
    //        {


    //            ///give proper harvest speed here
    //            /////using a trait 
    //            /////this should be for all tools cuz all tools lose durability
    //            InventoryItem item = InventorySystem.GetToolForGathering(((ResourceData)currentTargetInfo.GetComponent<Resource>().inventoryItem.data).resourcesType);


    //            if (item == null)
    //            {

    //                ToolAvailable = false;

    //            }
    //            else
    //            {

    //                ToolAvailable = true;


    //            }

    //            if (ForagingSystem.GatherThisResource(currentTargetInfo.GetComponent<Resource>(), InventorySystem.gatheringSpeedNullCheck(item), item))
    //            {
    //                if (item != null)
    //                {
    //                    CraftingSystem.ToolUse(item, currentTargetInfo.GetComponent<Resource>());
    //                }



    //                //  Debug.Log("returned True here-------------------");

    //            }
    //            else
    //            {
    //                //   Debug.Log("returned false here-------------------");
    //                surrounding.ResourcesInTrigger.Remove(currentTargetInfo.gameObject);
    //                currentTargetInfo = null;
    //                allowed = false;
    //                mainCharaterAnimator.SetBool("walking", false);
    //                mainCharaterAnimator.SetBool("foraging", false);
    //            }
    //        }
    //    }
    //    else
    //    {
    //        mainCharaterAnimator.SetBool("walking", false);
    //        mainCharaterAnimator.SetBool("foraging", false);
    //    }


    //} 
    public void ProcessResource()
    {
        /// reminder to fix this null issue and turn null into bare hands (REMINDER)
        InventoryItem item = InventorySystem.GetToolForResource(((ResourceData)currentTargetInfo.GetComponent<Resource>().inventoryItem.data).resourcesType);
        if (item == null)
        {
            ToolAvailable = false;
        }
        else
        {
            ToolAvailable = true;
        }

        if (ForagingSystem.ProcessResource(currentTargetInfo.GetComponent<Resource>()))
        {


            InventorySystem.instance.isIdle = false;

            //  Debug.Log("returned True here-------------------");

        }
        else
        {
            //   Debug.Log("returned false here-------------------");
            surrounding.ResourcesInTrigger.Remove(currentTargetInfo.gameObject);
            currentTargetInfo = null;
            allowed = false;
            mainCharaterAnimator.SetBool("walking", false);
            mainCharaterAnimator.SetBool("foraging", false);
            InventorySystem.instance.isIdle = true;
        }





    }



    public void ResetAnimation()
    {
        mainCharaterAnimator.SetBool("walking", false);
        mainCharaterAnimator.SetBool("foraging", false);
    }
    public void ProcessAnimation()
    {
        mainCharaterAnimator.SetBool("walking", false);

        if (currentTargetInfo.objectType == ObjectType.Resource)
        {
            mainCharaterAnimator.SetBool("foraging", true);
            ResourceData res = (ResourceData)currentTargetInfo.GetComponent<Resource>().inventoryItem.data;
            switch (res.resourcesType)
            {
                case ResourcesType.Bush:

                    mainCharaterAnimator.SetFloat("foraging float", 0);

                    break;

                case ResourcesType.water:

                    mainCharaterAnimator.SetFloat("foraging float", 0);


                    break;
                case ResourcesType.stone:
                    if (ToolAvailable)
                    {
                        mainCharaterAnimator.SetFloat("foraging float", .5f);
                    }
                    else
                    {
                        mainCharaterAnimator.SetFloat("foraging float", 0);
                    }
                    break;
                case ResourcesType.wood:
                    if (ToolAvailable)
                    {
                        mainCharaterAnimator.SetFloat("foraging float", .75f);
                    }
                    else
                    {
                        mainCharaterAnimator.SetFloat("foraging float", 0);
                    }
                    break;

                    // case resou
            }




        }


        else if (currentTargetInfo.objectType == ObjectType.Building)
        {
            mainCharaterAnimator.SetBool("foraging", true);

            mainCharaterAnimator.SetFloat("foraging float", 0);
        }


    }
    public void ProcessEnemy()
    {
        /// reminder to fix this null issue and turn null into bare hands (REMINDER)

        mainCharaterAnimator.SetBool("foraging", true);
        InventoryItem data = InventorySystem.HasSword();
        if (data != null)
        {
            mainCharaterAnimator.SetFloat("foraging float", 1);
        }
        else
        {
            mainCharaterAnimator.SetFloat("foraging float", 0);

        }






    }

    public static void ChageDirection(Transform gam, Vector3 target)
    {

        if (target != null)
        {
            //  Vector3 targetDirection = target - transform.position;
            if (target.x > gam.position.x)
            {

                gam.eulerAngles = new Vector3(0f, 0f, 0f);
            }
            else
            {
                gam.eulerAngles = new Vector3(0f, 180f, 0f);
            }

            //float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;


            //if (angle > 90f || angle < -90f)
            //{

            //}
            //else
            //{

            //}
        }


    }

    public void ExecuteAction()
    {


    }
    public void AutoMovement(GameObject Target)
    {

    }


}
