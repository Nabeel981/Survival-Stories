using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Announcements : MonoBehaviour
{
    public GameObject HideAbleObjects;
    public static Announcements instance;
    public GameObject withImageAnnouncement;
    public GameObject withoutImageAnnouncement;
    public Animator AnnouncementAnimator;
    public GameObject HoveringIcon;

    public Slider _slider;
    public Image fillColor;
    //  public TextMeshProUGUI text;
    private void OnEnable()
    {
        if (instance == null) instance = this;
    }


    private void Start()
    {
        ForagingSystem.resourceGather += StartFillingBar;
        HelperFunctions.TimePassed += StartFillingBar;
    }
    public void PlayAnimation(string text, AnnouncementType type, Sprite sp = null)
    {

        switch (type)
        {

            case AnnouncementType.withImage:

                withImageAnnouncement.GetComponentInChildren<TextMeshProUGUI>().text = text;
                withImageAnnouncement.GetComponentInChildren<Image>().sprite = sp;
                AnnouncementAnimator.Play("with", 0);
                //  AnnouncementAnimator.SetBool("withoutimage", false);
                //  AnnouncementAnimator.SetBool("withimage", false);
                //  AnnouncementAnimator.SetBool("withimage", true);

                break;

            case AnnouncementType.withoutImage:

                withoutImageAnnouncement.GetComponentInChildren<TextMeshProUGUI>().text = text;
                AnnouncementAnimator.Play("without", 0);
                // AnnouncementAnimator.SetBool("withimage", false);
                //AnnouncementAnimator.SetBool("withoutimage", false);
                // AnnouncementAnimator.SetBool("withoutimage", true);
                break;


        }

    }

    [ContextMenu("popopopopopopopo")]
    public void dodis()
    {
        AnnouncementAnimator.Play("with", 0);
        //  AnnouncementAnimator.SetBool("withoutimage", false);
        // AnnouncementAnimator.SetBool("withimage", true);
    }

    public void StartFillingBar(ObjectType objType, float value, Transform parent = null)
    {

        _slider.gameObject.SetActive(true);
        Vector3 positionwOFfset = new Vector3(parent.position.x, parent.position.y + .5f, 0f);
        Vector3 screenPos = Camera.main.WorldToScreenPoint(positionwOFfset);
        _slider.transform.position = screenPos;
        switch (objType)
        {
            case ObjectType.Resource:
                _slider.gameObject.SetActive(true);
                fillColor.color = new Color(0, 1, 0, 1);
                _slider.value = value;
                break;

            case ObjectType.Building:
                fillColor.color = new Color(0, 0, 1, 1);
                _slider.value = value;
                break;
        }


    }

    public void ShowHoveringIcon(ToolData tool, Transform parent, Building build)
    {
        Debug.Log("building hovering 2");
        HoveringIcon.SetActive(true);
        HoveringIcon.transform.GetChild(0).GetComponent<Image>().sprite = tool.icon;
        Vector3 positionOffset = new Vector3(parent.position.x, parent.position.y + .5f, 0f);
        Vector3 screenPos = Camera.main.WorldToScreenPoint(positionOffset);
        HoveringIcon.transform.position = screenPos;
        HoveringIcon.GetComponent<HoveringIconClicked>().currentObject = build.GetComponent<ObjectInfo>();
    }

    public static void DeactivateHoveringIcon()
    {
        Announcements.instance.HoveringIcon.SetActive(false);
    }
    public void HideIcons()
    {
        HideAbleObjects.SetActive(false);
    }
}
