using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Lighter))]
public class LighterEditor : Editor {

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Lighter lighter = (Lighter)target;
        if(GUILayout.Button("Initiate Fire"))
        {
            if (lighter.GetComponentInChildren<Fire>().lit)
            {
                lighter.StopFire();
            }
            else
            {
                lighter.StartFire();
            }
        }
    }
}
