using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ThrowObject : TutorialInteractable
{
    private Action Grabbed; //gets called from the interactor 
    private Action Hit; //gets called from the interactor 
    private ThrowObject Cube;

    private void Awake()
    {
        taskCompleted = new Action[] { Grabbed, Hit };
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Screen")
        {
            Cube = GetComponent<ThrowObject>();
            if (Cube.taskCompleted[1] != null)
            {
                Cube.taskCompleted[1].Invoke();
            }
        }
    }

}
