using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(VRWatchInteraction))]
public class VRLightInteraction : VRInteractionType
{
    public GameObject[] accordingTimePrefab;
    private VRLight interactingLight;


    private bool interactedThisFrame = false;

    protected override void Awake()
    {
        base.info = new LightInteractionInformation();
        base.Awake();
    }


    protected override void Start()
    {
        base.Start();


        vrInteraction = GetComponent<VRWatchInteraction>();
        ((VRWatchInteraction)vrInteraction).onInteract += OnInteract;
        ((VRWatchInteraction)vrInteraction).updateDisplay += UpdateDisplay;
    }

    protected void UpdateDisplay(ControllerInformation controller)
    {
        LightInteractionInformation info = (LightInteractionInformation)controller.GetFunctionalityInfoByType(typeof(LightInteractionInformation));
        WatchInteractionInformation watchInfo = (WatchInteractionInformation)controller.GetFunctionalityInfoByType(typeof(WatchInteractionInformation));

        if (watchInfo.watch.activeSelf && vrInteraction.interactingWith == this)
        {
            GameObject correctPart = null;
            if (info.isOnTime)
            {
                if ((watchInfo.isWatchOnFront && interactingLight.StartTime < 13) || (!watchInfo.isWatchOnFront && interactingLight.StartTime > 12))
                {
                    correctPart = ((VRWatchInteraction)vrInteraction).getCorrectPart(interactingLight.StartTime, controller);
                }
            }
            else
            {
                if ((watchInfo.isWatchOnFront && interactingLight.EndTime < 13) || (!watchInfo.isWatchOnFront && interactingLight.EndTime > 12))
                {
                    correctPart = ((VRWatchInteraction)vrInteraction).getCorrectPart(interactingLight.EndTime, controller);
                }
            }
            foreach (var item in watchInfo.allParts)
            {
                if (item != correctPart)
                    ((VRWatchInteraction)vrInteraction).SetPartToNormal(item, controller);
            }
            if (correctPart != null)
            {
                ((VRWatchInteraction)vrInteraction).SetPartToSelected(correctPart, controller);
            }

        }
    }

    protected override void OnInteract(GameObject go, ControllerInformation controller)
    {
        LightInteractionInformation info = (LightInteractionInformation)controller.GetFunctionalityInfoByType(typeof(LightInteractionInformation));
        LightInteractionInformation otherInfo = (LightInteractionInformation)controllerManager.getOtherController(controller).GetFunctionalityInfoByType(typeof(LightInteractionInformation));

        interactingLight = go.GetComponent<VRLight>();
        if (interactingLight)
        {
            ((VRWatchInteraction)vrInteraction).ShowWatch(controller);
            ((VRWatchInteraction)vrInteraction).ShowWatch(controllerManager.getOtherController(controller));
            vrInteraction.interactingWith = this;
            interactedThisFrame = true;
            info.OnOffTimeObject.SetActive(true);
            otherInfo.OnOffTimeObject.SetActive(true);
            ActiveController = controller;


            if (interactingLight.taskCompleted[0] != null)
            {
                interactingLight.taskCompleted[0].Invoke();
            }
        }
        else
        {
            controllerManager.GetController(controller.trackedObj).TriggerHapticPulse(3000);
        }
    }

    protected override void ActiveControllerUpdate(ControllerInformation controller)
    {
        if (vrInteraction.interactingWith && vrInteraction.interactingWith == this)
        {
            LightInteractionInformation info = (LightInteractionInformation)controller.GetFunctionalityInfoByType(typeof(LightInteractionInformation));
            LightInteractionInformation otherInfo = (LightInteractionInformation)controllerManager.getOtherController(controller).GetFunctionalityInfoByType(typeof(LightInteractionInformation));
            if (!interactedThisFrame)
            {
                if (controllerManager.GetController(controller.trackedObj).GetPressUp(vrInteraction.menuButton))
                {
                    vrInteraction.interactingWith = null;
                    ((VRWatchInteraction)vrInteraction).HideWatch(controller);
                    ((VRWatchInteraction)vrInteraction).HideWatch(controllerManager.getOtherController(controller));
                    info.OnOffTimeObject.SetActive(false);
                    otherInfo.OnOffTimeObject.SetActive(false);
                    ActiveController = null;
                }
            }
            else
            {
                interactedThisFrame = !interactedThisFrame;
            }
            //tutorial event..
            if (controllerManager.GetController(controller.trackedObj).GetPressDown(Valve.VR.EVRButtonId.k_EButton_Grip))
            {
                if (interactingLight.taskCompleted[1] != null)
                {
                    interactingLight.taskCompleted[1].Invoke();
                }
            }
            UpdateTime(controller);
        }
    }

    protected override void NonActiveControllerUpdate(ControllerInformation controller)
    {
        if (vrInteraction.interactingWith && vrInteraction.interactingWith == this)
        {
            UpdateTime(controller);
        }
    }

    protected override void AnyControllerUpdate(ControllerInformation controller) { }

    private void UpdateTime(ControllerInformation controller)
    {
        LightInteractionInformation info = (LightInteractionInformation)controller.GetFunctionalityInfoByType(typeof(LightInteractionInformation));
        WatchInteractionInformation watchInfo = (WatchInteractionInformation)controller.GetFunctionalityInfoByType(typeof(WatchInteractionInformation));
        Vector2 touchpadValue = controllerManager.GetController(controller.trackedObj).GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad);
        int time = ((VRWatchInteraction)vrInteraction).getTimeByVector(touchpadValue);
        if (time != 0)
        {
            if (!watchInfo.isWatchOnFront)
            {
                time += 12;
            }
            if (info.isOnTime && Mathf.Floor(time) != Mathf.Floor(interactingLight.StartTime))
            {
                interactingLight.StartTime = time;
                controllerManager.GetController(controller.trackedObj).TriggerHapticPulse(3000);
                UpdateDisplay(controller);
                if (interactingLight.taskCompleted[2] != null)
                {
                    interactingLight.taskCompleted[2].Invoke();
                }
            }
            else if (Mathf.Floor(time) != Mathf.Floor(interactingLight.EndTime))
            {
                interactingLight.EndTime = time;
                controllerManager.GetController(controller.trackedObj).TriggerHapticPulse(3000);
                UpdateDisplay(controller);
                if (interactingLight.taskCompleted[2] != null)
                {
                    interactingLight.taskCompleted[2].Invoke();
                }
            }
        }
    }

    protected override void AfterControllerIntialized()
    {
        var controllers = controllerManager.controllerInfos;

        for (int i = 0; i < controllers.Length; i++)
        {
            LightInteractionInformation info = (LightInteractionInformation)controllers[i].GetFunctionalityInfoByType(typeof(LightInteractionInformation));
            WatchInteractionInformation watchInfo = (WatchInteractionInformation)controllers[i].GetFunctionalityInfoByType(typeof(WatchInteractionInformation));

            info.isOnTime = (i == 0 ? true : false);

            info.OnOffTimeObject = (GameObject)Instantiate(accordingTimePrefab[i], watchInfo.watch.transform, false);
            info.OnOffTimeObject.SetActive(false);

        }
    }
}
