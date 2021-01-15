using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Ur_Script : MonoBehaviour {

    //public TMPro clockText;
    public Text clockText;
    public bool showSeconds;
    private int seconds;
    private int minutes;
    private DateTime time;
    // Use this for initialization
    void Start()
    {
        seconds = -1;
        minutes = -1;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        time = DateTime.Now;
        if (showSeconds)
        {
            if (seconds != time.Second)
            {
                UpdateText();
                seconds = time.Second;
            }
        }
        else
        {
            if (minutes != time.Minute)
            {
                UpdateText();
                minutes = time.Minute;
            }
        }
    }
    void UpdateText()
    {
        clockText.text = time.Hour.ToString("D2") + ":" + time.Minute.ToString("D2");
        if (showSeconds)
        {
            clockText.text += ":" + time.Second.ToString("D2");
        }
    }
}


