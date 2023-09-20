using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    public static UIHandler instance;
    public List<GameObject> allUI = new List<GameObject>();
    public GameObject buildingUi;

    private void OnEnable()
    {
        if (instance == null) instance = this;
    }
    public void DisableAllUI()
    {
        foreach (var g in allUI)
        {
            g.SetActive(false);
        }

    }
    public void EnableAllUI()
    {
        foreach (var g in allUI)
        {
            g.SetActive(true);
        }

    }

    public void BuildingPlacementModeStart()
    {
        DisableAllUI();
        buildingUi.SetActive(true);

    }
    public void BuildingPlacementModeEnd()
    {
        EnableAllUI();
        buildingUi.SetActive(false);

    }
}
