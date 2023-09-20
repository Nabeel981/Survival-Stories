using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using System;

public class DayNightCycle : MonoBehaviour
{
    [SerializeField] private Light2D sunLight; // Reference to the directional light representing the sun
    [SerializeField] private Light2D characterLight; // Reference to the directional light representing the player
    [SerializeField] private float minIntensity = .55f;
    [SerializeField] private float maxIntensity = 1.2f;
    double middayTime; // 12 hours * 60 minutes * 60 seconds
    double midnightTime;

    float intensity = 0;

    private void Start()
    {
        middayTime = 12 * 60 * 60; // 12 hours * 60 minutes * 60 seconds
        midnightTime = 0;
        InvokeRepeating("ChangeLightWithTime", 0, 3600);

    }
    [ContextMenu("changelightinstant")]
    public void ChangeInstantly()
    {
        ChangeLightWithTime();
    }
    private void ChangeLightWithTime()
    {
        // Get the current system time
        DateTime currentTime = DateTime.Now;

        // Calculate the total seconds elapsed since midnight
        double totalSeconds = currentTime.TimeOfDay.TotalSeconds;

        // Calculate the time at midday (12 PM) and midnight (12 AM) in total seconds
        double middayTime = 12 * 60 * 60; // 12 hours * 60 minutes * 60 seconds
        double midnightTime = 0;

        // Determine whether it's the first half of the day or the second half
        bool isFirstHalf = totalSeconds <= middayTime;

        // Calculate the normalized value based on the appropriate time range
        float t = isFirstHalf
            ? Mathf.InverseLerp((float)middayTime, (float)midnightTime, (float)totalSeconds)
            : Mathf.InverseLerp((float)middayTime, (float)midnightTime + 24 * 60 * 60, (float)totalSeconds);

        // Interpolate between minIntensity and maxIntensity based on the normalized time

        if (isFirstHalf)
        {
            Debug.Log("midnight to midday");
            intensity = Mathf.Lerp(maxIntensity, minIntensity, t);
        }
        else
        {
            Debug.Log("midday to midnight");
            intensity = Mathf.Lerp(maxIntensity, minIntensity, t);
        }


        // Set the sun's light intensity
        sunLight.intensity = intensity;
        if (intensity < .8f)
        {
            characterLight.enabled = true;
        }
        else
        {
            characterLight.enabled = false;
        }
    }



}
