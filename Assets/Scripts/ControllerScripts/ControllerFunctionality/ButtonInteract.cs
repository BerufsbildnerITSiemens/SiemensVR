using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The Script is used to interact with buttons.
/// </summary>
public class ButtonInteract : ControllerFunctionality
{
    protected override void Awake()
    {
        info = new ButtonInteractInformation();

        base.Awake();
    }


    private void TriggerEnter(Collider other, VRSensor sensor)
    {
        ControllerInformation controller = controllerManager.GetControllerInfo(sensor.GetComponent<SteamVR_TrackedObject>());
        ButtonInteractInformation grabObjInfo = (ButtonInteractInformation)controller.GetFunctionalityInfoByType(typeof(ButtonInteractInformation));

        VRButton button = other.GetComponent<VRButton>();
        //we check if it is a button and we are not already interacting with one
        if (button != null && grabObjInfo.buttonInRange == null)
        {


            grabObjInfo.buttonInRange = button;
            //And then we apply color to the button..
            button.OnControllerEnter();
        }
    }
    private void TriggerStay(Collider other, VRSensor sensor)
    {
        ControllerInformation controller = controllerManager.GetControllerInfo(sensor.GetComponent<SteamVR_TrackedObject>());
        ButtonInteractInformation grabObjInfo = (ButtonInteractInformation)controller.GetFunctionalityInfoByType(typeof(ButtonInteractInformation));

        if (grabObjInfo.buttonInRange == null)
        {
            VRButton button = other.GetComponent<VRButton>();
            if (button != null)
            {
                grabObjInfo.buttonInRange = button;
                button.OnControllerEnter();
            }
        }
    }
    private void TriggerExit(Collider other, VRSensor sensor)
    {
        ControllerInformation controller = controllerManager.GetControllerInfo(sensor.GetComponent<SteamVR_TrackedObject>());
        ButtonInteractInformation grabObjInfo = (ButtonInteractInformation)controller.GetFunctionalityInfoByType(typeof(ButtonInteractInformation));

        VRButton button = other.GetComponent<VRButton>();
        //we check if it was a button and if it was the one in range
        if (button != null && button == grabObjInfo.buttonInRange)
        {
            grabObjInfo.buttonInRange = null;
            //we get rid of the color again
            button.OnControllerLeave();
        }
    }
    protected override void ActiveControllerUpdate(ControllerInformation controller) { }

    protected override void NonActiveControllerUpdate(ControllerInformation controller) { }

    protected override void AnyControllerUpdate(ControllerInformation controller)
    {
        ButtonInteractInformation grabObjInfo = (ButtonInteractInformation)controller.GetFunctionalityInfoByType(typeof(ButtonInteractInformation));
        if (controllerManager.GetController(controller.trackedObj).GetHairTriggerDown())
        {
            if (grabObjInfo.buttonInRange != null)
            {
                grabObjInfo.buttonInRange.Interact();
            }

        }
    }


    protected override void OnControllerInitialized()
    {
        base.OnControllerInitialized();

        foreach (var item in controllerManager.controllerInfos)
        {
            item.sensor.triggerEnter += TriggerEnter;
            item.sensor.triggerStay += TriggerStay;
            item.sensor.triggerLeave += TriggerExit;
        }
    }

    protected override void AfterControllerIntialized(){}
}
