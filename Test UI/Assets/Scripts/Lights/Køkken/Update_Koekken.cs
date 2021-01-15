using System.Collections;
using System.Collections.Generic;
using BestHTTP;
using System;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Update_Koekken : MonoBehaviour {
    private float update;
    private int nextUpdate = 1;
    public TextMeshProUGUI Units_On;
    private int units_on_val = 0;

    public Slider lights_spisebord;
    public Toggle lights_spisebord_binary;
    public TextMeshProUGUI lights_spisebord_watt;
    public TextMeshProUGUI lights_spisebord_kwh;

    public Slider lights_indirekte;
    public Toggle lights_indirekte_binary;
    public TextMeshProUGUI lights_indirekte_watt;
    public TextMeshProUGUI lights_indirekte_kwh;

    public Slider lights_spots;
    public Toggle lights_spots_binary;
    public TextMeshProUGUI lights_spots_watt;
    public TextMeshProUGUI lights_spots_kwh;
    
    // Use this for initialization
    void Start () {
		
	}
	
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

    void check_units_on()
    {
        units_on_val = 0;
        if (lights_spisebord_binary.isOn == true)
        {
            units_on_val = units_on_val + 1;
        }
        if (lights_indirekte_binary.isOn == true)
        {
            units_on_val = units_on_val + 1;
        }
        if (lights_spots_binary.isOn == true)
        {
            units_on_val = units_on_val + 1;
        }
        if(units_on_val >= 1)
        {
            Units_On.text = units_on_val + " Enheder er tændt";
            //Debug.Log("Units on: " + units_on_val);
        }
        else
        {
            Units_On.text = "Alle Enheder er slukket";
            //Debug.Log("No units on!");
        }   

    }

    void UpdateEverySecond()
    {

        check_units_on();
        //Spisebord
        HTTPRequest lights_spisebord_request = new HTTPRequest(new Uri("http://10.0.10.50:8080/rest/items/zwave_device_87dc29b6_node20_switch_dimmer/state"), HTTPMethods.Get, lights_spisebord_dimmer_OnRequestFinished);
        lights_spisebord_request.Send();

        HTTPRequest lights_spisebord_watt_request = new HTTPRequest(new Uri("http://10.0.10.50:8080/rest/items/zwave_device_87dc29b6_node20_meter_watts/state"), HTTPMethods.Get, lights_spisebord_watt_OnRequestFinished);
        lights_spisebord_watt_request.Send();

        HTTPRequest lights_spisebord_kwh_request = new HTTPRequest(new Uri("http://10.0.10.50:8080/rest/items/zwave_device_87dc29b6_node20_meter_kwh/state"), HTTPMethods.Get, lights_spisebord_kwh_OnRequestFinished);
        lights_spisebord_kwh_request.Send();

        //Endevæg
        HTTPRequest lights_indirekte_request = new HTTPRequest(new Uri("http://10.0.10.50:8080/rest/items/zwave_device_87dc29b6_node19_switch_dimmer/state"), HTTPMethods.Get, lights_indirekte_dimmer_OnRequestFinished);
        lights_indirekte_request.Send();

        HTTPRequest lights_indirekte_watt_request = new HTTPRequest(new Uri("http://10.0.10.50:8080/rest/items/zwave_device_87dc29b6_node19_meter_watts/state"), HTTPMethods.Get, lights_indirekte_watt_OnRequestFinished);
        lights_indirekte_watt_request.Send();

        HTTPRequest lights_indirekte_kwh_request = new HTTPRequest(new Uri("http://10.0.10.50:8080/rest/items/zwave_device_87dc29b6_node19_meter_kwh/state"), HTTPMethods.Get, lights_indirekte_kwh_OnRequestFinished);
        lights_indirekte_kwh_request.Send();

        //spots
        HTTPRequest lights_spots_request = new HTTPRequest(new Uri("http://10.0.10.50:8080/rest/items/zwave_device_87dc29b6_node18_switch_dimmer/state"), HTTPMethods.Get, lights_spots_dimmer_OnRequestFinished);
        lights_spots_request.Send();

        HTTPRequest lights_spots_watt_request = new HTTPRequest(new Uri("http://10.0.10.50:8080/rest/items/zwave_device_87dc29b6_node18_meter_watts/state"), HTTPMethods.Get, lights_spots_watt_OnRequestFinished);
        lights_spots_watt_request.Send();

        HTTPRequest lights_spots_kwh_request = new HTTPRequest(new Uri("http://10.0.10.50:8080/rest/items/zwave_device_87dc29b6_node18_meter_kwh/state"), HTTPMethods.Get, lights_spots_kwh_OnRequestFinished);
        lights_spots_kwh_request.Send();


    }


    void lights_spisebord_dimmer_OnRequestFinished(HTTPRequest lights_spisebord_request, HTTPResponse response)
    {
        lights_spisebord.value = response.DataAsText != "NULL" ? float.Parse(response.DataAsText) : 0;

        if (response.DataAsText == "0")
        {
            lights_spisebord_binary.isOn = false;

        }
        else
        {
            lights_spisebord_binary.isOn = true;

        }

    }

    void lights_spisebord_watt_OnRequestFinished(HTTPRequest lights_spisebord_watt_request, HTTPResponse response)
    {
        //lights_spisebord_watt.text = "NUVÆRENDE: " + response.DataAsText + " Watt";
        lights_spisebord_watt.text = "NUVÆRENDE: " + response.DataAsText + " Watt";
    }

    void lights_spisebord_kwh_OnRequestFinished(HTTPRequest lights_spisebord_kwh_request, HTTPResponse response)
    {
        lights_spisebord_kwh.text = "AKKUMULERET: " + response.DataAsText + " kWh";
        
    }

    void lights_indirekte_dimmer_OnRequestFinished(HTTPRequest lights_indirekte_request, HTTPResponse response)
    {

        lights_indirekte.value = response.DataAsText != "NULL" ? float.Parse(response.DataAsText) : 0;
        //Debug.Log("indirekte" + response.DataAsText);

        if (response.DataAsText == "0")
        {
            lights_indirekte_binary.isOn = false;

        }
        else
        {
            lights_indirekte_binary.isOn = true;

        }

    }

    void lights_indirekte_watt_OnRequestFinished(HTTPRequest lights_indirekte_watt_request, HTTPResponse response)
    {
        lights_indirekte_watt.text = "NUVÆRENDE: " + response.DataAsText + " Watt";
    }

    void lights_indirekte_kwh_OnRequestFinished(HTTPRequest lights_indirekte_kwh_request, HTTPResponse response)
    {
        lights_indirekte_kwh.text = "AKKUMULERET: " + response.DataAsText + " kWh";
    }

    void lights_spots_dimmer_OnRequestFinished(HTTPRequest lights_spots_request, HTTPResponse response)
    {

        lights_spots.value = response.DataAsText != "NULL" ? float.Parse(response.DataAsText) : 0;
        //Debug.Log("spots" + response.DataAsText);

        if (response.DataAsText == "0")
        {
            lights_spots_binary.isOn = false;

        }
        else
        {
            lights_spots_binary.isOn = true;

        }

    }

    void lights_spots_watt_OnRequestFinished(HTTPRequest lights_spots_watt_request, HTTPResponse response)
    {
        lights_spots_watt.text = "NUVÆRENDE: " + response.DataAsText + " Watt";
    }

    void lights_spots_kwh_OnRequestFinished(HTTPRequest lights_spots_kwh_request, HTTPResponse response)
    {
        lights_spots_kwh.text = "AKKUMULERET: " + response.DataAsText + " kWh";
        //update = 0.0f;
    }

}
