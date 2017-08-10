using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FireExtinguisher : MonoBehaviour
{
    public ParticleSystem water;
    private List<Fire> firesToExtinguish;
    public int firesNeededToActivate;
    public float durationInSeconds;
    public bool activated = true;
    public float durationInSecondsBetweenFires;
    private bool isExtinguishing = false;


    // Use this for initialization
    void Start()
    {
        firesToExtinguish = new List<Fire>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetActivated(bool pactivated)
    {
        activated = pactivated;
        checkForFireAndExtinguishThem();
    }

    private void StartExtinguishFire()
    {
        water.Play();
    }

    private void StopExtinguishFire()
    {
        water.Stop();
    }

    private void OnTriggerEnter(Collider other)
    {
        Fire fire = other.gameObject.GetComponent<Fire>();
        if (fire && !fire.GetComponentInParent<Lighter>())
        {
            firesToExtinguish.Add(fire);
            if (!isExtinguishing)
                checkForFireAndExtinguishThem();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Fire fire = other.gameObject.GetComponent<Fire>();
        if (fire)
        {
            foreach (Fire item in firesToExtinguish)
            {
                if (ReferenceEquals(fire, item))
                {
                    firesToExtinguish.Remove(fire);
                    break;
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Fire fire = other.gameObject.GetComponent<Fire>();
        if (fire)
        {
            if (!isExtinguishing)
                checkForFireAndExtinguishThem();
        }
    }

    private void checkForFireAndExtinguishThem()
    {
        
            if (firesToExtinguish.Count >= firesNeededToActivate && activated)
            {
                isExtinguishing = true;
                StartExtinguishFire();
                StartCoroutine(WaitAndExtinguishFire());

            }
    }

    private IEnumerator WaitAndExtinguishFire()
    {
        List<Fire> firesToExtinguishCopy = new List<Fire>(firesToExtinguish);
        firesToExtinguish.Clear();
        while (firesToExtinguishCopy.Count > 0)
        {
            yield return new WaitForSeconds(durationInSecondsBetweenFires);
            Fire item;
            int randomRange = 0;
            if (firesToExtinguishCopy.Count == 1)
            {
                item = firesToExtinguishCopy[0];

            }
            else
            {
                randomRange = Random.Range(0, firesToExtinguishCopy.Count - 1);
                item = firesToExtinguishCopy[randomRange];
            }

            if (item != null)
                item.Extinguish();
            else
                firesToExtinguishCopy.RemoveAt(randomRange);
        }

        yield return new WaitForSeconds(durationInSeconds);
        StopExtinguishFire();
        isExtinguishing = false;
    }
}
