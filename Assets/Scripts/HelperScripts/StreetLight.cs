using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreetLight : MonoBehaviour
{
    public float turnOnTime;
    public float turnOffTime;
    public float transitionSpeed;

    private VRTime timescript;
    private Light lightToControl;
    private float defaultIntensity;

    private bool isOn;

    // Use this for initialization
    void Start()
    {
        timescript = GameObject.FindObjectOfType<VRTime>();
        timescript.timeChanged += TimeChanged;
        lightToControl = GetComponent<Light>();
        defaultIntensity = lightToControl.intensity;
    }

    private void TimeChanged(float time)
    {
        float twentyFourTimeFormat = timescript.getTimeAstwentyFourFormat();
        if ((twentyFourTimeFormat > turnOnTime && twentyFourTimeFormat < turnOffTime) || (turnOffTime < turnOnTime && (twentyFourTimeFormat < turnOffTime && twentyFourTimeFormat < turnOnTime || twentyFourTimeFormat > turnOffTime && twentyFourTimeFormat > turnOnTime)))
        {
            if (!isOn)
            {
                StopAllCoroutines();
                StartCoroutine(FadeLightOn());
            }
        }
        else
        {
            if (isOn)
            {
                StopAllCoroutines();
                StartCoroutine(FadeLightOff());
            }
        }
    }
    private IEnumerator FadeLightOn()
    {
        while (lightToControl.intensity < defaultIntensity)
        {
            lightToControl.intensity += transitionSpeed * Time.deltaTime;
            yield return 0;
        }
        lightToControl.intensity = defaultIntensity;
        isOn = true;
    }
    private IEnumerator FadeLightOff()
    {
        while (lightToControl.intensity > 0)
        {
            lightToControl.intensity -= transitionSpeed * Time.deltaTime;
            yield return 0;
        }
        lightToControl.intensity = 0;
        isOn = false;
    }
}
