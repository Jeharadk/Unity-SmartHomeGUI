using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class Ur_Script1 : MonoBehaviour {

    
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
        TextMeshProUGUI textmeshPro = GetComponent<TextMeshProUGUI>();

        textmeshPro.SetText (time.Hour.ToString("D2") + ":" + time.Minute.ToString("D2") + "    " + time.Day.ToString("D1") +" " + time.Date.ToString("MMM") + " " + time.Date.ToString("yyyy"));
        if (showSeconds)
        {
            textmeshPro.SetText (time.Hour.ToString("D2") + ":" + time.Minute.ToString("D2") + ":" + time.Second.ToString("D2"));
        }
    }
}


