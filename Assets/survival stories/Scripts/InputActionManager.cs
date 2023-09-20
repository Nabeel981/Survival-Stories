using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class InputActionManager : MonoBehaviour
{

    public static InputAction press = new InputAction();
    public Transform controllerTransform;

    public Vector2 oldUiLocation;




    public LayerMask uiLayerMask;


    protected void OnEnable()
    {
        // gameObject.SetActive(false);
        //#if (!UNITY_ANDROID && !UNITY_IOS) || UNITY_EDITOR

        //  controllerTransform.gameObject.SetActive(false);




        //#elif UNITY_ANDROID || UNITY_IOS
        oldUiLocation = controllerTransform.position;
        press.AddBinding("<Mouse>/leftButton");
        press.AddBinding("<Touchscreen>/touch*/press");

        InputActionManager.press.started += OnPointerDown;
        InputActionManager.press.canceled += OnPointerUp;


        InputActionManager.press.Enable();





        //#endif
    }

    private void OnPointerDown(InputAction.CallbackContext ctx)
    {


        var screenPosition = Vector2.zero;
        if (ctx.control?.device is Pointer pointer)
            screenPosition = pointer.position.ReadValue();

        //if (!EventSystem.current.IsPointerOverGameObject() )
        //{
        //    controllerTransform.position = screenPosition;
        //}
        //else
        //{
        //    Debug.Log("pointer over UI");
        //}







        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = screenPosition;
        List<RaycastResult> results = new List<RaycastResult>();
        // Check if the pointer is over any UI element
        EventSystem.current.RaycastAll(eventData, results);

        bool isOverUI = false;

        // Iterate through the results and check if any of the UI elements belong to the specified layer
        foreach (RaycastResult result in results)
        {


            if (Includes(uiLayerMask, result.gameObject.layer))
            {

                isOverUI = true;
                break;
            }
        }

        if (isOverUI)
        {
            //  Debug.Log("is over ui");
            // Pointer is over a UI element on the specified layer
            // Handle UI interaction
        }
        else
        {
            // Debug.Log("is not over ui");
            //controllerTransform.gameObject.SetActive(true);
            controllerTransform.position = screenPosition;
            // Pointer is not over a UI element on the specified layer
            // Handle non-UI interaction
        }










    }
    private void OnPointerUp(InputAction.CallbackContext ctx)
    {
        ConstructionSystem.instance.isDragging = false;
       // controllerTransform.gameObject.SetActive(false);
        controllerTransform.position = oldUiLocation;
    }

    // Update is called once per frame
    void Update()
    {


    }

    public static bool Includes(LayerMask mask, int layer)
    {
        return (mask.value & 1 << layer) > 0;
    }

}
