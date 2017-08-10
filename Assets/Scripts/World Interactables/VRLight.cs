using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class VRLight : TutorialInteractable
{
    public Light controlledLight;
    private float defaultIntensity;
    private VRTime time;

    private int _StartTime;
    public int StartTime
    {
        get
        {
            return _StartTime;
        }
        set
        {
            if (value <= 24 && value > 0)
            {
                _StartTime = value;
                UpdateLightStatus();
            }
        }
    }

    private int _EndTime;
    public int EndTime
    {
        get
        {
            return _EndTime;
        }
        set
        {
            if (value <= 24 && value > 0)
            {
                _EndTime = value;
                UpdateLightStatus();
            }
        }
    }

    private Action Interacted; //gets called from the interactor 
    private Action SideChanged; //gets called from the interactor 
    private Action TimeSet; //gets called from the interactor 

    private void Awake()
    {
        taskCompleted = new Action[] { Interacted, SideChanged, TimeSet };
    }
    private void Start()
    {
        _StartTime = 1;
        _EndTime = 1;
        time = GameObject.FindObjectOfType<VRTime>();
        time.timeChanged += OnTimeChanged;
        defaultIntensity = controlledLight.intensity;
        UpdateLightStatus();
    }

    private void OnTimeChanged(float time)
    {
        UpdateLightStatus();
    }

    private void UpdateLightStatus()
    {
        if ((time.Time > _StartTime && time.Time < _EndTime) || _StartTime == _EndTime || (_StartTime > _EndTime && (time.Time >= _StartTime || time.Time <= _EndTime)))
        {
            if (controlledLight != null)
            {
                controlledLight.intensity = defaultIntensity;
            }
        }
        else
        {
            if (controlledLight != null)
            {
                controlledLight.intensity = 0f;
            }
        }
    }
}
