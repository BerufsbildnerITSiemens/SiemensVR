using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Collider))]
public class VRSensor : MonoBehaviour
{
    public Action<Collider, VRSensor> triggerEnter;
    public Action<Collider, VRSensor> triggerStay;
    public Action<Collider, VRSensor> triggerLeave;
    public Action<Collision, VRSensor> collisionEnter;
    public Action<Collision, VRSensor> collisionLeave;

    private void OnTriggerEnter(Collider other)
    {
        if (triggerEnter != null)
        {
            triggerEnter(other, this);
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (triggerStay != null)
        {
            triggerStay(other, this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (triggerLeave != null)
        {
            triggerLeave(other, this);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collisionEnter != null)
        {
            collisionEnter(collision, this);
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collisionLeave != null)
        {
            collisionLeave(collision, this);
        }
    }
}
