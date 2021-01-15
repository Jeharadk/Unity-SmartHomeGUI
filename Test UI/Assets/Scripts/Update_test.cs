using System.Collections;
using System.Collections.Generic;
using BestHTTP;
using System;
using UnityEngine.UI;
using UnityEngine;

public class Update_test : MonoBehaviour {
    private float update;
    public Slider mSlider2;
    void Update()
    {
        update += Time.deltaTime;
        if (update > 1.0f)
        {
            
            HTTPRequest request = new HTTPRequest(new Uri("http://10.0.10.50:8080/rest/items/zwave_device_512_node42_switch_dimmer/state"), HTTPMethods.Get, OnRequestFinished);
            request.Send();
            Debug.Log("Update");
        }
    }

    void OnRequestFinished(HTTPRequest request, HTTPResponse response)
    {
        mSlider2.value = float.Parse(response.DataAsText);
        Debug.Log("Request Finished! Text received: " + response.DataAsText);
        update = 0.0f;
    }

}
