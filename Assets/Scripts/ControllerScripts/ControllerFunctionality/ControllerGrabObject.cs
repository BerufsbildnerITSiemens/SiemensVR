using System;
using UnityEngine;

/// <summary>
/// Thje Script is attached to a Controller
/// Script to grab Objects with a Rigidbody
/// </summary>
public class ControllerGrabObject : ControllerFunctionality
{
    JointBreakSensor jointBreakSensor; //TODO

    protected override void Awake()
    {
        info = new GrabObjectInformation();


        base.Awake();
    }

    private void TriggerEnter(Collider other, VRSensor sensor)
    {
        ControllerInformation info = controllerManager.GetControllerInfo(sensor.GetComponent<SteamVR_TrackedObject>());
        SetCollidingObject(other, info);
    }
    private void TriggerStay(Collider other, VRSensor sensor)
    {
        ControllerInformation info = controllerManager.GetControllerInfo(sensor.GetComponent<SteamVR_TrackedObject>());
        SetCollidingObject(other, info);
    }
    private void TriggerExit(Collider other, VRSensor sensor)
    {
        ControllerInformation info = controllerManager.GetControllerInfo(sensor.GetComponent<SteamVR_TrackedObject>());
        GrabObjectInformation grabObjInfo = (GrabObjectInformation)info.GetFunctionalityInfoByType(typeof(GrabObjectInformation));
        if (!grabObjInfo.collidingObject)
        {
            return;
        }

        grabObjInfo.collidingObject = null;
    }

    /// <summary>
    /// In here we check if the collision that jsut occured is relevant 
    /// </summary>
    private void SetCollidingObject(Collider col, ControllerInformation info)
    {
        GrabObjectInformation grabObjInfo = (GrabObjectInformation)info.GetFunctionalityInfoByType(typeof(GrabObjectInformation));
        if (grabObjInfo.collidingObject || !col.GetComponent<Rigidbody>() || col.GetComponent<SteamVR_TrackedObject>())
        {
            return;
        }
        grabObjInfo.collidingObject = col.gameObject;

        //Debug.Log("Collided with -> " +info.trackedObj + " - " + grabObjInfo.collidingObject.name);
    }

    private void GrabObject(ControllerInformation info)
    {
        GrabObjectInformation grabObjInfo = (GrabObjectInformation)info.GetFunctionalityInfoByType(typeof(GrabObjectInformation));
        grabObjInfo.objectInHand = grabObjInfo.collidingObject;
        grabObjInfo.collidingObject = null;
        //objectInHand.transform.position = Vector3.Lerp(transform.position,objectInHand.transform.position, 0.8f);
        if (!info.trackedObj.GetComponent<FixedJoint>())
        {
            var joint = AddFixedJoint(info);
            joint.connectedBody = grabObjInfo.objectInHand.GetComponent<Rigidbody>();
        }
    }

    private FixedJoint AddFixedJoint(ControllerInformation info)
    {
        FixedJoint fx = info.trackedObj.gameObject.AddComponent<FixedJoint>();
        fx.breakForce = 20000;
        fx.breakTorque = 20000;
        return fx;
    }

    private void ReleaseObject(ControllerInformation info)
    {
        GrabObjectInformation grabObjInfo = (GrabObjectInformation)info.GetFunctionalityInfoByType(typeof(GrabObjectInformation));


        FixedJoint[] joints = info.trackedObj.gameObject.GetComponents<FixedJoint>();
        if (joints != null && joints.Length > 0)
        {
            foreach (var item in joints)
            {
                item.connectedBody = null;
                Destroy(item);
            }

        }
        if (grabObjInfo.objectInHand == null)
            return;
        grabObjInfo.objectInHand.GetComponent<Rigidbody>().velocity = Quaternion.Euler(0, 180, 0) * controllerManager.GetController(info.trackedObj).velocity;
        grabObjInfo.objectInHand.GetComponent<Rigidbody>().angularVelocity = Quaternion.Euler(0, 180, 0) * controllerManager.GetController(info.trackedObj).angularVelocity;


        if (grabObjInfo.ObjectReleased != null)
        {
            grabObjInfo.ObjectReleased.Invoke(grabObjInfo.objectInHand);
        }

        grabObjInfo.objectInHand = null;
    }

    public void ForceGrab(GameObject go, ControllerInformation info)
    {
        GrabObjectInformation grabObjInfo = (GrabObjectInformation)info.GetFunctionalityInfoByType(typeof(GrabObjectInformation));
        ReleaseObject(info);
        grabObjInfo.collidingObject = go;
        GrabObject(info);
    }

    protected override void ActiveControllerUpdate(ControllerInformation controller) { }

    protected override void NonActiveControllerUpdate(ControllerInformation controller) { }

    protected override void AnyControllerUpdate(ControllerInformation controller)
    {
        GrabObjectInformation grabObjInfo = (GrabObjectInformation)controller.GetFunctionalityInfoByType(typeof(GrabObjectInformation));

        if (controllerManager.GetController(controller.trackedObj).GetHairTriggerDown())
        {
            if (grabObjInfo.collidingObject)
            {
                GrabObject(controller);
            }
        }

        if (controllerManager.GetController(controller.trackedObj).GetHairTriggerUp())
        {
            ReleaseObject(controller);
        }

        if (controllerManager.GetController(controller.trackedObj).GetPressDown(SteamVR_Controller.ButtonMask.Grip))
        {
            if (grabObjInfo.objectInHand != null)
            {
                if (grabObjInfo.objectInHand.GetComponent<Lighter>())
                {
                    grabObjInfo.objectInHand.GetComponent<Lighter>().StartFire();
                }
            }
        }

        if (controllerManager.GetController(controller.trackedObj).GetPressUp(SteamVR_Controller.ButtonMask.Grip))
        {
            if (grabObjInfo.objectInHand != null)
            {
                if (grabObjInfo.objectInHand.GetComponent<Lighter>())
                {
                    grabObjInfo.objectInHand.GetComponent<Lighter>().StopFire();
                }
            }
        }
    }

    protected override void OnControllerInitialized()
    {
        base.OnControllerInitialized();
        foreach (var item in controllerManager.controllerInfos)
        {
            item.sensor.triggerEnter += TriggerEnter;
            item.sensor.triggerLeave += TriggerExit;
            item.sensor.triggerStay += TriggerStay;
        }
    }

    private void OnJointBroke(JointBreakSensor sensor)
    {
        ReleaseObject(controllerManager.GetControllerInfo(sensor.GetComponent<SteamVR_TrackedObject>()));
    }

    protected override void AfterControllerIntialized(){}
}
