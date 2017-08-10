using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class VRElevator : MonoBehaviour
{
    public int floorIndex;
    public VRButton[] buttons;
    public Transform yPosOfFloor;
    public float secondsForOneFloor;
    public Transform[] doors;
    public float doorMinScale;
    public float doorMaxScale;
    public float doorSpeed;
    public VRSensor doorSensor;
    private VRElevator[] allElevators;
    private bool doorMoving = false;

    private List<GameObject> elevatorContent;

    void Start()
    {
        elevatorContent = new List<GameObject>();
        allElevators = GameObject.FindObjectsOfType<VRElevator>();

        doorSensor.triggerEnter += OnSensorEnter;
        doorSensor.triggerLeave += OnSensorLeave;

        foreach (var item in buttons)
        {
            item.buttonPressed += OnButtonPress;
        }
    }

    private void OnSensorEnter(Collider other, VRSensor sensor)
    {
        if (other.GetComponent<SteamVR_Camera>())
        {
            StartCoroutine(OpenDoors());
        }
    }

    private void OnSensorLeave(Collider other, VRSensor sensor)
    {
        if (other.GetComponent<SteamVR_Camera>())
        {
            StartCoroutine(CloseDoors());
        }
    }

    private void OnButtonPress(int value)
    {
        if (value != floorIndex && value < 3 && value >= 0)
        {

            StartCoroutine(ChangeToFloor(value));
        }
    }

    private IEnumerator ChangeToFloor(int newFloorIndex)
    {

        //Close doors
        StartCoroutine(CloseDoors());
        while (doorMoving)
        {
            yield return 0;
        }

        //*move up* (not really, just wait)
        for (int i = 0; i < Mathf.Abs(floorIndex - newFloorIndex); i++)
        {
            yield return new WaitForSeconds(secondsForOneFloor);
        }

        //get target Elevator
        VRElevator targetElevator = null;
        for (int x = 0; x < allElevators.Length; x++)
        {
            if (allElevators[x].floorIndex == newFloorIndex)
            {
                targetElevator = allElevators[x];
                break;
            }
        }

        //move objects to new their new Position

        if (elevatorContent != null)
        {
            foreach (var item in elevatorContent)
            {
                Vector3 newPos = item.transform.position;
                Vector3 targetPosOffset = newPos - yPosOfFloor.position;

                newPos.y = targetPosOffset.y + targetElevator.yPosOfFloor.position.y;
                item.transform.position = newPos;
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<ControllerGrabObject>() || elevatorContent.Contains(other.gameObject))
        {
            return;
        }
        if (other.GetComponent<SteamVR_Camera>() || other.GetComponent<Rigidbody>())
        {
            if (other.GetComponent<SteamVR_Camera>())
            {
                GameObject rig = other.GetComponentInParent<SteamVR_ControllerManager>().gameObject;
                if (elevatorContent.Contains(rig))
                {
                    return;
                }
                elevatorContent.Add(rig);
            }
            else
            {
                elevatorContent.Add(other.gameObject);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<SteamVR_Camera>() || other.GetComponent<Rigidbody>())
        {
            elevatorContent.Remove(other.gameObject);
        }
    }
    private IEnumerator OpenDoors()
    {
        doorMoving = true;
        while (doors[0].localScale.x  > doorMinScale)
        {

            for (int i = 0; i < doors.Length; i++)
            {
                Vector3 scale = doors[i].localScale;
                scale.x -= doorSpeed * Time.deltaTime;
                scale.x = Mathf.Clamp(scale.x, doorMinScale, doorMaxScale);
                doors[i].localScale = scale;
            }
            yield return 0;
        }
        doorMoving = false;
    }
    private IEnumerator CloseDoors()
    {
        doorMoving = true;
        while (doors[0].localScale.x < doorMaxScale)
        {

            for (int i = 0; i < doors.Length; i++)
            {
                Vector3 scale = doors[i].localScale;
                scale.x += doorSpeed * Time.deltaTime;
                scale.x = Mathf.Clamp(scale.x, doorMinScale, doorMaxScale);
                doors[i].localScale = scale;
            }
            yield return 0;
        }
        doorMoving = false;
    }
}

