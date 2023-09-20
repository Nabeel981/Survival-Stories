using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievmentSystem : MonoBehaviour
{
    public Achievment[] allAchievments;
    public static int TotalEnemiesKilled;
    public AchievmentPopUpUiHandler popHandler;
    public Transform badgeParent;
    public GameObject badgePrefab;

    private void Start()
    {
        ShowAllUnlockedAchievments();
    }



    //makes new images
    public void ShowAllUnlockedAchievments()
    {
        foreach (var item in allAchievments)
        {
            if (item.isCompleted && !BadgeLareadyAvailable(item.Badge))
            {
             
                MakeNewBadge(item.Badge);
            }
        }
    }

    public bool BadgeLareadyAvailable(Sprite sp)
    {
        for (int i = 0; i < badgeParent.childCount; i++)
        {
            if (badgeParent.GetChild(i).GetComponent<Image>().sprite == sp)
            {
                return true;
            }
        }
        return false;
    }

    [ContextMenu("asdadsad")]
    public void CheckAchievmentUnlock()
    {
        foreach (var item in allAchievments)
        {
            if (item.unlockAchievment() && !BadgeLareadyAvailable(item.Badge))
            {
                popHandler.SetData(item);
                MakeNewBadge(item.Badge);
            }
        }
    }

    public void MakeNewBadge(Sprite icon)
    {
        GameObject newBadge = Instantiate(badgePrefab, badgeParent);
        newBadge.GetComponent<Image>().sprite = icon;


    }
}
