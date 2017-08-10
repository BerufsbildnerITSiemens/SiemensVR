using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTutorial : TutorialSection
{
    public LaserPointer laserPointer;

    protected override void TaskNeedsVerification()
    {
        base.NextTask();
    }
    protected override void SetTask(TutorialTask task)
    {
        if (activeTask != null)
        {
            switch (activeTask.eventIndex)
            {
                case 0:
                    laserPointer.OnLaserShow -= TaskNeedsVerification;
                    break;
                case 1:
                    laserPointer.OnTeleport -= TaskNeedsVerification;
                    break;
            }
        }

        activeTask = task;
        switch (activeTask.eventIndex)
        {
            case 0:
                laserPointer.OnLaserShow += TaskNeedsVerification;
                break;
            case 1:
                laserPointer.OnTeleport += TaskNeedsVerification;
                break;
        }

        base.UpdateDisplay();
    }


}
