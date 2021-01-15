using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using BestHTTP;
using UnityEngine;

public class POST_Openhab : MonoBehaviour {

    public void Set_Light_On()
    {
        HTTPRequest request = new HTTPRequest(new Uri("http://10.0.10.50:8080/rest/items/zwave_device_512_node42_switch_dimmer"), HTTPMethods.Post, OnRequestFinished);
        request.AddHeader("Content-Type", "text/plain");
        request.AddHeader("Accept", "application/json");
        request.RawData = Encoding.UTF8.GetBytes("ON");
        request.Send();
    }

    public void Set_Light_Off()
    {
        HTTPRequest request = new HTTPRequest(new Uri("http://10.0.10.50:8080/rest/items/zwave_device_512_node42_switch_dimmer"), HTTPMethods.Post, OnRequestFinished);
        request.AddHeader("Content-Type", "text/plain");
        request.AddHeader("Accept", "application/json");
        request.RawData = Encoding.UTF8.GetBytes("OFF");
        request.Send();
    }



    void OnRequestFinished(HTTPRequest request, HTTPResponse response)
    {

        Debug.Log("Request Finished! Text received: " + response.DataAsText);
    }
}
