using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text timerText;
    void Start()
    {
        timerText = GetComponentInChildren<Text>();
    }

    void Update()
    {

        float t = Time.timeSinceLevelLoad;
        float milliseconds = (Mathf.Floor(t * 100) % 100);
        int seconds = (int)(t % 60);
        t /= 60;
        int minutes = (int)(t % 60);
        timerText.text = string.Format("{0}:{1}.{2}", minutes.ToString("00"), seconds.ToString("00"), milliseconds.ToString("00"));
    }
}
