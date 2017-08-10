using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Night : MonoBehaviour {
    private float xRotation;
    private Light Sun;
    public float transition = 0.01f;
    public float sunRise = 340;
    public float sunSet = 270;

    private void Start()
    {
        Sun = this.GetComponent<Light>();
    }

    // Update is called once per frame
    void Update () {
        xRotation = transform.eulerAngles.x;
        if(xRotation > sunSet && xRotation < sunRise) {
            if(Sun.intensity > 0)
            {
                Sun.intensity -= transition;
            }
        }
        else
        {
            if(Sun.intensity < 1)
            {
                Sun.intensity += transition;
            }
        }
    }
}
