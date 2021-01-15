using System.Collections;
using System.Collections.Generic;
using BestHTTP;
using System;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Update_Stuen : MonoBehaviour {
    private float update;
    private int nextUpdate = 1;
    public TextMeshProUGUI Units_On;
    private int units_on_val = 0;

    public Slider lights_spisebord;
    public Toggle lights_spisebord_binary;
    public TextMeshProUGUI lights_spisebord_watt;
    public TextMeshProUGUI lights_spisebord_kwh;

    public Slider lights_endevaeg;
    public Toggle lights_endevaeg_binary;
    public TextMeshProUGUI lights_endevaeg_watt;
    public TextMeshProUGUI lights_endevaeg_kwh;

    public Slider lights_indgang;
    public Toggle lights_indgang_binary;
    public TextMeshProUGUI lights_indgang_watt;
    public TextMeshProUGUI lights_indgang_kwh;
    
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
        if (lights_endevaeg_binary.isOn == true)
        {
            units_on_val = units_on_val + 1;
        }
        if (lights_indgang_binary.isOn == true)
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
        HTTPRequest lights_spisebord_request = new HTTPRequest(new Uri("http://10.0.10.50:8080/rest/items/zwave_device_512_node20_switch_dimmer/state"), HTTPMethods.Get, lights_spisebord_dimmer_OnRequestFinished);
        lights_spisebord_request.Send();

        HTTPRequest lights_spisebord_watt_request = new HTTPRequest(new Uri("http://10.0.10.50:8080/rest/items/zwave_device_512_node20_meter_watts/state"), HTTPMethods.Get, lights_spisebord_watt_OnRequestFinished);
        lights_spisebord_watt_request.Send();

        HTTPRequest lights_spisebord_kwh_request = new HTTPRequest(new Uri("http://10.0.10.50:8080/rest/items/zwave_device_512_node20_meter_kwh/state"), HTTPMethods.Get, lights_spisebord_kwh_OnRequestFinished);
        lights_spisebord_kwh_request.Send();

        //Endevæg
        HTTPRequest lights_endevaeg_request = new HTTPRequest(new Uri("http://10.0.10.50:8080/rest/items/zwave_device_512_node21_switch_dimmer/state"), HTTPMethods.Get, lights_endevaeg_dimmer_OnRequestFinished);
        lights_endevaeg_request.Send();

        HTTPRequest lights_endevaeg_watt_request = new HTTPRequest(new Uri("http://10.0.10.50:8080/rest/items/zwave_device_512_node21_meter_watts/state"), HTTPMethods.Get, lights_endevaeg_watt_OnRequestFinished);
        lights_endevaeg_watt_request.Send();

        HTTPRequest lights_endevaeg_kwh_request = new HTTPRequest(new Uri("http://10.0.10.50:8080/rest/items/zwave_device_512_node21_meter_kwh/state"), HTTPMethods.Get, lights_endevaeg_kwh_OnRequestFinished);
        lights_endevaeg_kwh_request.Send();

        //Indgang
        HTTPRequest lights_indgang_request = new HTTPRequest(new Uri("http://10.0.10.50:8080/rest/items/zwave_device_512_node22_switch_dimmer/state"), HTTPMethods.Get, lights_indgang_dimmer_OnRequestFinished);
        lights_indgang_request.Send();

        HTTPRequest lights_indgang_watt_request = new HTTPRequest(new Uri("http://10.0.10.50:8080/rest/items/zwave_device_512_node22_meter_watts/state"), HTTPMethods.Get, lights_indgang_watt_OnRequestFinished);
        lights_indgang_watt_request.Send();

        HTTPRequest lights_indgang_kwh_request = new HTTPRequest(new Uri("http://10.0.10.50:8080/rest/items/zwave_device_512_node22_meter_kwh/state"), HTTPMethods.Get, lights_indgang_kwh_OnRequestFinished);
        lights_indgang_kwh_request.Send();


    }


    void lights_spisebord_dimmer_OnRequestFinished(HTTPRequest lights_spisebord_request, HTTPResponse response)
    {

        lights_spisebord.value = response.DataAsText != "NULL" ? float.Parse(response.DataAsText) : 0;
        //Debug.Log("Spisebord" + response.DataAsText);

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
        lights_spisebord_watt.text = "NUVÆRENDE: " + response.DataAsText + " Watt";
    }

    void lights_spisebord_kwh_OnRequestFinished(HTTPRequest lights_spisebord_kwh_request, HTTPResponse response)
    {
        lights_spisebord_kwh.text = "AKKUMULERET: " + response.DataAsText + " kWh";
    }

    void lights_endevaeg_dimmer_OnRequestFinished(HTTPRequest lights_endevaeg_request, HTTPResponse response)
    {

        lights_endevaeg.value = response.DataAsText != "NULL" ? float.Parse(response.DataAsText) : 0;
        //Debug.Log("endevaeg" + response.DataAsText);

        if (response.DataAsText == "0")
        {
            lights_endevaeg_binary.isOn = false;

        }
        else
        {
            lights_endevaeg_binary.isOn = true;

        }

    }

    void lights_endevaeg_watt_OnRequestFinished(HTTPRequest lights_endevaeg_watt_request, HTTPResponse response)
    {
        lights_endevaeg_watt.text = "NUVÆRENDE: " + response.DataAsText + " Watt";
    }

    void lights_endevaeg_kwh_OnRequestFinished(HTTPRequest lights_endevaeg_kwh_request, HTTPResponse response)
    {
        lights_endevaeg_kwh.text = "AKKUMULERET: " + response.DataAsText + " kWh";
    }

    void lights_indgang_dimmer_OnRequestFinished(HTTPRequest lights_indgang_request, HTTPResponse response)
    {

        lights_indgang.value = response.DataAsText != "NULL" ? float.Parse(response.DataAsText) : 0;
        //Debug.Log("indgang" + response.DataAsText);

        if (response.DataAsText == "0")
        {
            lights_indgang_binary.isOn = false;

        }
        else
        {
            lights_indgang_binary.isOn = true;

        }

    }

    void lights_indgang_watt_OnRequestFinished(HTTPRequest lights_indgang_watt_request, HTTPResponse response)
    {
        lights_indgang_watt.text = "NUVÆRENDE: " + response.DataAsText + " Watt";
    }

    void lights_indgang_kwh_OnRequestFinished(HTTPRequest lights_indgang_kwh_request, HTTPResponse response)
    {
        lights_indgang_kwh.text = "AKKUMULERET: " + response.DataAsText + " kWh";
        //update = 0.0f;
    }

}
