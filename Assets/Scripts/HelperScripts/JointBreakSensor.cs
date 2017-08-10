using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointBreakSensor : MonoBehaviour
{
    public System.Action<JointBreakSensor> jointBroke;

    private void OnJointBreak(float breakForce)
    {
        if (jointBroke != null)
        {
            jointBroke(this);
        }
    }
}
