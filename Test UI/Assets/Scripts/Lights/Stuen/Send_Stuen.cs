using System.Collections;
using System.Collections.Generic;
using BestHTTP;
using System;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System.Text;

public class Send_Stuen : MonoBehaviour {

    public TextMeshProUGUI lights_spisebord_toggle_text;
    public Toggle lights_spisebord_toggle;
    public Slider lights_spisebord_slider;
    private string lights_spisebord_slider_val;

    public TextMeshProUGUI lights_endevaeg_toggle_text;
    public Toggle lights_endevaeg_toggle;
    public Slider lights_endevaeg_slider;
    private string lights_endevaeg_slider_val;
    public TextMeshProUGUI lights_indgang_toggle_text;
    public Toggle lights_indgang_toggle;
    public Slider lights_indgang_slider;
    private string lights_indgang_slider_val;

    void Start()
    {
        lights_spisebord_slider.onValueChanged.AddListener(delegate { ValueChanged_spisebord_slider(); });
        lights_endevaeg_slider.onValueChanged.AddListener(delegate { ValueChanged_endevaeg_slider(); });
        lights_indgang_slider.onValueChanged.AddListener(delegate { ValueChanged_indgang_slider(); });
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

    public void ValueChanged_endevaeg(Toggle t)
    {
        //TextMeshProUGUI textmeshPro = GetComponentInChildren<TextMeshProUGUI>();
        if (t.isOn)
        {
            //textmeshPro.SetText("ON");
            lights_endevaeg_toggle_text.text = "ON";
            HTTPRequest request = new HTTPRequest(new Uri("http://10.0.10.50:8080/rest/items/zwave_device_512_node21_switch_dimmer"), HTTPMethods.Post);
            request.AddHeader("Content-Type", "text/plain");
            request.AddHeader("Accept", "application/json");
            request.RawData = Encoding.UTF8.GetBytes("ON");
            request.Send();
        }
        else
        {
            lights_endevaeg_toggle_text.text = "OFF";
            HTTPRequest request = new HTTPRequest(new Uri("http://10.0.10.50:8080/rest/items/zwave_device_512_node21_switch_dimmer"), HTTPMethods.Post);
            request.AddHeader("Content-Type", "text/plain");
            request.AddHeader("Accept", "application/json");
            request.RawData = Encoding.UTF8.GetBytes("OFF");
            request.Send();
        }
    }

    public void ValueChanged_endevaeg_slider()
    {
        lights_endevaeg_slider_val = lights_endevaeg_slider.value.ToString();
    }

    public void Send_endevaeg_Slider_Value()
    {
        HTTPRequest request = new HTTPRequest(new Uri("http://10.0.10.50:8080/rest/items/zwave_device_512_node21_switch_dimmer"), HTTPMethods.Post);
        request.AddHeader("Content-Type", "text/plain");
        request.AddHeader("Accept", "application/json");
        request.RawData = Encoding.UTF8.GetBytes(lights_endevaeg_slider_val);
        request.Send();
    }

    public void ValueChanged_indgang(Toggle t)
    {
        //TextMeshProUGUI textmeshPro = GetComponentInChildren<TextMeshProUGUI>();
        if (t.isOn)
        {
            //textmeshPro.SetText("ON");
            lights_indgang_toggle_text.text = "ON";
            HTTPRequest request = new HTTPRequest(new Uri("http://10.0.10.50:8080/rest/items/zwave_device_512_node22_switch_dimmer"), HTTPMethods.Post);
            request.AddHeader("Content-Type", "text/plain");
            request.AddHeader("Accept", "application/json");
            request.RawData = Encoding.UTF8.GetBytes("ON");
            request.Send();
        }
        else
        {
            lights_indgang_toggle_text.text = "OFF";
            HTTPRequest request = new HTTPRequest(new Uri("http://10.0.10.50:8080/rest/items/zwave_device_512_node22_switch_dimmer"), HTTPMethods.Post);
            request.AddHeader("Content-Type", "text/plain");
            request.AddHeader("Accept", "application/json");
            request.RawData = Encoding.UTF8.GetBytes("OFF");
            request.Send();
        }
    }

    public void ValueChanged_indgang_slider()
    {
        lights_indgang_slider_val = lights_indgang_slider.value.ToString();
    }

    public void Send_indgang_Slider_Value()
    {
        HTTPRequest request = new HTTPRequest(new Uri("http://10.0.10.50:8080/rest/items/zwave_device_512_node22_switch_dimmer"), HTTPMethods.Post);
        request.AddHeader("Content-Type", "text/plain");
        request.AddHeader("Accept", "application/json");
        request.RawData = Encoding.UTF8.GetBytes(lights_indgang_slider_val);
        request.Send();
    }


}
