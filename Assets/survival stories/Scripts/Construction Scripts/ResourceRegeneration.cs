using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceRegeneration : MonoBehaviour
{
    public float time;
    public Resource data;
    private void Start()
    {
        if (TryGetComponent<Resource>(out Resource res))
        {
            data = res;
            StartCoroutine(RestoreCharge());
        }
    }
    public IEnumerator RestoreCharge()
    {

        while (true)
        {
            yield return new WaitForSeconds(1);
            data.inventoryItem.stacksize++;
        }

    }
}
