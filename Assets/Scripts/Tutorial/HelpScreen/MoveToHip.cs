using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToHip : MonoBehaviour
{
    public float percentageFromRigToHead = 0.5f;
    private Transform rig;
    private Transform head;
    // Use this for initialization
    void Start()
    {
        rig = GameObject.FindObjectOfType<SteamVR_ControllerManager>().transform;
        head = GameObject.FindObjectOfType<SteamVR_Camera>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = getHipPos();
    }

    private Vector3 getHipPos()
    {
        Vector3 lerped = Vector3.Lerp(rig.position, head.position, percentageFromRigToHead);

        return new Vector3(head.position.x, lerped.y, head.position.z);
    }
}
