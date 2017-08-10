using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoScroll : MonoBehaviour
{
    private ScrollRect scrollRect;

    public float speed = 3f;

    // Use this for initialization
    void Start()
    {
        scrollRect = GetComponent<ScrollRect>();
    }

    // Update is called once per frame
    void Update()
    {
        scrollRect.verticalNormalizedPosition -= speed / 10f * Time.deltaTime;
        if (scrollRect.verticalNormalizedPosition <= 0)
        {
            scrollRect.verticalNormalizedPosition = 1;
        }
    }
}
