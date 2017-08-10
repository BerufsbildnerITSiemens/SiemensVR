using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarageDoorOpen : MonoBehaviour {
    public VRSensor sensor;
    private bool entered = false;
    private Animator _animator;

    // Use this for initialization
    void Start () {
        _animator = GetComponent<Animator>();
        sensor.triggerEnter += triggerEnter;
        sensor.triggerLeave += triggerLeave;
    }

    private void triggerEnter(Collider collider, VRSensor sensor)
    {
        if(collider.gameObject.transform.name == "Camera (eye)" && entered == false)
        {
            entered = true;
            _animator.SetBool("isopen", true);
        }
    }

    private void triggerLeave(Collider collider, VRSensor sensor)
    {
        entered = false;
        _animator.SetBool("isopen", false);
    }
}
