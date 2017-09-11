using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartRoom : MonoBehaviour {

    public VRSensor trigger;
    public Transform target;
    private bool entered = false;

	// Use this for initialization
	void Start () {
        trigger.triggerEnter += triggerEnter;
	}

    private void triggerEnter(Collider collider, VRSensor sensor)
    {
        if (collider.gameObject.transform.name == "Camera (eye)" && entered == false)
        {
            transform.position = target.position;
        }
    }
}
