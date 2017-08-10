using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveScreenSpawner : ControllerFunctionality
{
    public GameObject spawnerPrefab;
    public Vector3 spawnOffset;
    public VRSensor hipSensor;

    private void Start()
    {
        hipSensor.triggerEnter += TriggerEnter;
        hipSensor.triggerLeave += TriggerExit;
    }


    private void TriggerEnter(Collider other, VRSensor sensor)
    {
        var trackedObj = other.GetComponent<SteamVR_TrackedObject>();
        if (trackedObj)
        {
            ControllerInformation grabber = controllerManager.GetControllerInfo(trackedObj);
            if (ActiveController == null)
            {
                ActiveController = grabber;
            }
        }
    }
    private void TriggerExit(Collider other, VRSensor sensor)
    {
        var trackedObj = other.GetComponent<SteamVR_TrackedObject>();
        if (trackedObj)
        {
            ControllerInformation grabber = controllerManager.GetControllerInfo(trackedObj);
            if (ActiveController == grabber)
            {
                ActiveController = null;
            }
        }
    }

    protected override void ActiveControllerUpdate(ControllerInformation controller)
    {
        if (controllerManager.GetController(controller.trackedObj).GetPressDown(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger) && InteractWithHelpScreen.interactingHelpScreen == null)
        {
            controllerManager.GetComponent<ControllerGrabObject>().ForceGrab(Instantiate(spawnerPrefab, controller.trackedObj.transform.position + spawnOffset, Quaternion.identity), controller);
        }
    }

    protected override void NonActiveControllerUpdate(ControllerInformation controller)
    {
    }

    protected override void AnyControllerUpdate(ControllerInformation controller)
    {
    }

    protected override void AfterControllerIntialized(){}
}
