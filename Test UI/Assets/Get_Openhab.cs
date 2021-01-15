using BestHTTP;
using System;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Get_Openhab : MonoBehaviour {
    public Slider mSlider;
	public void Get_Value()
    {
        HTTPRequest request = new HTTPRequest(new Uri("http://10.0.10.50:8080/rest/items/zwave_device_512_node42_switch_dimmer/state"), HTTPMethods.Get, OnRequestFinished);
        request.Send();
    }

    void OnRequestFinished(HTTPRequest request, HTTPResponse response)
    {
        mSlider.value = float.Parse(response.DataAsText);
        Debug.Log("Request Finished! Text received: " + response.DataAsText); 
    }
}
