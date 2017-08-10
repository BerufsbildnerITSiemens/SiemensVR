using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirPlane : MonoBehaviour
{
    public Transform[] waypoints;
    public float flightSpeed = 3f;
    public float turnSpeed = 2f;
    public float maxWaypointDistance = 5;
    private int activeWaypoint = 0;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * flightSpeed * Time.deltaTime;

        Vector3 dir = (waypoints[activeWaypoint].position - transform.position).normalized;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(dir), turnSpeed);

        if (Vector3.Distance(waypoints[activeWaypoint].position, transform.position) < maxWaypointDistance)
        {
            activeWaypoint++;
            if (waypoints.Length <= activeWaypoint)
            {
                activeWaypoint = 0;
            }
        }

    }
    private void OnDrawGizmos()
    {
        for (int i = 0; i < waypoints.Length; i++)
        {
            Gizmos.color = Color.blue;
            if (i == waypoints.Length - 1)
            {
                Gizmos.DrawLine(waypoints[i].position, waypoints[0].position);
            }
            else
            {
                Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
            }
            if (i == activeWaypoint)
            {
                Gizmos.color = Color.red;
            }
            else
            {
                Gizmos.color = Color.blue;
            }
            Gizmos.DrawSphere(waypoints[i].position, maxWaypointDistance);
        }
    }
}
