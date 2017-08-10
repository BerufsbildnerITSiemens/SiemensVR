using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRFireExtinguisherInteraction : VRInteractionType
{

    [HideInInspector]
    public FireExtinguisher interactingFireExtinguisher;

    private GameObject OnOffSign;
    public GameObject OnOffSignPrefab;
    public float turnSpeed;

    private bool isSwitchingSide = false;
    private Transform head;
    private bool isOn = false;


    private bool interactedThisFrame = false;
    public Vector3 buttonOffset;

    protected override void Start()
    {
        base.Start();
        vrInteraction = GetComponent<VRInteraction>();
        vrInteraction.onInteract += OnInteract;

        head = FindObjectOfType<SteamVR_Camera>().transform;
        OnOffSign = Instantiate(OnOffSignPrefab);
        OnOffSign.SetActive(false);
    }

    protected override void OnInteract(GameObject go, ControllerInformation controller)
    {
        interactingFireExtinguisher = go.GetComponent<FireExtinguisher>();

        if (interactingFireExtinguisher)
        {
            ShowButton(controller);
            vrInteraction.interactingWith = this;
            interactedThisFrame = true;
            ActiveController = controller;
        }
        else
        {
            controllerManager.GetController(controller.trackedObj).TriggerHapticPulse(3000);
        }
    }

    public void ActivateFireExtinguisher(bool state)
    {
        interactingFireExtinguisher.SetActivated(state);
    }

    public void ShowButton(ControllerInformation controller)
    {
        OnOffSign.SetActive(true);
        OnOffSign.transform.SetParent(controller.trackedObj.transform);
        OnOffSign.transform.localPosition = buttonOffset;
    }

    public void HideButton()
    {
        OnOffSign.SetActive(false);
    }

    public IEnumerator SwitchSide()
    {
        if (isSwitchingSide)
            yield break;

        isOn = !isOn;
        isSwitchingSide = true;
        Vector3 oldRot = OnOffSign.transform.GetChild(0).localRotation.eulerAngles;
        Vector3 targetRot = new Vector3(oldRot.x, oldRot.y - 180, oldRot.z);

        while (OnOffSign.transform.GetChild(0).localRotation != Quaternion.Euler(targetRot))
        {
            //Vector3 newRot = OnOffSign.transform.GetChild(0).localRotation.eulerAngles;
            //newRot.y -= turnSpeed * Time.deltaTime * 50f;
            //OnOffSign.transform.GetChild(0).localRotation = Quaternion.Euler(newRot);

            //OnOffSign.transform.localRotation = Quaternion.Euler(Vector3.RotateTowards(OnOffSign.transform.localRotation.eulerAngles, targetRot, turnSpeed, 0));

            OnOffSign.transform.GetChild(0).localRotation = Quaternion.RotateTowards(OnOffSign.transform.GetChild(0).localRotation, Quaternion.Euler(targetRot), turnSpeed);
            yield return 0;
        }

        isSwitchingSide = false;
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
                    HideButton();
                    ActiveController = null;
                }
                else
                {
                    interactedThisFrame = !interactedThisFrame;
                }
            }
            if (controllerManager.GetController(controller.trackedObj).GetPressDown(vrInteraction.gripButton))
            {
                interactingFireExtinguisher.SetActivated(!interactingFireExtinguisher.activated);
                StartCoroutine(SwitchSide());
            }
            OnOffSign.transform.LookAt(head);
        }
    }

    protected override void NonActiveControllerUpdate(ControllerInformation controller) { }

    protected override void AnyControllerUpdate(ControllerInformation controller) { }

    protected override void AfterControllerIntialized() { }
}
