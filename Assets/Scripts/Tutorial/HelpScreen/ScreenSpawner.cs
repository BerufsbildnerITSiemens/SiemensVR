using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSpawner : MonoBehaviour
{
    public GameObject screenPrefab;
    public GameObject explosionPrefab;
    public Vector3 spawnOffset;
    private TutorialScreen screen;
    private GameObject explosion;
    private bool spawned = false;


    [ContextMenu("Spawn")]
    private IEnumerator Spawn()
    {
        spawned = true;
        

        GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(explosion, 1);

        screen = Instantiate(screenPrefab).GetComponent<TutorialScreen>();
        screen.transform.position = transform.position + spawnOffset;

        Vector3 rot = GameObject.FindObjectOfType<SteamVR_Camera>().transform.rotation.eulerAngles;

        rot.x = 0;
        rot.z = 0;
        rot.y -= 90;

        screen.transform.rotation = Quaternion.Euler(rot);
        yield return new WaitForSeconds(2);
        

        
    }

    private void OnCollisionEnter(Collision collision)
    {if (!spawned)
        {
            StartCoroutine(Spawn());
            Destroy(gameObject);
        }
    }
}
