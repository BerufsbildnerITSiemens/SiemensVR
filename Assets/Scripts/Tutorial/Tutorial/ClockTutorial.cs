using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

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
        // Get call stack
        StackTrace stackTrace = new StackTrace();

        // Get calling method name
        Console.WriteLine(stackTrace.GetFrame(1).GetMethod().Name);
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
