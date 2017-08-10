using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerManager : MonoBehaviour
{
    public SteamVR_TrackedObject[] controllersForInitialization;

    public ControllerInformation[] controllerInfos;

    private void Awake()
    {
        controllerInfos = new ControllerInformation[controllersForInitialization.Length];
        for (int i = 0; i < controllersForInitialization.Length; i++)
        {
            VRSensor sensor = null;
            try
            {
                sensor = controllersForInitialization[i].GetComponent<VRSensor>();
            }
            catch (System.NullReferenceException)
            {
                Debug.LogError("The controller: " + controllersForInitialization[i] + " has no VRSensor attached.");
            }

            controllerInfos[i] = new ControllerInformation(controllersForInitialization[i], sensor);
        }
    }


    public SteamVR_Controller.Device GetController(int index)
    {
        int deviceIndex = (int)controllerInfos[index].trackedObj.index;
        if (deviceIndex >= 0)
        {
            return SteamVR_Controller.Input(deviceIndex);
        }
        else
        {
            return null;
        }
    }
    public SteamVR_Controller.Device GetController(SteamVR_TrackedObject trackedObj)
    {
        int deviceIndex = (int)trackedObj.index;
        if (deviceIndex >= 0)
        {
            return SteamVR_Controller.Input(deviceIndex);
        }
        else
        {
            return null;
        }
    }

    public bool ButtonHeldDownBothController(Valve.VR.EVRButtonId button)
    {
        if (controllerInfos.Length == 0)
        {
            return false;
        }
        for (int i = 0; i < controllerInfos.Length; i++)
        {
            if (GetController(i) == null)
                return false;

            if (!GetController(i).GetPress(button))
            {
                return false;
            }
        }
        return true;
    }

    public ControllerInformation GetControllerInfo(SteamVR_TrackedObject obj)
    {
        foreach (var item in controllerInfos)
        {
            if (item.trackedObj == obj)
            {
                return item;
            }
        }
        return null;
    }
    public ControllerInformation getOtherController(ControllerInformation controller)
    {
        foreach (var item in controllerInfos)
        {
            if (item != controller)
            {
                return item;
            }
        }
        return null;
    }
}
