using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public bool isFinishedBuilding = false;
    public BuildingData thisBuildingData;
    public float resetTime;
    // public CharacterBehaviour MainCharacter;

    public float timeToDestroy;

    private void Start()
    {



    }

    public void RangeIncreased()
    {
        GetComponentInChildren<SurroundingEffect>().gameObject.transform.localScale = new Vector3(thisBuildingData.aoeRange, thisBuildingData.aoeRange, thisBuildingData.aoeRange);
    }
    public void StartBreakingProcess()
    {
        // StartCoroutine(reduceBreakTime());
    }
    public IEnumerator reduceBreakTime()
    {

        while (timeToDestroy > 0)
        {

            yield return new WaitForSeconds(1);

            timeToDestroy--;

        }
        SelfDestruct();
    }
    public void SelfDestruct()
    {


        Destroy(gameObject);

    }

    public void SpawnIconOnTop(ToolData tool)
    {


    }
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.GetComponent<CharacterBehaviour>())
    //    {
    //        ConstructionSystem.ApplyBuildingEffectOnPlayer(thisBuildingData);
    //    }

    //}



    //private void OnTriggerExit2D(Collider2D collision)
    //{

    //    if (collision.GetComponent<CharacterBehaviour>())
    //    {
    //        ConstructionSystem.RemoveBuildingEffectOnPlayer(thisBuildingData);
    //    }

    //}

}
