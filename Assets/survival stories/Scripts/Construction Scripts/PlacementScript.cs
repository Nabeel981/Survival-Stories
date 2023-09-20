using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementScript : MonoBehaviour
{
    public float constructionTime;

    public BoxCollider2D myCollider;
    public SpriteRenderer mysprite;
    public Rigidbody2D myRB;
    public bool isPlaced = false;
    public bool allowPlacement;
    public List<GameObject> InTriggerObjects = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        isPlaced = false;
        myCollider = GetComponent<BoxCollider2D>();
        myCollider.isTrigger = true;
        myRB = GetComponent<Rigidbody2D>();
        myRB.isKinematic = true;
        myRB.constraints = RigidbodyConstraints2D.FreezeAll;
        mysprite = GetComponent<SpriteRenderer>();
    //    this.GetComponent<ObjectInfo>().enabled = false;
        this.GetComponent<Building>().enabled = false;


    }

    // Update is called once per frame
    void Update()
    {
        if (!isPlaced)
        {
            BeingPlaced();

        }
   




    }
    public void BuildingCompletion()
    {
       // mysprite.color = new Color(255, 255, 255, 255);
        this.GetComponent<PlacementScript>().enabled = false;

            
            this.GetComponent<Building>().enabled = true;
        

    }
    public void BeingPlaced()
    {
        if (InTriggerObjects.Count > 0)
        {
            Debug.Log("color should be less here");
            mysprite.color = new Color(255, 0, 0, 255);
            allowPlacement = false;
        }
        else if (InTriggerObjects.Count == 0)
        {
            Debug.Log("color should be normal here");
            allowPlacement = true;
            mysprite.color = new Color(255, 255, 255, 255);
            Debug.Log("color non trans");
        }






    }

    public void BuildingPlaced()
    {
        Debug.Log("biuilding finally placed");
        foreach (var item in InTriggerObjects)
        {
            item.SetActive(false);
        }
        this.gameObject.GetComponent<PlacementScript>().enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!InTriggerObjects.Contains(collision.gameObject))
        {
            InTriggerObjects.Add(collision.gameObject);
        }

    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (InTriggerObjects.Contains(collision.gameObject))
        {
            InTriggerObjects.Remove(collision.gameObject);
          
         
        }

    }
}
