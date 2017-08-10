using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WatchInteractionInformation : ControllerFunctionalityInformation
{
    public Action updateDisplay;
    public GameObject watch;
    public GameObject[] allParts;
    public bool isWatchOnFront = true;
    public bool isSwitchingSide = false;
}
