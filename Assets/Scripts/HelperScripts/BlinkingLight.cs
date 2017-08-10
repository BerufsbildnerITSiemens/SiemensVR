using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkingLight : MonoBehaviour
{
    public float periodTime = 1f;
    private bool isOn;
    private Light controlledLight;
    // Use this for initialization
    void Start()
    {
        controlledLight = GetComponent<Light>();
        StartCoroutine(Blink());
    }

    // Update is called once per frame
    void Update()
    {

    }
    IEnumerator Blink()
    {
        while (true)
        {
            yield return new WaitForSeconds(periodTime);
            isOn = !isOn;
            controlledLight.enabled = isOn;
        }
    }
}
