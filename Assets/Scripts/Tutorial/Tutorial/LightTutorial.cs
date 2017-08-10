using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightTutorial : TutorialSection
{
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
                //check on and off time
                    base.NextTask();
                break;
        }
    }
}
