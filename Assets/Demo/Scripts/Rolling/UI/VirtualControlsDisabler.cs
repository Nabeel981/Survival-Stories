using UnityEngine;

using UnityEngine.InputSystem;
using UnityEngine.EventSystems;




public class VirtualControlsDisabler : MonoBehaviour
{//script not in use

    public bool isPointerOverUI = true;
    public Vector2 oldUiLocation;
  
    private void OnDisable()
    {


    }

    private void OnPointerDown(InputAction.CallbackContext ctx)
    {

        isPointerOverUI = EventSystem.current.IsPointerOverGameObject();
        var screenPosition = Vector2.zero;
        if (ctx.control?.device is Pointer pointer)
            screenPosition = pointer.position.ReadValue();
  
      
        transform.position = screenPosition;
    }
    private void OnPointerUp(InputAction.CallbackContext ctx)
    {
        transform.position = oldUiLocation;

    }


    protected void OnEnable()
    {
        // gameObject.SetActive(false);
        #if (!UNITY_ANDROID && !UNITY_IOS) || UNITY_EDITOR

         gameObject.SetActive(false);




#elif UNITY_ANDROID || UNITY_IOS
        oldUiLocation = transform.position;

        InputActionManager.press.started += OnPointerDown;
        InputActionManager.press.canceled += OnPointerUp;
        InputActionManager.press.Enable();





#endif
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
}


