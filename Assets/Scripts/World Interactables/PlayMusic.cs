using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PlayMusic : MonoBehaviour {

    private AudioSource speaker;

    // Use this for initialization
    void Start () {
        speaker = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.transform.name == "Controller (left)" || collider.transform.name == "Controller (right)")
        {
            speaker.Play();
        }
    }
}
