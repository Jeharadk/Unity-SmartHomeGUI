using System.Collections;
using System.Collections.Generic;
using BestHTTP;
using System;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.Text;

public class Send_Koekken : MonoBehaviour {

    public TextMeshProUGUI lights_spisebord_toggle_text;
    public Toggle lights_spisebord_toggle;
    public Slider lights_spisebord_slider;
    private string lights_spisebord_slider_val;

    public TextMeshProUGUI lights_indirekte_toggle_text;
    public Toggle lights_indirekte_toggle;
    public Slider lights_indirekte_slider;
    private string lights_indirekte_slider_val;
    public TextMeshProUGUI lights_spots_toggle_text;
    public Toggle lights_spots_toggle;
    public Slider lights_spots_slider;
    private string lights_spots_slider_val;

    void Start()
    {
        lights_spisebord_slider.onValueChanged.AddListener(delegate { ValueChanged_spisebord_slider(); });
        lights_indirekte_slider.onValueChanged.AddListener(delegate { ValueChanged_indirekte_slider(); });
        lights_spots_slider.onValueChanged.AddListener(delegate { ValueChanged_spots_slider(); });
    }

    public void ValueChanged_spisebord(Toggle t)
    {
        //TextMeshProUGUI textmeshPro = GetComponentInChildren<TextMeshProUGUI>();
        if (t.isOn)
        {
            //textmeshPro.SetText("ON");
            lights_spisebord_toggle_text.text = "ON";
            HTTPRequest request = new HTTPRequest(new Uri("http://10.0.10.50:8080/rest/items/zwave_device_512_node20_switch_dimmer"), HTTPMethods.Post);
            request.AddHeader("Content-Type", "text/plain");
            request.AddHeader("Accept", "application/json");
            request.RawData = Encoding.UTF8.GetBytes("ON");
            request.Send();
        }
        else
        {
            lights_spisebord_toggle_text.text = "OFF";
            HTTPRequest request = new HTTPRequest(new Uri("http://10.0.10.50:8080/rest/items/zwave_device_512_node20_switch_dimmer"), HTTPMethods.Post);
            request.AddHeader("Content-Type", "text/plain");
            request.AddHeader("Accept", "application/json");
            request.RawData = Encoding.UTF8.GetBytes("OFF");
            request.Send();
        }
    }

    public void ValueChanged_spisebord_slider()
    {
        lights_spisebord_slider_val = lights_spisebord_slider.value.ToString();
    }

    public void Send_spisebord_Slider_Value()
    {
        HTTPRequest request = new HTTPRequest(new Uri("http://10.0.10.50:8080/rest/items/zwave_device_512_node20_switch_dimmer"), HTTPMethods.Post);
        request.AddHeader("Content-Type", "text/plain");
        request.AddHeader("Accept", "application/json");
        request.RawData = Encoding.UTF8.GetBytes(lights_spisebord_slider_val);
        request.Send();
    }

    public void ValueChanged_indirekte(Toggle t)
    {
        //TextMeshProUGUI textmeshPro = GetComponentInChildren<TextMeshProUGUI>();
        if (t.isOn)
        {
            //textmeshPro.SetText("ON");
            lights_indirekte_toggle_text.text = "ON";
            HTTPRequest request = new HTTPRequest(new Uri("http://10.0.10.50:8080/rest/items/zwave_device_512_node19_switch_dimmer"), HTTPMethods.Post);
            request.AddHeader("Content-Type", "text/plain");
            request.AddHeader("Accept", "application/json");
            request.RawData = Encoding.UTF8.GetBytes("ON");
            request.Send();
        }
        else
        {
            lights_indirekte_toggle_text.text = "OFF";
            HTTPRequest request = new HTTPRequest(new Uri("http://10.0.10.50:8080/rest/items/zwave_device_512_node19_switch_dimmer"), HTTPMethods.Post);
            request.AddHeader("Content-Type", "text/plain");
            request.AddHeader("Accept", "application/json");
            request.RawData = Encoding.UTF8.GetBytes("OFF");
            request.Send();
        }
    }

    public void ValueChanged_indirekte_slider()
    {
        lights_indirekte_slider_val = lights_indirekte_slider.value.ToString();
    }

    public void Send_indirekte_Slider_Value()
    {
        HTTPRequest request = new HTTPRequest(new Uri("http://10.0.10.50:8080/rest/items/zwave_device_512_node19_switch_dimmer"), HTTPMethods.Post);
        request.AddHeader("Content-Type", "text/plain");
        request.AddHeader("Accept", "application/json");
        request.RawData = Encoding.UTF8.GetBytes(lights_indirekte_slider_val);
        request.Send();
    }

    public void ValueChanged_spots(Toggle t)
    {
        //TextMeshProUGUI textmeshPro = GetComponentInChildren<TextMeshProUGUI>();
        if (t.isOn)
        {
            //textmeshPro.SetText("ON");
            lights_spots_toggle_text.text = "ON";
            HTTPRequest request = new HTTPRequest(new Uri("http://10.0.10.50:8080/rest/items/zwave_device_512_node18_switch_dimmer"), HTTPMethods.Post);
            request.AddHeader("Content-Type", "text/plain");
            request.AddHeader("Accept", "application/json");
            request.RawData = Encoding.UTF8.GetBytes("ON");
            request.Send();
        }
        else
        {
            lights_spots_toggle_text.text = "OFF";
            HTTPRequest request = new HTTPRequest(new Uri("http://10.0.10.50:8080/rest/items/zwave_device_512_node18_switch_dimmer"), HTTPMethods.Post);
            request.AddHeader("Content-Type", "text/plain");
            request.AddHeader("Accept", "application/json");
            request.RawData = Encoding.UTF8.GetBytes("OFF");
            request.Send();
        }
    }

    public void ValueChanged_spots_slider()
    {
        lights_spots_slider_val = lights_spots_slider.value.ToString();
    }

    public void Send_spots_Slider_Value()
    {
        HTTPRequest request = new HTTPRequest(new Uri("http://10.0.10.50:8080/rest/items/zwave_device_512_node18_switch_dimmer"), HTTPMethods.Post);
        request.AddHeader("Content-Type", "text/plain");
        request.AddHeader("Accept", "application/json");
        request.RawData = Encoding.UTF8.GetBytes(lights_spots_slider_val);
        request.Send();
    }


}
