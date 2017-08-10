using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VROneAxisLever : MonoBehaviour
{
    public Transform refrencePoint;
    public Transform origPos;
    [HideInInspector]
    public int value { get; private set; }


    void Update()
    {
        value = Mathf.RoundToInt((origPos.position.z - refrencePoint.position.z) * -200f);

        //value = Mathf.RoundToInt(transform.localRotation.eulerAngles.x) - 270;

        //if (transform.localRotation.eulerAngles.y < 179)
        //{
        //    value *= -1;
        //}
    }
}
