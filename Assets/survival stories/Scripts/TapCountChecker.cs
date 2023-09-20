using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class TapCountChecker : MonoBehaviour, IPointerDownHandler
{
    public delegate void TapCountCheck( );
    public event TapCountCheck TapCountEvent;
    public float oldTime = 0;
    private float buttonClickThreshold = .3f;



    public void OnPointerDown(PointerEventData eventData)
    {
        float diff = Time.time - oldTime;
        Debug.Log("double clicked : " + Time.time);
        if (diff < buttonClickThreshold)
        {
            
            Debug.Log("double clicked 1 : " + diff);
           // int clicks = eventData.clickCount;
            TapCountEvent?.Invoke();

        }
        oldTime = Time.time;
    }
}
