using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadGame : MonoBehaviour
{
    ControllerManager controllerManager;
    public float increaseSpeed = 5f;
    public Image radialLoadingBar;

    private AsyncOperation asyncSceneLoad;

    private float FillPercentage
    {
        get
        {
            return radialLoadingBar.fillAmount;
        }
        set
        {
            radialLoadingBar.fillAmount = value;
            radialLoadingBar.fillAmount = Mathf.Clamp01(radialLoadingBar.fillAmount);
            if (radialLoadingBar.fillAmount >= 1)
            {
                LoadNextScene();
            }
        }
    }

    // Use this for initialization
    void Start()
    {
        controllerManager = GetComponent<ControllerManager>();
        StartCoroutine(loadHouseScene());
    }

    // Update is called once per frame
    void Update()
    {
        if (controllerManager.ButtonHeldDownBothController(Valve.VR.EVRButtonId.k_EButton_Grip))
        {
            Debug.Log("true");
            FillPercentage += increaseSpeed * Time.deltaTime;
        }
        else
        {
            Debug.Log("False");
            FillPercentage -= increaseSpeed * Time.deltaTime;
        }
    }
    [ContextMenu("nextScene")]
    public void LoadNextScene()
    {
        asyncSceneLoad.allowSceneActivation = true;
        if (asyncSceneLoad != null && asyncSceneLoad.isDone)
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(1));
        }
    }

    IEnumerator loadHouseScene()
    {
        Debug.LogWarning("ASYNC LOAD STARTED - " +
           "DO NOT EXIT PLAY MODE UNTIL SCENE LOADS... UNITY WILL CRASH");
        asyncSceneLoad = SceneManager.LoadSceneAsync(1);
        asyncSceneLoad.allowSceneActivation = false;
        yield return asyncSceneLoad;
    }
}
