using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AchievmentPopUpUiHandler : MonoBehaviour
{

    public Image icon;
    public TextMeshProUGUI achievmentName;
    public TextMeshProUGUI achievmentInfo;
    public Animator anim;


    public void SetData(Achievment data)
    {
        icon.sprite = data.Badge;
        achievmentName.text = data.achievmentName;
        achievmentInfo.text = data.info;
        anim.enabled = true;
    }
}
