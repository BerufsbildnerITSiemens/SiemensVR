using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRTwoAxisLever : MonoBehaviour
{

    public Vector2 value { get; private set; }
    public Transform origPos;
    public Transform refrencePos;


    void Update()
    {
        int xDistance = Mathf.RoundToInt((origPos.position.x - refrencePos.position.x)*100f);
        int zDistance = Mathf.RoundToInt((origPos.position.z - refrencePos.position.z)*100f);

        value = new Vector2(xDistance, -zDistance);
    }
}
