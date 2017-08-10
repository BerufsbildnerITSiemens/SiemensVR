using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Keycard : MonoBehaviour
{
    /// <summary>
    /// The room numbers are 1 - 5
    /// </summary>
    public List<int> permittedRooms;

}
