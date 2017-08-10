using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITracked : MonoBehaviour
{
    public RectTransform trackedUIObject;
    public Vector3 offset;
    public Vector3 setScale;
    public SizeApplyMode sizeApplyMode;

    public enum SizeApplyMode
    {
        None, FromTrackedObject, SetSize
    }

    private void Update()
    {
        transform.position = trackedUIObject.position + offset;

        switch (sizeApplyMode)
        {
            case SizeApplyMode.FromTrackedObject:
                transform.localScale = trackedUIObject.localScale;
                break;
            case SizeApplyMode.SetSize:
                transform.localScale = setScale;
                break;
        }
    }

}
