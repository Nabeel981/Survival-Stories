using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurroundingCheckAction : MonoBehaviour
{
   
    public ObjectInfo myObjectInfo;

  //  public List<GameObject> ResourcesInTrigger = new List<GameObject>();
    public List<GameObject> EnemiesInTrigger = new List<GameObject>();
  //  public List<GameObject> BuildingsInTrigger = new List<GameObject>();
    //bug if you spam up and down really fast you lose the detected item status :not fixed
    private void Start()
    {

        myObjectInfo = GetComponentInParent<ObjectInfo>();
        if (myObjectInfo.playerType == PlayerType.AiEnemy)
        {
            
        }
      


    }

 
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent<ObjectInfo>(out ObjectInfo objectInfo))
        {

            switch (objectInfo.playerType)
            {
            //    case ObjectType.Resource:
            //        if (!ResourcesInTrigger.Contains(collision.gameObject))
            //        {
            //            ResourcesInTrigger.Add(collision.gameObject);
            //        }
            //        break;
                case PlayerType.User:
                    if (!EnemiesInTrigger.Contains(collision.gameObject))
                    {
                        EnemiesInTrigger.Add(collision.gameObject);
                    }
                    break;
                //case ObjectType.Building:
                //    if (!BuildingsInTrigger.Contains(collision.gameObject))
                //    {
                //        ConstructionSystem.ApplyBuildingEffectOnPlayer(collision.GetComponent<Building>().thisBuildingData);
                //        BuildingsInTrigger.Add(collision.gameObject);
                //    }
                //    break;
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

            switch (objectInfo.playerType)
            {
               // case ObjectType.Resource:
                   // ResourcesInTrigger.Remove(collision.gameObject);

                   // break;
             //   case ObjectType.Building:
                  //  ConstructionSystem.RemoveBuildingEffectOnPlayer(collision.GetComponent<Building>().thisBuildingData);
                  //  BuildingsInTrigger.Remove(collision.gameObject);

                 //   break;
                case PlayerType.User:
                   
                    EnemiesInTrigger.Remove(collision.gameObject);
                    break;
            }




        }

    }



}
