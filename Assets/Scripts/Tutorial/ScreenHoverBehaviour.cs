using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenHoverBehaviour : MonoBehaviour
{
    [Header("Hover Effect")]
    public Transform[] wheels;
    public float spinSpeed = 2f;
    public float hoverStrength = 2f;
    public float hoverSpeed = 2f;
    public float targetAltitude = 20f;
    public AnimationCurve flyAwaySpeedCurve;
    private bool despawning = false;


    private void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!despawning)
        {
            if (wheels.Length > 0)
            {
                foreach (var item in wheels)
                {
                    item.GetChild(0).Rotate(Vector3.forward, spinSpeed * Time.deltaTime * 100f);
                    item.position += (Vector3.up * Mathf.Sin(Time.time * hoverSpeed) * hoverStrength / 2f);
                }
                foreach (var item in wheels)
                {
                    item.LookAt(item.position + Vector3.up);
                }
            }
            transform.position += (Vector3.up * Mathf.Sin(Time.time * hoverSpeed) * hoverStrength);
        }
    }

    [ContextMenu("flyAndDestroy")]
    public void FlyAwayAndDestroy()
    {
        if (!despawning)
        {
            StartCoroutine(Despawn());
        }
    }

    private IEnumerator Despawn()
    {
        despawning = true;
        float startAltitude = transform.position.y;
        float calculatedTargetAltitude = transform.position.y + targetAltitude;


        LookAtPlayer lookat = transform.root.GetComponent<LookAtPlayer>();
        if (lookat)
        {
            lookat.enabled = false;
        }




        while (transform.position.y < calculatedTargetAltitude)
        {
            float percentageToTargetAltitude = 1f / targetAltitude * (transform.position.y - startAltitude);
            Vector3 vectorToAdd = Vector3.up * hoverSpeed * Time.deltaTime * 10f * flyAwaySpeedCurve.Evaluate(percentageToTargetAltitude);
            if (wheels.Length > 0)
            {
                foreach (var item in wheels)
                {
                    item.GetChild(0).Rotate(Vector3.forward, (spinSpeed + (spinSpeed * flyAwaySpeedCurve.Evaluate(percentageToTargetAltitude))) * Time.deltaTime * 100f);
                    item.position += vectorToAdd;
                }
            }
            transform.position += vectorToAdd;

            yield return 0;
        }
        if (wheels.Length > 0)
        {
            foreach (var item in wheels)
            {
                Destroy(item.gameObject);
            }
        }
        Destroy(gameObject);

    }
}
