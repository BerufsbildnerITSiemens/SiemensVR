using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    private Transform player;
    // Use this for initialization
    void Start()
    {
        player = GameObject.FindObjectOfType<SteamVR_Camera>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(player);
    }
}
