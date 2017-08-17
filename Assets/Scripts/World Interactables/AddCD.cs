using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Collider))]
public class AddCD : MonoBehaviour {

    public Color targetColor = Color.red;
    public System.Action<CD> cdInserted;
    public System.Action<CD> cdEjected;
    public Transform targetTransform;
    public VRButton ejectButton;
    public float ejectForce = 500f;

    private Color origColor;
    private MeshRenderer meshRenderer;
    private CD objectInRange;
    private CD cdIn;

    ControllerManager controllerManager;
    // Use this for initialization
    void Start ()
    {
        controllerManager = GameObject.FindObjectOfType<ControllerManager>();
        meshRenderer = GetComponent<MeshRenderer>();
        origColor = meshRenderer.material.color;

        foreach (var item in controllerManager.controllerInfos)
        {
            ((GrabObjectInformation)item.GetFunctionalityInfoByType(typeof(GrabObjectInformation))).ObjectReleased += OnControllerDropObject;
        }
        ejectButton.buttonPressed += OnEject;
    }

    private void OnEject(int no)
    {
        if (cdIn != null)
        {
            Destroy(gameObject.GetComponent<Joint>());
            cdIn.GetComponent<Rigidbody>().AddForce(transform.forward * ejectForce);
        }
    }

    private void OnControllerDropObject(GameObject obj)
    {
        CD cd = obj.GetComponent<CD>();

        if (cd != null && cd == objectInRange)
        {
            cdIn = cd;

            cd.transform.position = targetTransform.position;
            cd.transform.rotation = targetTransform.rotation;

            FixedJoint fx = gameObject.AddComponent<FixedJoint>();
            fx.breakForce = 5000;
            fx.breakTorque = 5000;
            fx.connectedBody = cd.GetComponent<Rigidbody>();

            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        CD cd = other.GetComponent<CD>();
        if (cd != null && objectInRange == null)
        {
            objectInRange = cd;
            meshRenderer.material.color = Color.Lerp(origColor, targetColor, 0.5f);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        CD cd = other.GetComponent<CD>();
        if (cd != null && objectInRange == null)
        {
            objectInRange = cd;
            meshRenderer.material.color = Color.Lerp(origColor, targetColor, 0.5f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        CD cd = other.GetComponent<CD>();
        if (cd != null && objectInRange == cd)
        {
            if (cdIn != null)
            {
                if (cdEjected != null)
                {
                    cdEjected.Invoke(cdIn);
                }
                cdIn = null;
            }
            objectInRange = null;
            meshRenderer.material.color = origColor;
        }
    }
}
