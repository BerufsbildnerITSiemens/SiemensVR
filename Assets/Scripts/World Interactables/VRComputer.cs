using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRComputer : MonoBehaviour
{
    public VRButton[] buttons;
    public VROneAxisLever oneAxisLever;
    public VRTwoAxisLever twoAxisLever;

    public float zoomFactor = 0.1f;
    public float rotateFactor = 0.1f;

    public RenderTexture tex;

    private SecurityCamera[] cameras;
    private int activeCamera = 0;

    // Use this for initialization
    void Start()
    {
        if (buttons != null && buttons.Length > 0)
        {
            foreach (var item in buttons)
            {
                item.buttonPressed += OnButtonPress;
            }
        }
        cameras = GameObject.FindObjectsOfType<SecurityCamera>();

        SetActiveCamera(activeCamera);
    }

    // Update is called once per frame
    void Update()
    {
        if (oneAxisLever.value != 0)
        {
            cameras[activeCamera].SetZoom(oneAxisLever.value * zoomFactor);
        }
        if (twoAxisLever.value != Vector2.zero)
        {
            cameras[activeCamera].Rotate(twoAxisLever.value * rotateFactor);
        }
    }

    private void OnButtonPress(int value)
    {
        switch (value)
        {
            case 1:
                break;
            case 2:
                if (activeCamera + 1 < cameras.Length)
                {
                    SetActiveCamera(activeCamera + 1);
                }
                else
                {
                    SetActiveCamera(0);
                }
                break;
            case 3:
                if (activeCamera - 1 >= 0)
                {
                    SetActiveCamera(activeCamera - 1);
                }
                else
                {
                    SetActiveCamera(cameras.Length - 1);
                }
                break;
            default:
                throw new System.Exception("Case not defined");
        }
    }
    private void SetActiveCamera(int index)
    {
        cameras[activeCamera].RemoveTargetTexture();
        activeCamera = index;
        cameras[activeCamera].SetTargetTexture(tex);
    }
}
