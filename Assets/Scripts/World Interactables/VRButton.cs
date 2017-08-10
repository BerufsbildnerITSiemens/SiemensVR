using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(Collider), typeof(MeshRenderer))]
public class VRButton : MonoBehaviour
{
    public int value;
    public Color targetColor = Color.red;
    public Action<int> buttonPressed;

    private Animator anim;
    private Color origColor;
    private MeshRenderer meshRenderer;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        origColor = meshRenderer.material.color;
        anim = GetComponent<Animator>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (buttonPressed != null)
        {
            buttonPressed.Invoke(value);
        }
        anim.SetTrigger("Press");
    }

    public void PressButton()
    {
        buttonPressed.Invoke(value);
        anim.SetTrigger("Press");
    }
    /// <summary>
    /// Gets called from the controller (ButtonInteract Script)
    /// </summary>
    public void OnControllerEnter()
    {
        meshRenderer.material.color = targetColor;
    }

    /// <summary>
    /// Gets called from the controller (ButtonInteract Script)
    /// </summary>
    public void OnControllerLeave()
    {
        meshRenderer.material.color = origColor;
    }


    /// <summary>
    /// Gets called from the controller (ButtonInteract Script)
    /// </summary>
    public void Interact()
    {
        if (buttonPressed != null)
        {
            buttonPressed.Invoke(value);
        }
        anim.SetTrigger("Press");
    }
}
