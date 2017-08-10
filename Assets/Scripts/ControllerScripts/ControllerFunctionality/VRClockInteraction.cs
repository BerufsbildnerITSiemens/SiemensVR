using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(VRWatchInteraction))]
public class VRClockInteraction : VRInteractionType
{
    private VRTime timeScript;
    [HideInInspector]
    public VRClock interactingClock;
    private bool interactedThisFrame = false;

    private VRWatchInteraction watchInteraction;

    protected override void Start()
    {
        base.Start();
        watchInteraction = GetComponent<VRWatchInteraction>();
        timeScript = GameObject.FindObjectOfType<VRTime>();
        timeScript.timeChanged += OnTimeChanged;
        ((VRWatchInteraction)vrInteraction).onInteract += OnInteract;
        ((VRWatchInteraction)vrInteraction).updateDisplay += UpdateDisplay;

    }


    protected override void OnInteract(GameObject go, ControllerInformation controller)
    {
        interactingClock = go.GetComponent<VRClock>();
        if (interactingClock)
        {
            ((VRWatchInteraction)vrInteraction).ShowWatch(controller);
            vrInteraction.interactingWith = this;
            interactedThisFrame = true;
            ActiveController = controller;
        }
        else
        {
            controllerManager.GetController(controller.trackedObj).TriggerHapticPulse(3000);
            ActiveController = null;
        }
    }
    private void OnTimeChanged(float newTime)
    {
        foreach (var item in controllerManager.controllerInfos)
        {
            UpdateDisplay(item);
        }

    }
    protected void UpdateDisplay(ControllerInformation controller)
    {
        WatchInteractionInformation info = (WatchInteractionInformation)controller.GetFunctionalityInfoByType(typeof(WatchInteractionInformation));

        if (info != null)
        {
            if (info.watch.activeSelf && vrInteraction.interactingWith == this)
            {
                foreach (var item in info.allParts)
                {
                    ((VRWatchInteraction)vrInteraction).SetPartToNormal(item, controller);
                }
                if (info.isWatchOnFront != timeScript.isNight)
                {
                    if (Mathf.FloorToInt(timeScript.Time) - 1 >= 0 && Mathf.FloorToInt(timeScript.Time) - 1 < info.allParts.Length)
                    {
                        ((VRWatchInteraction)vrInteraction).SetPartToSelected(((VRWatchInteraction)vrInteraction).getCorrectPart(Mathf.FloorToInt(timeScript.Time), controller), controller);
                    }
                }
            }
        }
    }
    public void SetTime(int newTime, ControllerInformation controller)
    {
        WatchInteractionInformation info = (WatchInteractionInformation)controller.GetFunctionalityInfoByType(typeof(WatchInteractionInformation));

        if (timeScript.Time != newTime)
        {
            float f = timeScript.Time - Mathf.Floor(timeScript.Time);
            timeScript.Time = newTime + f;
            timeScript.isNight = !info.isWatchOnFront;
            OnTimeChanged(timeScript.Time);

            //notification for tutorial
            if (interactingClock.taskCompleted[2] != null)
            {
                interactingClock.taskCompleted[2].Invoke();
            }
        }
    }

    protected override void ActiveControllerUpdate(ControllerInformation controller)
    {
        if (vrInteraction.interactingWith && vrInteraction.interactingWith == this)
        {
            if (controllerManager.GetController(controller.trackedObj).GetPressUp(vrInteraction.menuButton))
            {
                if (!interactedThisFrame)
                {
                    vrInteraction.interactingWith = null;
                    ((VRWatchInteraction)vrInteraction).HideWatch(controller);
                    ActiveController = null;
                }
                else
                {
                    interactedThisFrame = !interactedThisFrame;
                    //notification for tutorial
                    if (interactingClock.taskCompleted[0] != null)
                    {
                        interactingClock.taskCompleted[0].Invoke();
                    }
                }

            }
            if (controllerManager.GetController(controller.trackedObj).GetPressDown(Valve.VR.EVRButtonId.k_EButton_Grip))
            {
                //notification for tutorial
                if (interactingClock.taskCompleted[1] != null)
                {
                    interactingClock.taskCompleted[1].Invoke();
                }
            }
            Vector2 touchpadValue = controllerManager.GetController(controller.trackedObj).GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad);
            int time = ((VRWatchInteraction)vrInteraction).getTimeByVector(touchpadValue);
            if (time != 0 && Mathf.Floor(time) != Mathf.Floor(timeScript.Time))
            {
                SetTime(time, controller);
                controllerManager.GetController(controller.trackedObj).TriggerHapticPulse(3000);
            }
        }
    }

    protected override void NonActiveControllerUpdate(ControllerInformation controller) { }

    protected override void AnyControllerUpdate(ControllerInformation controller) { }

    protected override void AfterControllerIntialized() { }
}
