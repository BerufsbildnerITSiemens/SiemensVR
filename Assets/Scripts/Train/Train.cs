using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train : MonoBehaviour
{
    [Header("Driving")]
    public Transform player;
    public Transform breakPoint;
    public float fullSpeed = 15f;
    public float breakSpeed = 0.05f;
    private float currentSpeed;

    [Header("Doors")]
    public Transform[] doorsLeft;
    public Transform[] doorsRight;
    public float doorMove = 1f;
    public float doorSpeed = 1f;
    private bool doorsOpen = false;
    private bool trainDepart = false;
    private bool start = false;
    public VRSensor Trigger;

    public VRSensor playerLeft;
    // Use this for initialization
    void Start()
    {
        currentSpeed = fullSpeed;
        playerLeft.triggerEnter += StationSensor;
        Trigger.triggerEnter += Start;
    }

    private void Start(Collider other, VRSensor sensor)
    {
        if (!start)
        {
            player.SetParent(this.transform, true);
            StartCoroutine(Drive(breakPoint));
        }
    }

    private void StationSensor(Collider other, VRSensor sensor)
    {
        if (!trainDepart)
        {
            SteamVR_Camera player = other.GetComponent<SteamVR_Camera>();
            if (player != null)
            {
                Depart();
                trainDepart = true;
            }
        }
    }

    IEnumerator Drive(Transform t)
    {
        if (currentSpeed < fullSpeed)
        {
            currentSpeed = fullSpeed / 100f;
        }
        while (currentSpeed < fullSpeed)
        {
            currentSpeed += Mathf.Lerp(0f, currentSpeed, breakSpeed);
            transform.position += (Vector3.back * currentSpeed * Time.deltaTime);
            yield return 0;
        }
        while (currentSpeed > 0.5f)
        {
            if (t != null && transform.position.z < t.position.z)
            {
                currentSpeed -= Mathf.Lerp(0f, currentSpeed, breakSpeed);
            }
            transform.position += (Vector3.back * currentSpeed * Time.deltaTime);
            yield return 0;
        }

        Stop();
    }

    public void Stop()
    {
        currentSpeed = 0;
        player.SetParent(null, true);

        StartCoroutine(OpenDoors());

    }

    public void Depart()
    {
        StartCoroutine(CloseDoors());
        Destroy(gameObject, 20);

    }

    private IEnumerator OpenDoors()
    {
        float moved = 0f;
        while (moved < doorMove)
        {
            foreach (var item in doorsLeft)
            {
                item.position -= Vector3.forward * doorSpeed * Time.deltaTime;
            }
            foreach (var item in doorsRight)
            {
                item.position -= Vector3.back * doorSpeed * Time.deltaTime;
            }
            moved += Mathf.Abs(doorSpeed * Time.deltaTime);
            yield return 0;
        }
        doorsOpen = true;
    }
    private IEnumerator CloseDoors()
    {
        float moved = 0f;
        while (moved < doorMove)
        {
            foreach (var item in doorsLeft)
            {
                item.position += Vector3.forward * doorSpeed * Time.deltaTime;
            }
            foreach (var item in doorsRight)
            {
                item.position += Vector3.back * doorSpeed * Time.deltaTime;
            }
            moved += Mathf.Abs(doorSpeed * Time.deltaTime);
            yield return 0;
        }
        doorsOpen = false;
        StartCoroutine(Drive(null));
    }
}
