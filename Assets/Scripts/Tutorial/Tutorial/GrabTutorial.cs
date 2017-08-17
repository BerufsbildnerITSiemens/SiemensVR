using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GrabTutorial : TutorialSection
{
    public GameObject Target;
    public ThrowObject Cube;

    protected override void Start()
    {
        base.Start();
    }
    protected override void TaskNeedsVerification()
    {
        switch (activeTask.eventIndex)
        {
            case 0:
                base.NextTask();
                break;
            case 1:
                base.NextTask();
                break;
        }
    }
    [ContextMenu("finish")]
    public void finish()
    {
        FinishTutorial();
    }

    private void OnTriggerExit(Collider other)
    {
        GameObject obj;
        if(other.gameObject.name == "Cube" && tutorialStarted == true)
        {
            Destroy(objectToTeach, 1);
            obj = Instantiate(objectToTeachPrefab);
            obj.transform.SetParent(transform, true);
            obj.transform.position = objectSpawnPoint.position;
            obj.transform.rotation = objectSpawnPoint.rotation;
            instanceOfObject = obj.GetComponentInChildren<TutorialInteractable>();
            objectToTeach = obj;

            instanceOfObject.taskCompleted[activeTask.eventIndex] += TaskNeedsVerification;
        }
    }
}
