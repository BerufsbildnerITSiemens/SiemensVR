using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Collider))]
public class VRObjectStand : MonoBehaviour
{
    public Color targetColor = Color.red;
    public System.Action<Keycard> keycardInserted;
    public System.Action<Keycard> keycardEjected;
    public Transform targetTransform;
    public VRButton ejectButton;
    public float ejectForce = 500f;

    private Color origColor;
    private MeshRenderer meshRenderer;
    private Keycard objectInRange;
    private Keycard keycardIn;

    ControllerManager controllerManager;
    // Use this for initialization
    void Start()
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
        if (keycardIn != null)
        {
            Destroy(gameObject.GetComponent<Joint>());
            keycardIn.GetComponent<Rigidbody>().AddForce(transform.forward * ejectForce);
        }
    }

    private void OnControllerDropObject(GameObject obj)
    {
        Keycard card = obj.GetComponent<Keycard>();
            
        if (card != null && card == objectInRange)
        {
            keycardIn = card;
            if (keycardInserted != null)
            {
                keycardInserted.Invoke(card);
                card.transform.position = targetTransform.position;
                card.transform.rotation = targetTransform.rotation;

                FixedJoint fx = gameObject.AddComponent<FixedJoint>();
                fx.breakForce = 5000;
                fx.breakTorque = 5000;
                fx.connectedBody = card.GetComponent<Rigidbody>();

            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Keycard card = other.GetComponent<Keycard>();
        if (card != null && objectInRange == null)
        {
            objectInRange = card;
            meshRenderer.material.color = Color.Lerp(origColor, targetColor, 0.5f);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Keycard card = other.GetComponent<Keycard>();
        if (card != null && objectInRange == null)
        {
            objectInRange = card;
            meshRenderer.material.color = Color.Lerp(origColor, targetColor, 0.5f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Keycard card = other.GetComponent<Keycard>();
        if (card != null && objectInRange == card)
        {
            if (keycardIn != null)
            {
                if (keycardEjected != null)
                {
                    keycardEjected.Invoke(keycardIn);
                }
                keycardIn = null;
            }
            objectInRange = null;
            meshRenderer.material.color = origColor;
        }
    }
}
