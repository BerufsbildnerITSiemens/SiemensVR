using System;
using UnityEngine;

public class LaserPointer : ControllerFunctionality
{
    public Transform cameraRigTransform;
    public Transform headTransform; // The camera rig's head
    public Vector3 teleportReticleOffset; // Offset from the floor for the reticle to avoid z-fighting
    public LayerMask teleportMask; // Mask to filter out areas where teleports are allowed

    public GameObject laserPrefab; // The laser prefab
    private GameObject laser; // A reference to the spawned laser
    private Transform laserTransform; // The transform component of the laser for ease of use

    public GameObject teleportReticlePrefab; // Stores a reference to the teleport reticle prefab.
    private GameObject reticle; // A reference to an instance of the reticle
    private Transform teleportReticleTransform; // Stores a reference to the teleport reticle transform for ease of use

    private Vector3 hitPoint; // Point where the raycast hits
    private bool shouldTeleport; // True if there's a valid teleport target

    public Action OnLaserShow;
    public Action OnTeleport;

    //new
    void Start()
    {
        laser = Instantiate(laserPrefab);
        laserTransform = laser.transform;
        reticle = Instantiate(teleportReticlePrefab);
        teleportReticleTransform = reticle.transform;
    }

    private void ShowLaser(RaycastHit hit, SteamVR_TrackedObject trackedObj)
    {
        if (OnLaserShow != null)
        {
            OnLaserShow();
        }
        laser.SetActive(true); //Show the laser
        laserTransform.position = Vector3.Lerp(trackedObj.transform.position, hitPoint, .5f); // Move laser to the middle between the controller and the position the raycast hit
        laserTransform.LookAt(hitPoint); // Rotate laser facing the hit point
        laserTransform.localScale = new Vector3(laserTransform.localScale.x, laserTransform.localScale.y,
            hit.distance); // Scale laser so it fits exactly between the controller & the hit point
    }

    private void Teleport()
    {
        if (OnTeleport != null)
        {
            OnTeleport();
        }
        shouldTeleport = false; // Teleport in progress, no need to do it again until the next touchpad release
        reticle.SetActive(false); // Hide reticle
        Vector3 difference = cameraRigTransform.position - headTransform.position; // Calculate the difference between the center of the virtual room & the player's head
        difference.y = 0; // Don't change the final position's y position, it should always be equal to that of the hit point

        cameraRigTransform.position = hitPoint + difference; // Change the camera rig position to where the the teleport reticle was. Also add the difference so the new virtual room position is relative to the player position, allowing the player's new position to be exactly where they pointed. (see illustration)
    }

    protected override void ActiveControllerUpdate(ControllerInformation controller)
    {
        laser.SetActive(false);
        reticle.SetActive(false);


        // Is the touchpad held down?
        if (controllerManager.GetController(controller.trackedObj).GetPress(SteamVR_Controller.ButtonMask.Touchpad))
        {
            RaycastHit hit;

            // Send out a raycast from the controller
            if (Physics.Raycast(controller.trackedObj.transform.position, controller.trackedObj.transform.forward, out hit, 10, LayerMask.NameToLayer("Everything"), QueryTriggerInteraction.Ignore))
            {
                if (teleportMask == (teleportMask | (1 << hit.collider.gameObject.layer)))
                {
                    hitPoint = hit.point;

                    ShowLaser(hit, controller.trackedObj);

                    //Show teleport reticle
                    reticle.SetActive(true);
                    teleportReticleTransform.position = hitPoint + teleportReticleOffset;

                    shouldTeleport = true;
                }
            }
        }

        // Touchpad released this frame & valid teleport position found
        if (controllerManager.GetController(controller.trackedObj).GetPressUp(SteamVR_Controller.ButtonMask.Touchpad) && shouldTeleport)
        {
            Teleport();
            ActiveController = null;
        }
    }

    protected override void NonActiveControllerUpdate(ControllerInformation controller)
    {

    }

    protected override void AnyControllerUpdate(ControllerInformation controller)
    {
        if (ActiveController == null)
        {
            if (controllerManager.GetController(controller.trackedObj).GetPress(SteamVR_Controller.ButtonMask.Touchpad))
            {
                ActiveController = controller;
            }
        }
    }

    protected override void AfterControllerIntialized(){}
}
