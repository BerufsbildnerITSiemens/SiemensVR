using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockTutorial : TutorialSection
{
    private VRTime timeScript;

    protected override void Start()
    {
        timeScript = GameObject.FindObjectOfType<VRTime>();
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
            case 2:
                if (Mathf.FloorToInt(timeScript.Time) == 5)
                {
                    base.NextTask();
                }
                break;
        }
    }
    [ContextMenu("finish")]
    public void finish()
    {
        FinishTutorial();
    }
}
