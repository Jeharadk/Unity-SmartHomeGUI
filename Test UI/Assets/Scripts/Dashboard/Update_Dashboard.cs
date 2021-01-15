using System.Collections;
using System.Collections.Generic;
using BestHTTP;
using System;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Update_Dashboard : MonoBehaviour {

    private int nextUpdate = 1;
    private float temptemp;
    public TextMeshProUGUI Temperature_Stuen_Sensor;
    public TextMeshProUGUI Temperature_Stuen_Setpoint;
    public TextMeshProUGUI Temperature_Stuen_Valve;


    // Update is called once per frame
    void Update () {
        if (Time.time >= nextUpdate)
        {
            // Debug.Log(Time.time + ">=" + nextUpdate);
            // Change the next update (current second+1)
            nextUpdate = Mathf.FloorToInt(Time.time) + 2;
            // Call your fonction
            UpdateEverySecond();
        }
    }
    void UpdateEverySecond()
    {

       
        //Temperatur
        HTTPRequest temperature_spisebord_sensor_request = new HTTPRequest(new Uri("http://10.0.10.50:8080/rest/items/mqtt_topic_57504eab_AirthingsStuenTemperature/state"), HTTPMethods.Get, temperature_spisebord_sensor_OnRequestFinished);
        temperature_spisebord_sensor_request.Send();

        HTTPRequest temperature_spisebord_setpoint_request = new HTTPRequest(new Uri("http://10.0.10.50:8080/rest/items/Livingroom_TargetTemp/state"), HTTPMethods.Get, temperature_spisebord_setpoint_OnRequestFinished);
        temperature_spisebord_setpoint_request.Send();


    }


    void temperature_spisebord_sensor_OnRequestFinished(HTTPRequest temperature_spisebord_sensor_request, HTTPResponse response)
    {
        temptemp = float.Parse(response.DataAsText);
        Temperature_Stuen_Sensor.text = temptemp.ToString("F0") + " °C";
    }

    void temperature_spisebord_setpoint_OnRequestFinished(HTTPRequest temperature_spisebord_setpoint_request, HTTPResponse response)
    {
        temptemp = float.Parse(response.DataAsText);
        Temperature_Stuen_Setpoint.text = "SÆTPUNKT: " + temptemp.ToString("F0") + " °C";
    }

    void temperature_spisebord_valve_OnRequestFinished(HTTPRequest temperature_spisebord_valve_request, HTTPResponse response)
    {
        temptemp = float.Parse(response.DataAsText);
        Temperature_Stuen_Valve.text = "VENTILÅBNING: " + temptemp.ToString("F0") + " %";
    }

}
