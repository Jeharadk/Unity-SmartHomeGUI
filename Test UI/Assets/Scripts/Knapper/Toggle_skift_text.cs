using TMPro;
using UnityEngine.UI;
using System.Text;
using BestHTTP;
using UnityEngine;
using System;

public class Toggle_skift_text : MonoBehaviour {


    public void ValueChanged(Toggle t)
    {
        TextMeshProUGUI textmeshPro = GetComponentInChildren<TextMeshProUGUI>();
        if (t.isOn)
        {
            textmeshPro.SetText ("ON");
            HTTPRequest request = new HTTPRequest(new Uri("http://10.0.10.50:8080/rest/items/zwave_device_512_node20_switch_dimmer"), HTTPMethods.Post);
            request.AddHeader("Content-Type", "text/plain");
            request.AddHeader("Accept", "application/json");
            request.RawData = Encoding.UTF8.GetBytes("ON");
            request.Send();
        }
        else
        {
            textmeshPro.SetText("OFF");
            HTTPRequest request = new HTTPRequest(new Uri("http://10.0.10.50:8080/rest/items/zwave_device_512_node20_switch_dimmer"), HTTPMethods.Post);
            request.AddHeader("Content-Type", "text/plain");
            request.AddHeader("Accept", "application/json");
            request.RawData = Encoding.UTF8.GetBytes("OFF");
            request.Send();
        }
    }


}
