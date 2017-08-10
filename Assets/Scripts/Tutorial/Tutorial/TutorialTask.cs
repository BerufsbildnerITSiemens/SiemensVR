using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TutorialTask
{
    public string title;
    [TextArea(3,10)]
    public string description;
    public Texture image;
    public int eventIndex;
}
