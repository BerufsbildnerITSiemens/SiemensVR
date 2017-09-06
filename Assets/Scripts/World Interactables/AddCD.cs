using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class AddCD : MonoBehaviour {
    public VRSensor Trigger;
    public Color targetColor = Color.red;
    public CD cd;
    public System.Action<CD> cdInserted;
    public System.Action<CD> cdEjected;
    public Transform targetTransform;
    public VRButton ejectButton;
    public VRButton playButton;
    public float ejectForce = 500f;
    private bool buttonPlayClicked = false;
    private SongState audio = SongState.stopped;
    private enum SongState { stopped, playing, paused };
    private Color origColor;
    private MeshRenderer meshRenderer;
    private CD objectInRange;
    private CD cdIn;
    private AudioSource Speaker;

    ControllerManager controllerManager;
    // Use this for initialization
    void Start ()
    {
        Trigger.triggerLeave += triggerLeave;
        controllerManager = GameObject.FindObjectOfType<ControllerManager>();
        meshRenderer = GetComponent<MeshRenderer>();
        origColor = meshRenderer.material.color;
        Speaker = GetComponent<AudioSource>();
        foreach (var item in controllerManager.controllerInfos)
        {
            ((GrabObjectInformation)item.GetFunctionalityInfoByType(typeof(GrabObjectInformation))).ObjectReleased += AddCd;
        }
        ejectButton.buttonPressed += OnEject;
        playButton.buttonPressed += OnPlay;
    }

    private void Update()
    {
        if(cdIn != null)    //CD inserted
        {
            if (buttonPlayClicked)  //Button on Play Mode
            {
                if(audio == SongState.stopped)  //Audio stopped
                {
                    Speaker.Play();                 //Playing Audio
                    audio = SongState.playing;     //Audio Playing
                }
                if(audio == SongState.paused)   //Audio playing
                {
                    Speaker.UnPause();           //Continue Audio
                    audio = SongState.playing;  //Audio Plaing
                }
            }
            else if(audio == SongState.playing)
            {
                Speaker.Pause();              //Pause Audio
                audio = SongState.paused;    //Audio Paused
            }
        } 
    }


    private void OnPlay(int no)
    {
        if(buttonPlayClicked == false)
        {
            buttonPlayClicked = true;
        }
        else
        {
            buttonPlayClicked = false;
        }
    }

    private void OnEject(int no)
    {
        if(cdIn != null)
        {
            cdIn = null;
            var geschwindigkeit = 1.5f;
            var richtung = new Vector3(9f, 4f, 0f);
            Speaker.Stop();             //Stop Audio
            audio = SongState.stopped;   //Audio Stopped
            buttonPlayClicked = false;

            cd.transform.position += richtung * geschwindigkeit * Time.deltaTime;
        }
    }

    private void AddCd(GameObject obj)
    {
        CD cd_real = obj.GetComponent<CD>();

        if (cd_real != null && cd_real == objectInRange && cdIn == null)
        {
            cdIn = cd_real;
            Speaker.clip = obj.GetComponent<CD>().song;
            cd = cd_real;
            cd_real.transform.position = targetTransform.position;
            cd_real.transform.rotation = targetTransform.rotation;

            FixedJoint fx = gameObject.AddComponent<FixedJoint>();
            fx.breakForce = 5000;
            fx.breakTorque = 5000;
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        CD cd_real = other.GetComponent<CD>();
        if (cd_real != null && objectInRange == null)
        {
            cd = cd_real;
            objectInRange = cd_real;
            meshRenderer.material.color = Color.Lerp(origColor, targetColor, 0.5f);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        CD cd_real = other.GetComponent<CD>();
        if (cd_real != null && objectInRange == null)
        {
            objectInRange = cd_real;
            cd = cd_real;
            meshRenderer.material.color = Color.Lerp(origColor, targetColor, 0.5f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        CD cd_real = other.GetComponent<CD>();
        if (cd_real != null && objectInRange == cd_real)
        {
            if (cdIn != null)
            {
                if (cdEjected != null)
                {
                    cdEjected.Invoke(cdIn);
                }
            }
            objectInRange = null;
            meshRenderer.material.color = origColor;
        }
    }
    private void triggerLeave(Collider collider, VRSensor sensor)
    {
        if(audio == SongState.playing){
            Speaker.Pause();
            audio = SongState.paused;
            buttonPlayClicked = false;
        }
    }

}