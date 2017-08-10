using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRWatchInteraction : VRInteraction
{
    #region watch Properties
    [Header("Watch Properties")]
    public GameObject watchPrefab;

    public float FadeSpeed = 1;
    public float TurnSpeed = 4;

    public Material normalMaterial;
    public Material highlightedMaterial;
    public int normalSize;
    public int highlightedSize;

    public Action<ControllerInformation> updateDisplay;

    private Coroutine[] allCoroutines;

    #endregion
    private Transform head;

    protected override void Awake()
    {
        info = new WatchInteractionInformation();
        allCoroutines = new Coroutine[12];

        base.Awake();
    }

    protected override void Start()
    {
        base.Start();

        head = GameObject.FindObjectOfType<SteamVR_Camera>().transform;
    }

    public void ShowWatch(ControllerInformation controller)
    {
        WatchInteractionInformation info = (WatchInteractionInformation)controller.GetFunctionalityInfoByType(typeof(WatchInteractionInformation));
        info.watch.SetActive(true);
        updateDisplay(controller);
        //foreach (var item in controllerManager.controllerInfos)
        //{
        //    Debug.Log(item.functionalityInformations.Count);
        //}
    }
    public void HideWatch(ControllerInformation controller)
    {
        WatchInteractionInformation info = (WatchInteractionInformation)controller.GetFunctionalityInfoByType(typeof(WatchInteractionInformation));
        info.watch.SetActive(false);
    }

    public int getTimeByVector(Vector2 vec)
    {
        if (vec == Vector2.zero)
        {
            return 0;
        }

        Vector3 forward = new Vector3(vec.x, 0, vec.y);
        Vector3 upward = new Vector3(0, 1, 0);
        Vector3 rotation = Quaternion.LookRotation(forward, upward).eulerAngles;

        int returnValue = Mathf.FloorToInt(rotation.y / 30f);
        if (returnValue == 0)
        {
            return 12;
        }
        return returnValue;
    }

    public void SetPartToNormal(GameObject part, ControllerInformation controllerInfo)
    {
        int index = Array.IndexOf(GetOrderedParts(controllerInfo), part);
        if (allCoroutines[index] != null)
        {
            StopCoroutine(allCoroutines[index]);
        }
        allCoroutines[index] = StartCoroutine(_SetPartToNormal(part));
    }
    public void SetPartToSelected(GameObject part, ControllerInformation controllerInfo)
    {
        int index = Array.IndexOf(GetOrderedParts(controllerInfo), part);
        if (allCoroutines[index] != null)
        {
            StopCoroutine(allCoroutines[index]);
        }
        allCoroutines[index] = StartCoroutine(_SetPartToSelected(part));
    }

    private IEnumerator _SetPartToNormal(GameObject part)
    {

        MeshRenderer rend = part.GetComponent<MeshRenderer>();
        rend.material = normalMaterial;
        while (part.transform.localScale.x > normalSize)
        {
            part.transform.localScale = part.transform.localScale - Vector3.one * FadeSpeed;
            yield return 0;
        }
    }
    private IEnumerator _SetPartToSelected(GameObject part)
    {
        MeshRenderer rend = part.GetComponent<MeshRenderer>();
        rend.material = highlightedMaterial;
        while (part.transform.localScale.x < highlightedSize)
        {
            part.transform.localScale = part.transform.localScale + Vector3.one * FadeSpeed;
            yield return 0;
        }
    }
    private GameObject[] GetOrderedParts(ControllerInformation controller)
    {
        WatchInteractionInformation info = (WatchInteractionInformation)controller.GetFunctionalityInfoByType(typeof(WatchInteractionInformation));

        GameObject[] parts = new GameObject[12];
        for (int i = 0; i < parts.Length; i++)
        {
            parts[i] = info.watch.transform.GetChild(0).Find("Part" + (i + 1)).gameObject;
        }
        return parts;
    }

    public IEnumerator SwitchSide(ControllerInformation controller)
    {
        WatchInteractionInformation info = (WatchInteractionInformation)controller.GetFunctionalityInfoByType(typeof(WatchInteractionInformation));
        if (info.isSwitchingSide)
        {
            yield break;
        }
        updateDisplay(controller);
        info.isWatchOnFront = !info.isWatchOnFront;
        info.isSwitchingSide = true;
        Transform child = info.watch.transform.GetChild(0);
        Vector3 oldRot = child.localRotation.eulerAngles;
        Vector3 targetRot = oldRot;
        targetRot.z += 180;
        while (child.transform.localRotation != Quaternion.Euler(targetRot))
        {
            child.transform.localRotation = Quaternion.RotateTowards(child.transform.localRotation, Quaternion.Euler(targetRot), TurnSpeed);
            yield return 0;
        }
        info.isSwitchingSide = false;
    }
    public GameObject getCorrectPart(int time, ControllerInformation controller)
    {
        WatchInteractionInformation info = (WatchInteractionInformation)controller.GetFunctionalityInfoByType(typeof(WatchInteractionInformation));
        int newTime = time;
        if (newTime > 12)
        {
            newTime -= 12;
        }
        if (info.isWatchOnFront)
        {
            return info.allParts[newTime - 1];
        }
        else
        {
            return info.allParts[12 - newTime];
        }
    }

    protected override void OnControllerInitialized()
    {
        base.OnControllerInitialized();

        foreach (var item in controllerManager.controllerInfos)
        {
            WatchInteractionInformation info = (WatchInteractionInformation)item.GetFunctionalityInfoByType(typeof(WatchInteractionInformation));


            info.watch = Instantiate(watchPrefab, item.trackedObj.transform);
            info.watch.SetActive(false);
            info.allParts = GetOrderedParts(item);
        }

    }



    protected override void AnyControllerUpdate(ControllerInformation controller)
    {
        base.AnyControllerUpdate(controller);
        WatchInteractionInformation info = (WatchInteractionInformation)controller.GetFunctionalityInfoByType(typeof(WatchInteractionInformation));
        if (interactingWith)
        {
            if (controllerManager.GetController(controller.trackedObj).GetPressDown(gripButton))
            {
                StartCoroutine(SwitchSide(controller));
            }
        }
        info.watch.transform.LookAt(head);
    }

    protected override void AfterControllerIntialized() { }
}
