using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportScene : MonoBehaviour {

    public VRSensor Trigger;
    public GameObject Player;
    private int teleport = 0;

    // Use this for initialization
    private void Start()
    {
        //Trigger.triggerEnter += triggerEnter;
        Trigger.triggerLeave += triggerLeave;
        Trigger.triggerStay += triggerStay;
    }

    private void Update()
    {

    }

    private void triggerStay(Collider collider, VRSensor sensor)
    {
        if (collider.gameObject.transform.name == "Camera (eye)")
        {
            if (teleport == 50)
            {
                SceneManager.LoadScene("House", LoadSceneMode.Additive);
            }
            else
            {
                teleport++;
            }
        }
    }

    private void triggerLeave(Collider collider, VRSensor sensor)
    {
        if (collider.gameObject.transform.name == "Camera (eye)")
        {
            teleport = 0;
        }
    }
    
        
}

