using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Print : MonoBehaviour
{
    public VRButton button;
    public VRSensor sensor;
    public Transform output;
    public List<GameObject> inPrinter;
    public bool controllerInPrinter = false;

    // Use this for initialization
    void Start()
    {
        button.buttonPressed += buttonPressed;
        sensor.triggerEnter += triggerEnter;
        sensor.triggerLeave += triggerLeave; 
    }

    private void buttonPressed(int value)
    {
        if (inPrinter != null)
        {
            if (controllerInPrinter == false)
            {
                for (int i = 0; i < inPrinter.Count; i++)
                {
                    GameObject go = Instantiate(inPrinter[i], transform, true);
                    go.transform.position = output.position;
                }    
            }
        }
    }

    private void triggerEnter(Collider collider, VRSensor sensor)
    {
        if(collider.gameObject.transform.name == "Controller (left)" || collider.gameObject.transform.name == "Controller (right)")
        {
            controllerInPrinter = true;
        }
        else
        {
            inPrinter.Add(collider.gameObject);
        }
        
    }

    private void triggerLeave(Collider collider, VRSensor sensor)
    {
        if(collider.gameObject.transform.name == "Controller (left)" || collider.transform.name == "Controller (right)")
        {
            controllerInPrinter = false;
        }
        else
        {
            inPrinter.Remove(collider.gameObject);
        }
    }

}
