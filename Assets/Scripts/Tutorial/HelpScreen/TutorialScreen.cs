using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialScreen : MonoBehaviour
{
    #region Tutorial
    [Header("Topics")]
    public TopicInfo[] allTopics;
    private TopicInfo activeTopic;

    private List<VRTopic> allVRTopics;

    private VRTopic _selectedTopic;
    public VRTopic selectedTopic
    {
        get
        {
            return _selectedTopic;
        }
        set
        {
            _selectedTopic = value;
            UpdateTopics();
        }
    }

    public RectTransform contentHolder;
    public GameObject TopicPrefab;
    public GameObject UiTrackedPrefab;

    public ScrollRect scrollRect;
    public float scrollSpeed = 1f;


    [Header("Tutorial")]

    public RawImage desciptionImage;
    public Text descriptionText;


    [Header("Spawning/Despawning")]
    public float targetFactor;
    public float growSpeed = 2f;
    private float smallSize;
    private SteamVR_Camera player;
    private bool isDespawning = false;
    public float maxDistanceToPlayer = 5f;
    #endregion
    // Use this for initialization
    void Start()
    {

        InteractWithHelpScreen.interactingHelpScreen = this;
        //instantiate topics
        allVRTopics = new List<VRTopic>();
        foreach (var item in allTopics)
        {
            GameObject topic = Instantiate(TopicPrefab, contentHolder);
            VRTopic thisTopic = topic.GetComponent<VRTopic>();
            thisTopic.info = item;

            GameObject uiTracked = Instantiate(UiTrackedPrefab, transform);
            uiTracked.GetComponent<UITracked>().trackedUIObject = topic.GetComponent<RectTransform>();

            allVRTopics.Add(thisTopic);
        }

        selectedTopic = allVRTopics[0];



        //stuff
        player = GameObject.FindObjectOfType<SteamVR_Camera>();
        smallSize = transform.localScale.x;

        //if no space -> despawn

        for (int i = 0; i < transform.childCount; i++)
        {
            VRSensor sensor = transform.GetChild(i).GetComponent<VRSensor>();
            if (sensor != null)
            {
                sensor.collisionEnter += NotEnoughSpace;
            }

        }

        //start grow
        StartCoroutine(GrowScreen());
    }

    // Update is called once per frame
    void Update()
    {


        //Debug.Log(Vector3.Distance(player.transform.position, transform.position));
        if (!isDespawning && Vector3.Distance(player.transform.position, transform.position) > maxDistanceToPlayer)
        {
            StartCoroutine(Despawn());
        }
    }

    void UpdateTopics()
    {
        foreach (var item in allVRTopics)
        {
            if (item != selectedTopic)
            {
                item.SetNormal();
            }
            else
            {
                item.SetHighlighted();
            }

        }
        descriptionText.text = selectedTopic.info.Text;
        desciptionImage.texture = selectedTopic.info.Image;
    }

    private IEnumerator GrowScreen()
    {
        while (transform.localScale.x < smallSize * targetFactor)
        {
            transform.localScale += Vector3.one * growSpeed * Time.deltaTime * 0.1f;
            yield return 0;
        }
        //set content holder size
        RectTransform rectTransform = TopicPrefab.GetComponent<RectTransform>();
        contentHolder.sizeDelta = new Vector2(0f, rectTransform.sizeDelta.y * (contentHolder.childCount - 4)); //why -4? dont know.
        //contentHolder.offsetMax = Vector2.zero;
        //contentHolder.offsetMin = Vector2.zero;

    }
    private IEnumerator Despawn()
    {
        isDespawning = true;
        InteractWithHelpScreen.interactingHelpScreen = null;


        while (transform.localScale.x > smallSize)
        {
            transform.localScale -= Vector3.one * growSpeed * Time.deltaTime * 0.1f;
            yield return 0;
        }
        Destroy(gameObject);
    }

    public void Scroll(int dir)
    {
        Vector3 pos = contentHolder.transform.position;
        pos.y += Mathf.Clamp(dir, -1, 1) * scrollSpeed * Time.deltaTime;
        contentHolder.transform.position = pos;
    }

    public Transform getUpperBorder()
    {
        return transform.Find("Screen").Find("UpperTopicBorder");
    }
    public Transform getLowerBorder()
    {
        return transform.Find("Screen").Find("LowerTopicBorder");
    }
    
    public void NotEnoughSpace(Collision go, VRSensor sensor)
    {
        if (go.collider.GetComponent<UITracked>())
        {
            return;
        }
        StopAllCoroutines();
        StartCoroutine(Despawn());
    }
}
