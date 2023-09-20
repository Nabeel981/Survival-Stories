using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoveringIconClicked : MonoBehaviour
{
    public ObjectInfo currentObject;
    //  public ObjectInfo currenTool;
    public void OnCLick()
    {
        if (currentObject != null)
        {
            switch (currentObject.objectType)
            {
                case ObjectType.Building:
                    Destroy(currentObject.gameObject);
                    gameObject.SetActive(false);
                    break;

            }
        }

        

    }
}
