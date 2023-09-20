using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HelperFunctions
{
    [SerializeField] private static float _duration = 0f;
    private static float _timer = 0f;


    public static float totalElapsedTime;

    public static GameObject oldObj = null;


    public delegate void TimePassedHelperFunction(ObjectType objType, float value, Transform parent);
    public static event TimePassedHelperFunction TimePassed;
    public static bool TimePasssed(float timeDelay)
    {
        if (_duration != timeDelay)
        {

            _duration = timeDelay;

            _timer = 0f;
        }

        _timer += Time.deltaTime;

        if (_timer >= _duration)
        {
            _duration = 0;


            _timer = 0f;



            return true;

        }

        return false;
    }





























    public static GameObject currentRes = null;
    public static float b;
    public static float a;
    public static float oldTotalStacks;
    public static float NormalizeTotalTimeNFillSlider(GameObject gam, float currentTime, int totalStacks, float timeForOneStack)
    {
        if (currentRes != gam)
        {
            currentRes = gam;
            oldTotalStacks = totalStacks;
            //   Debug.Log("current res " + currentRes + "new res " + gam);

        }

   //    a = oldTotalStacks * timeForOneStack;
        // Debug.Log(" oldTotal Stacks = " + oldTotalStacks + " timeForOne Stack = " + timeForOneStack);
        b = currentTime / timeForOneStack;
        // Debug.Log(" time needed  = " + a + " current time = " + currentTime);

        //  return b;

        return Remap(currentTime, 0, timeForOneStack, 0, 1);

    }

    
    public static float Remap(float value, float from1, float to1, float from2, float to2)
    {
        // this formula is just a ratio method using differences instead of just values
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    public static float DecreaseByPercent(float percentage, float originalNumber)
    {
        return originalNumber - (originalNumber * (percentage / 100));  
    } 
    public static float IncreaseByPercent(float percentage, float originalNumber)
    {
        return originalNumber + (originalNumber * (percentage / 100));  
    }
}
