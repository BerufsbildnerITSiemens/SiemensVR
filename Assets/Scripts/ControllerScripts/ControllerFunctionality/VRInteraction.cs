using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public abstract class VRInteraction : ControllerFunctionality
{
    [HideInInspector]
    public Valve.VR.EVRButtonId gripButton = Valve.VR.EVRButtonId.k_EButton_Grip;
    [HideInInspector]
    public Valve.VR.EVRButtonId menuButton = Valve.VR.EVRButtonId.k_EButton_ApplicationMenu;
    [HideInInspector]

    public Action<GameObject, ControllerInformation> onInteract;

    public GameObject laserPrefab; // The laser prefab
    private GameObject laser; // A reference to the spawned laser
    private Transform laserTransform; // The transform component of the laser for ease of use

    [HideInInspector]
    //public bool shouldInteract = true;
    public VRInteractionType interactingWith;


    protected virtual void Start()
    {
        laser = Instantiate(laserPrefab, transform);
        laserTransform = laser.transform;
        laser.SetActive(false);
    }

    protected override void ActiveControllerUpdate(ControllerInformation controller)
    {
        SteamVR_Controller.Device Controller = controllerManager.GetController(controller.trackedObj);
        if (interactingWith == null)
        {
            if (Controller.GetPress(menuButton))
            {
                RaycastHit hit;

                Ray ray = new Ray(controller.trackedObj.transform.position, controller.trackedObj.transform.forward);

                // Send out a raycast from the controller

                if (Physics.Raycast(ray, out hit, 100, LayerMask.NameToLayer("Everything"), QueryTriggerInteraction.Ignore) && !hit.collider.isTrigger)
                {
                    laser.SetActive(true); //Show the laser
                    laserTransform.position = Vector3.Lerp(controller.trackedObj.transform.position, hit.point, .5f); // Move laser to the middle between the controller and the position the raycast hit
                    laserTransform.LookAt(hit.point); // Rotate laser facing the hit point
                    laserTransform.localScale = new Vector3(laserTransform.localScale.x, laserTransform.localScale.y,
                        hit.distance * 1.333f); // Scale laser so it fits exactly between the controller & the hit point
                }
                else
                {
                    laser.SetActive(false);
                }
            }
            else if (Controller.GetPressUp(menuButton))
            {
                RaycastHit hit;

                // Send out a raycast from the controller
                if (Physics.Raycast(controller.trackedObj.transform.position, controller.trackedObj.transform.forward, out hit, 10000) && !hit.collider.isTrigger)
                {
                    if (onInteract != null)
                    {
                        onInteract(hit.collider.gameObject, controller);
                    }

                }
                else
                {
                    Controller.TriggerHapticPulse(3000);
                    ActiveController = null;
                }
                laser.SetActive(false);
            }
        }
    }
    protected override void NonActiveControllerUpdate(ControllerInformation controller) { }

    protected override void AnyControllerUpdate(ControllerInformation controller)
    {
        if (ActiveController == null)
        {
            SteamVR_Controller.Device Controller = controllerManager.GetController(controller.trackedObj);
            if (Controller.GetPressDown(menuButton))
            {
                ActiveController = controller;
            }
        }
    }
}
