using UnityEngine;
using System.Collections;

public class VRTime : MonoBehaviour
{
    public System.Action<float> timeChanged;
    public float secAmountForOneDay = 24;
    public float tickSpeed = 1;
    public Transform sun;

    [HideInInspector]
    public bool isNight;
    private float _Time;
    public float Time
    {
        get
        {
            return _Time;
        }
        set
        {
            if (value > 13)
            {
                _Time = (value % 12) + (value - 12) - 1;
                isNight = !isNight;
            }
            else
            {
                _Time = value;
            }
            if (timeChanged != null)
            {
                timeChanged(_Time);
                UpdateSun();
            }
        }
    }


    public float getTimeAstwentyFourFormat()
    {
        return Time + (isNight ? 12 : 0);
    }

    private void UpdateSun()
    {
        float rotation = Remap(_Time, 1, 12, 0, 180) - 90f;
        if (isNight)
        {
            rotation += 180;
        }
        sun.localRotation = Quaternion.Euler(new Vector3(rotation, 30, sun.localRotation.z));
    }
    private void Start()
    {
        isNight = false;
        Time = 1;
        StartCoroutine(IncreaseTime());
    }

    private IEnumerator IncreaseTime()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(tickSpeed);
            float addAmount = 24 / secAmountForOneDay * tickSpeed;

            Time += addAmount;
        }
    }
    private float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}
