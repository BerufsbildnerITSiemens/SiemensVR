using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VRTopic : MonoBehaviour
{
    [HideInInspector]
    public TopicInfo info;
    private Image background;
    public Text Title;
    public RawImage Icon;

    private Color defaultColor;
    public Color highlightColor;

    private void Awake()
    {
        background = GetComponent<Image>();
        defaultColor = background.color;
    }
    private void Start()
    {
        Title.text = info.Title;
        Icon.texture = info.Icon;
    }

    public void SetHighlighted()
    {
        background.color = highlightColor;
    }
    public void SetNormal()
    {
        background.color = defaultColor;
    }
}
