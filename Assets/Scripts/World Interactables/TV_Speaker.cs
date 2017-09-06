using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TV_Speaker : MonoBehaviour {
    public VRSensor Trigger;
    private AudioSource Speaker;
    private bool entered = false;

    // Use this for initialization
    private void Start()
    {
        Speaker = GetComponent<AudioSource>();
        Trigger.triggerEnter += triggerEnter;
        Trigger.triggerLeave += triggerLeave;
    }

    private void triggerEnter(Collider collider, VRSensor sensor)
    {
        if (collider.gameObject.transform.name == "Camera (eye)" && entered == false && Speaker != null)
        {
            entered = true;
            Speaker.Play();
        }
    }

    private void triggerLeave(Collider collider, VRSensor sensor)
    {
        if(Speaker != null)
        {
            entered = false;
            Speaker.Pause();
        }
    }
}
