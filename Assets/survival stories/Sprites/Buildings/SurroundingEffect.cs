using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurroundingEffect : MonoBehaviour
{
    public List<GameObject> ResourcesInTrigger = new List<GameObject>();

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.TryGetComponent<ObjectInfo>(out ObjectInfo inf))
        {
            if (inf.objectType == ObjectType.Resource)
            {
                
                GameObject.Destroy(collision.gameObject);
            }

        }





    }



}
