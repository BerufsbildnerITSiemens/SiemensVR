using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(VRButton))]
public class ButtonEditor : Editor {

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Press"))
        {
            ((VRButton)target).PressButton();
        }
    }
}
