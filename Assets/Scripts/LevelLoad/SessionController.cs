using UnityEngine;
using System;
using System.IO;
using System.Xml.Serialization;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SessionController : MonoBehaviour
{
    public string path;
    private SessionSettings sessionSettings;
    public Text timeLeft;
    private DateTime startTime;
    private DateTime endTime;

    // Use this for initialization
    void Start()
    {
        LoadSessionLength();
        startTime = DateTime.Now;
        endTime = startTime.AddMinutes(sessionSettings.minutesPerSession);

        InvokeRepeating("UpdateLabel", 0, 1);
    }

    private void UpdateLabel()
    {
        TimeSpan leftTime = (endTime - DateTime.Now);
        timeLeft.text = leftTime.Minutes.ToString("00") + " : " + leftTime.Seconds.ToString("00");

        if (leftTime.TotalSeconds <= 0)
        {
            CancelInvoke();
            EndSession();
        }
    }

    private void EndSession()
    {
        SceneManager.LoadScene(2);
    }

    #region saveLoadEtc
    private void saveTestFile()
    {
        SessionSettings settings = new SessionSettings();
        settings.minutesPerSession = 10;

        using (FileStream fs = new FileStream(path, FileMode.CreateNew))
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(SessionSettings));
            xmlSerializer.Serialize(fs, settings);
        }
    }

    private void LoadSessionLength()
    {
        SessionSettings settings;
        try
        {
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(SessionSettings));
                settings = (SessionSettings)xmlSerializer.Deserialize(fs);
            }
        }
        catch (Exception)
        {
            throw;
        }
        if (settings != null)
        {
            sessionSettings = settings;
        }
    }
    #endregion
}
