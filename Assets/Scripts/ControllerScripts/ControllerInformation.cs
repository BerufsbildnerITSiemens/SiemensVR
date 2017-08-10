using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ControllerInformation
{
    public SteamVR_TrackedObject trackedObj { get; private set; }
    public VRSensor sensor { get; private set; }
    public List<ControllerFunctionalityInformation> functionalityInformations; //private


    public ControllerInformation(SteamVR_TrackedObject _trackedObj, VRSensor controllerSensor)
    {
        trackedObj = _trackedObj;
        sensor = controllerSensor;
        functionalityInformations = new List<ControllerFunctionalityInformation>();
    }

    public void AddFunctionalityInfo(ControllerFunctionalityInformation info)
    {
        functionalityInformations.Add(info);
    }
    public ControllerFunctionalityInformation GetFunctionalityInfoByType(Type type)
    {
        foreach (var item in functionalityInformations)
        {
            if (item.GetType() == type)
            {
                return item;
            }
        }
        return null;
    }

}
