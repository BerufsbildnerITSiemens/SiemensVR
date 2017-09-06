using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class TV : MonoBehaviour {

    public VRSensor Trigger;
    public VideoPlayer Screen;
    private bool entered = false;
    

    // Use this for initialization
    private void Start () {
        Trigger.triggerEnter += triggerEnter;
        Trigger.triggerLeave += triggerLeave;
    }

    private void triggerEnter(Collider collider, VRSensor sensor)
    {
        if (collider.gameObject.transform.name == "Camera (eye)" && entered == false)
        {
            entered = true;
            Screen.Play();
        }
    }

    private void triggerLeave(Collider collider, VRSensor sensor)
    {
        entered = false;
        Screen.Pause();
    }
}
