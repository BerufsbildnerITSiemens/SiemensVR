using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Print : MonoBehaviour
{
    public VRButton button;
    public VRSensor sensor;
    public Transform output;
    public List<GameObject> inPrinter;
    [HideInInspector]
    public bool controllerInPrinter = false;
    [HideInInspector]
    public bool cameraInPrinter = false;


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
        if (collider.gameObject.transform.name != "Controller (left)" && collider.gameObject.transform.name != "Controller (right)" && collider.gameObject.transform.name != "Camera (eye)")
        {
            inPrinter.Add(collider.gameObject);
        }
        else if (collider.gameObject.transform.name == "Controller (left)" || collider.gameObject.transform.name == "Controller (right)")
        {
            controllerInPrinter = true;
        }
        else if (collider.gameObject.transform.name == "Camera (eye)")
        {
            cameraInPrinter = true;
        }
    }

    private void triggerLeave(Collider collider, VRSensor sensor)
    {
        if (collider.gameObject.transform.name != "Controller (left)" && collider.gameObject.transform.name != "Controller (right)" && collider.gameObject.transform.name != "Camera (eye)")
        {
            inPrinter.Remove(collider.gameObject);
        }
        else if (collider.gameObject.transform.name == "Controller (left)" || collider.gameObject.transform.name == "Controller (right)")
        {
            controllerInPrinter = false;
        }
        else if (collider.gameObject.transform.name == "Camera (eye)")
        {
            cameraInPrinter = false;
        }
    }

}
