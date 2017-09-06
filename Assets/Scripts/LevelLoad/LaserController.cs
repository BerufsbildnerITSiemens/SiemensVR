using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour {

    private void OnCollisionEnter(Collision collision)
    {
        GameObject uzu = new GameObject();
        uzu = collision.gameObject;
    }
}
