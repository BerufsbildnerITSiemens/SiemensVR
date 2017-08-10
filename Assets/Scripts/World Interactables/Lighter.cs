using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lighter : MonoBehaviour {
    public ParticleSystem fire;
    public GameObject light;

	// Use this for initialization
	void Start () {
        fire.Stop();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartFire()
    {
        fire.Play();
        fire.GetComponent<Fire>().lit = true;
        light.SetActive(true);
    }

    public void StopFire()
    {
        fire.Stop();
        fire.GetComponent<Fire>().lit = false;
        light.SetActive(false);
    }
}
