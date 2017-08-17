using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Collider))]
public class Recognize_cd : MonoBehaviour
{
    //public VRButton button;
    //public VRSensor sensor;

    private AudioSource speaker;
    private AudioClip audio;
    private CD cd;

    // Use this for initialization
    void Start()
    {
        speaker = GetComponent<AudioSource>();
    }

    private void buttonPressed(int value)
    {
        
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.GetComponent<CD>())
        {
            speaker.clip = collider.gameObject.GetComponent<CD>().song;
        }
    }

    

}
