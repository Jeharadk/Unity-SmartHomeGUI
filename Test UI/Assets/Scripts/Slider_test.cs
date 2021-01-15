using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Text;
using BestHTTP;
using UnityEngine;
using System;

public class Slider_test : MonoBehaviour {
    public string value2;
    public Slider main_slider;
    // Use this for initialization
        void Start () {
            main_slider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        }
	
        void ValueChangeCheck()
        {
            value2 = main_slider.value.ToString();
            // Debug.Log(main_slider.value);
        }

        public void send_value()
        {
            HTTPRequest request = new HTTPRequest(new Uri("http://10.0.10.50:8080/rest/items/zwave_device_512_node42_switch_dimmer"), HTTPMethods.Post);
            request.AddHeader("Content-Type", "text/plain");
            request.AddHeader("Accept", "application/json");
            request.RawData = Encoding.UTF8.GetBytes(value2);
            request.Send();

        }


}
