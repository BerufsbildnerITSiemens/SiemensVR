using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportationPoint : MonoBehaviour {
    public VRSensor Trigger;
    public Transform Target;
    public GameObject Player;
    private int teleport = 0;

	// Use this for initialization
	private void Start () {
        Trigger.triggerLeave += triggerLeave;
        Trigger.triggerStay += triggerStay;
	}

    private void triggerStay(Collider collider, VRSensor sensor)
    {
        if (collider.gameObject.transform.name == "Camera (eye)")
        {
            if (teleport == 50)
            {
                Player.transform.position = Target.position;
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
