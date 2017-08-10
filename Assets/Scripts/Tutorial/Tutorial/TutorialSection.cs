using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(Collider))]
public abstract class TutorialSection : MonoBehaviour
{
    [Header("Tutorial Stuff")]
    public GameObject objectToTeachPrefab;
    public bool NoObjectToTeach = false;
    public Transform objectSpawnPoint;
    public GameObject screenPrefab;
    public Transform screenSpawnPoint;
    public GameObject[] borders;

    [Header("UI Stuff")]
    [HideInInspector]
    public Text title;
    [HideInInspector]
    public Text description;
    [HideInInspector]
    public RawImage image;

    private ScreenHoverBehaviour screenHover;
    private ScreenHoverBehaviour objectHover;

    public TutorialTask[] allTasks;
    protected TutorialTask activeTask;

    protected TutorialInteractable instanceOfObject;

    // Use this for initialization
    protected virtual void Start()
    {
        //StartTutorial();
    }

    protected abstract void TaskNeedsVerification(); //in subclass check if correct interaction then call base.NextTask() else do nothing

    protected void NextTask()
    {
        if (allTasks[allTasks.Length - 1] != activeTask)
        {
            SetTask(allTasks[Array.IndexOf(allTasks, activeTask) + 1]);
        }
        else
        {
            FinishTutorial();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        SteamVR_Camera cam = other.GetComponent<SteamVR_Camera>();
        if (cam)
        {
            StartTutorial();
            GetComponent<Collider>().enabled = false; //Dont start tutorial anymore
        }

    }

    private void StartTutorial()
    {
        activeTask = allTasks[0];

        //object instantiation
        GameObject go;
        if (!NoObjectToTeach)
        {

            go = Instantiate(objectToTeachPrefab);
            go.transform.SetParent(transform, true);
            go.transform.position = objectSpawnPoint.position;
            go.transform.rotation = objectSpawnPoint.rotation;
            instanceOfObject = go.GetComponent<TutorialInteractable>();
            if (instanceOfObject == null)
            {
                instanceOfObject = go.GetComponentInChildren<TutorialInteractable>();
                if (instanceOfObject == null)
                {
                    throw new System.Exception("ObjectToTeach does not have the TuztorialIntertactable Interface");
                }
            }
            objectHover = go.GetComponentInChildren<ScreenHoverBehaviour>();
        }


        //screen instantiation

        go = Instantiate(screenPrefab);
        go.transform.SetParent(transform, true);
        go.transform.position = screenSpawnPoint.position;
        go.transform.rotation = screenSpawnPoint.rotation;
        screenHover = go.GetComponentInChildren<ScreenHoverBehaviour>();

        image = GameObject.Find("TutorialImage").GetComponent<RawImage>();
        description = GameObject.Find("description").GetComponent<Text>(); //very awful (:
        title = GameObject.Find("Title").GetComponent<Text>();

        UpdateDisplay();

        SetTask(activeTask); //meh
    }

    protected virtual void SetTask(TutorialTask task)
    {
        if (NoObjectToTeach)
        {
            throw new Exception("The SetTask Methods needs to be overwritten if you dont have a object to teach.");
        }
        if (activeTask != null)
        {
            if (instanceOfObject.taskCompleted[activeTask.eventIndex] != null)
            {
                instanceOfObject.taskCompleted[activeTask.eventIndex] -= TaskNeedsVerification;
            }
        }

        activeTask = task;
        instanceOfObject.taskCompleted[activeTask.eventIndex] += TaskNeedsVerification;

        UpdateDisplay();
    }

    protected void UpdateDisplay()
    {
        if (activeTask != null)
        {
            title.text = activeTask.title;
            description.text = activeTask.description;
            image.texture = activeTask.image;
        }
    }

    [ContextMenu("finish")]
    public void FinishTutorial()
    {
        screenHover.FlyAwayAndDestroy();
        if (!NoObjectToTeach)
        {
            objectHover.FlyAwayAndDestroy();
        }

        if (borders != null)
        {
            foreach (var item in borders)
            {
                item.SetActive(false);
            }
        }
    }


}
