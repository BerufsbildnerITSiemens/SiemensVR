using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Igniteable : MonoBehaviour
{
    public GameObject firePrefab;
    [HideInInspector]
    public List<Fire> fires;
    public float BoxColliderResizeFactor;
    public int maxFires = 1;

    // Use this for initialization
    void Start()
    {
        fires = new List<Fire>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    [ContextMenu("Ignite")]
    public void Ignite(Vector3 position)
    {
        if (fires.Count < maxFires)
        {
            Fire fire = Instantiate(firePrefab, transform).GetComponent<Fire>();
            BoxCollider fireBoxCollider = fire.GetComponent<BoxCollider>();
            Transform light = fire.transform.Find("Light");
            fire.transform.position = position;
            //fire.transform.localScale = new Vector3(1f / transform.position.x, 1f / transform.position.y, 1f / transform.position.z);
            fireBoxCollider.size = new Vector3(fire.GetComponent<BoxCollider>().size.x / (transform.localScale.x * BoxColliderResizeFactor), fire.GetComponent<BoxCollider>().size.y / (transform.localScale.y * BoxColliderResizeFactor), fire.GetComponent<BoxCollider>().size.z / (transform.localScale.z * BoxColliderResizeFactor));
            fireBoxCollider.center = Vector3.zero;
            fire.lit = true;
            light.localPosition = Vector3.zero;
            light.gameObject.SetActive(true);
            fire.GetComponent<ParticleSystem>().Play();
            fires.Add(fire);
            fire.extinguished += FireExtinguished;
        }
    }

    private void FireExtinguished(Fire fireToRemove)
    {
        fires.Remove(fireToRemove);
        Destroy(fireToRemove.gameObject);
    }
}
