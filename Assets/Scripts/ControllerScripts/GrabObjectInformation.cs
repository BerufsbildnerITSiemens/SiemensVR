using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GrabObjectInformation : ControllerFunctionalityInformation {

    /// <summary>
    /// Event which gets fires when we relrease a object.
    /// Gameobject is the object we release.
    /// </summary>
    public Action<GameObject> ObjectReleased;

    /// <summary>
    /// When our controller collide with a object we save it here
    /// </summary>
    public GameObject collidingObject;
    /// <summary>
    /// When we grab a object we save it here
    /// </summary>
    public GameObject objectInHand;
}
