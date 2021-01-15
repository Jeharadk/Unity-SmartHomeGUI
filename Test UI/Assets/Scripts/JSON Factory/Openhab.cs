using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BestHTTP;
using System;
using Newtonsoft.Json;
using TMPro;
using System.Text;

public class Openhab : MonoBehaviour {
    private int nextUpdate = 1;
    public List<OpenhabItems> OpenhabItemsList;
    public OpenhabItems Items;
    private bool drag_active = false;

    //Navn i Openhab Lysdæmpere + Watt + KwH
    //Køkken
    private readonly string Lys_Koekken_Spisebord_dimmer = "zwave_device_87dc29b6_node20_switch_dimmer";
    private readonly string Lys_Koekken_Spisebord_watt = "zwave_device_87dc29b6_node20_meter_watts";
    private readonly string Lys_Koekken_Spisebord_kwh = "zwave_device_87dc29b6_node20_meter_kwh";

    private readonly string Lys_Koekken_Indirekte_dimmer = "zwave_device_87dc29b6_node19_switch_dimmer";
    private readonly string Lys_Koekken_Indirekte_watt = "zwave_device_87dc29b6_node19_meter_watts";
    private readonly string Lys_Koekken_Indirekte_kwh = "zwave_device_87dc29b6_node19_meter_kwh";

    private readonly string Lys_Koekken_Spots_dimmer = "zwave_device_87dc29b6_node18_switch_dimmer";
    private readonly string Lys_Koekken_Spots_watt = "zwave_device_87dc29b6_node18_meter_watts";
    private readonly string Lys_Koekken_Spots_kwh = "zwave_device_87dc29b6_node18_meter_kwh";

    //Stue
    private readonly string Lys_Stue_Spisebord_dimmer = "zwave_device_87dc29b6_node20_switch_dimmer";
    private readonly string Lys_Stue_Spisebord_watt = "zwave_device_87dc29b6_node20_meter_watts";
    private readonly string Lys_Stue_Spisebord_kwh = "zwave_device_87dc29b6_node20_meter_kwh";

    private readonly string Lys_Stue_Endevaeg_dimmer = "zwave_device_87dc29b6_node21_switch_dimmer";
    private readonly string Lys_Stue_Endevaeg_watt = "zwave_device_87dc29b6_node21_meter_watts";
    private readonly string Lys_Stue_Endevaeg_kwh = "zwave_device_87dc29b6_node21_meter_kwh";

    private readonly string Lys_Stue_Indgang_dimmer = "zwave_device_87dc29b6_node22_switch_dimmer";
    private readonly string Lys_Stue_Indgang_watt = "zwave_device_87dc29b6_node22_meter_watts";
    private readonly string Lys_Stue_Indgang_kwh = "zwave_device_87dc29b6_node22_meter_kwh";



    //Objekter til manipulering i Unity
    //Køkken
    private int Lights_Koekken_Units_On_Val = 0;
    public TextMeshProUGUI Lights_Koekken_Units_On;

    public Slider Lights_Koekken_Spisebord_dimmer;
    private string Lights_Koekken_Spisebord_dimmer_val;
    private int Lights_Koekken_Spisebord_dimmer_off_counter = 0;
    public Toggle Lights_Koekken_Spisebord_binary;
    public TextMeshProUGUI Lights_Koekken_Spisebord_binary_label;
    public TextMeshProUGUI Lights_Koekken_Spisebord_watt;
    public TextMeshProUGUI Lights_Koekken_Spisebord_kwh;

    public Slider Lights_Koekken_Indirekte_dimmer;
    private string Lights_Koekken_Indirekte_dimmer_val;
    private int Lights_Koekken_Indirekte_dimmer_off_counter = 0;
    public Toggle Lights_Koekken_Indirekte_binary;
    public TextMeshProUGUI Lights_Koekken_Indirekte_binary_label;
    public TextMeshProUGUI Lights_Koekken_Indirekte_watt;
    public TextMeshProUGUI Lights_Koekken_Indirekte_kwh;

    public Slider Lights_Koekken_Spots_dimmer;
    private string Lights_Koekken_Spots_dimmer_val;
    private int Lights_Koekken_Spots_dimmer_off_counter = 0;
    public Toggle Lights_Koekken_Spots_binary;
    public TextMeshProUGUI Lights_Koekken_Spots_binary_label;
    public TextMeshProUGUI Lights_Koekken_Spots_watt;
    public TextMeshProUGUI Lights_Koekken_Spots_kwh;

    //Stue
    private int Lights_Stue_Units_On_Val = 0;
    public TextMeshProUGUI Lights_Stue_Units_On;

    public Slider Lights_Stue_Spisebord_dimmer;
    private string Lights_Stue_Spisebord_dimmer_val;
    public Toggle Lights_Stue_Spisebord_binary;
    public TextMeshProUGUI Lights_Stue_Spisebord_binary_label;
    public TextMeshProUGUI Lights_Stue_Spisebord_watt;
    public TextMeshProUGUI Lights_Stue_Spisebord_kwh;

    public Slider Lights_Stue_Endevaeg_dimmer;
    private string Lights_Stue_Endevaeg_dimmer_val;
    public Toggle Lights_Stue_Endevaeg_binary;
    public TextMeshProUGUI Lights_Stue_Endevaeg_binary_label;
    public TextMeshProUGUI Lights_Stue_Endevaeg_watt;
    public TextMeshProUGUI Lights_Stue_Endevaeg_kwh;

    public Slider Lights_Stue_Indgang_dimmer;
    private string Lights_Stue_Indgang_dimmer_val;
    public Toggle Lights_Stue_Indgang_binary;
    public TextMeshProUGUI Lights_Stue_Indgang_binary_label;
    public TextMeshProUGUI Lights_Stue_Indgang_watt;
    public TextMeshProUGUI Lights_Stue_Indgang_kwh;



    public class StateDescription
    {
        public string pattern { get; set; }
        public bool readOnly { get; set; }
        public List<object> options { get; set; }
        public int? minimum { get; set; }
        public int? maximum { get; set; }
        public int? step { get; set; }
    }

    public class OpenhabItems
    {
        public string link { get; set; }
        public object state { get; set; }
        public StateDescription stateDescription { get; set; }
        public bool editable { get; set; }
        public string type { get; set; }
        public string name { get; set; }
        public string label { get; set; }
        public List<object> tags { get; set; }
        public List<object> groupNames { get; set; }
        public string category { get; set; }
        public List<object> members { get; set; }
    }


    // Use this for initialization
    void Start() {
        Lights_Koekken_Spisebord_dimmer.onValueChanged.AddListener(delegate { ValueChanged_Lights_Koekken_Spisebord_dimmer(); });
        Lights_Koekken_Indirekte_dimmer.onValueChanged.AddListener(delegate { ValueChanged_Lights_Koekken_Indirekte_dimmer(); });
        Lights_Koekken_Spots_dimmer.onValueChanged.AddListener(delegate { ValueChanged_Lights_Koekken_Spots_dimmer(); });

        Lights_Stue_Spisebord_dimmer.onValueChanged.AddListener(delegate { ValueChanged_Lights_Stue_Spisebord_dimmer(); });
        Lights_Stue_Endevaeg_dimmer.onValueChanged.AddListener(delegate { ValueChanged_Lights_Stue_Endevaeg_dimmer(); });
        Lights_Stue_Indgang_dimmer.onValueChanged.AddListener(delegate { ValueChanged_Lights_Stue_Indgang_dimmer(); });

    }

    // Update is called once per frame
    void Update() {
        if (Time.time >= nextUpdate)
        {
            // Debug.Log(Time.time + ">=" + nextUpdate);
            // Change the next update (current second+1)
            nextUpdate = Mathf.FloorToInt(Time.time) + 2;
            // Call your fonction
            UpdateEverySecond();
        }
    }
    public void Active_Drag()
    {
        drag_active = true;

    }


    //Køkken 
    void ValueChanged_Lights_Koekken_Spisebord_dimmer()
    {
        Lights_Koekken_Spisebord_dimmer_val = Lights_Koekken_Spisebord_dimmer.value.ToString();
    }
    public void ValueChange_Lights_Koekken_Spisebord_toggle()
    {
        if (Lights_Koekken_Spisebord_binary.isOn)
        {
            //textmeshPro.SetText("ON");
            drag_active = true;
            Lights_Koekken_Spisebord_binary_label.text = "ON";
            HTTPRequest request = new HTTPRequest(new Uri("http://10.0.10.50:8080/rest/items/" + Lys_Koekken_Spisebord_dimmer), HTTPMethods.Post);
            request.AddHeader("Content-Type", "text/plain");
            request.AddHeader("Accept", "application/json");
            request.RawData = Encoding.UTF8.GetBytes("ON");
            request.Send();
            drag_active = false;
        }
        else
        {
            drag_active = true;
            Lights_Koekken_Spisebord_binary_label.text = "OFF";
            HTTPRequest request = new HTTPRequest(new Uri("http://10.0.10.50:8080/rest/items/" + Lys_Koekken_Spisebord_dimmer), HTTPMethods.Post);
            request.AddHeader("Content-Type", "text/plain");
            request.AddHeader("Accept", "application/json");
            request.RawData = Encoding.UTF8.GetBytes("OFF");
            request.Send();
            drag_active = false;
        }
    }
    public void Lights_Koekken_Spisebord_Dimmer_Send()
    {
        HTTPRequest request = new HTTPRequest(new Uri("http://10.0.10.50:8080/rest/items/" + Lys_Koekken_Spisebord_dimmer), HTTPMethods.Post);
        request.AddHeader("Content-Type", "text/plain");
        request.AddHeader("Accept", "application/json");
        request.RawData = Encoding.UTF8.GetBytes(Lights_Koekken_Spisebord_dimmer_val);
        request.Send();
        drag_active = false;
    }

    void ValueChanged_Lights_Koekken_Indirekte_dimmer()
    {
        Lights_Koekken_Indirekte_dimmer_val = Lights_Koekken_Indirekte_dimmer.value.ToString();
    }
    public void ValueChange_Lights_Koekken_Indirekte_toggle()
    {
        if (Lights_Koekken_Indirekte_binary.isOn)
        {
            //textmeshPro.SetText("ON");
            drag_active = true;
            Lights_Koekken_Indirekte_binary_label.text = "ON";
            HTTPRequest request = new HTTPRequest(new Uri("http://10.0.10.50:8080/rest/items/" + Lys_Koekken_Indirekte_dimmer), HTTPMethods.Post);
            request.AddHeader("Content-Type", "text/plain");
            request.AddHeader("Accept", "application/json");
            request.RawData = Encoding.UTF8.GetBytes("ON");
            request.Send();
            drag_active = false;
        }
        else
        {
            drag_active = true;
            Lights_Koekken_Indirekte_binary_label.text = "OFF";
            HTTPRequest request = new HTTPRequest(new Uri("http://10.0.10.50:8080/rest/items/" + Lys_Koekken_Indirekte_dimmer), HTTPMethods.Post);
            request.AddHeader("Content-Type", "text/plain");
            request.AddHeader("Accept", "application/json");
            request.RawData = Encoding.UTF8.GetBytes("OFF");
            request.Send();
            drag_active = false;
        }
    }
    public void Lights_Koekken_Indirekte_Dimmer_Send()
    {
        HTTPRequest request = new HTTPRequest(new Uri("http://10.0.10.50:8080/rest/items/" + Lys_Koekken_Indirekte_dimmer), HTTPMethods.Post);
        request.AddHeader("Content-Type", "text/plain");
        request.AddHeader("Accept", "application/json");
        request.RawData = Encoding.UTF8.GetBytes(Lights_Koekken_Indirekte_dimmer_val);
        request.Send();
        drag_active = false;
    }

    void ValueChanged_Lights_Koekken_Spots_dimmer()
    {
        Lights_Koekken_Spots_dimmer_val = Lights_Koekken_Spots_dimmer.value.ToString();
    }
    public void ValueChange_Lights_Koekken_Spots_toggle()
    {
        if (Lights_Koekken_Spots_binary.isOn)
        {
            //textmeshPro.SetText("ON");
            drag_active = true;
            Lights_Koekken_Spots_binary_label.text = "ON";
            HTTPRequest request = new HTTPRequest(new Uri("http://10.0.10.50:8080/rest/items/" + Lys_Koekken_Spots_dimmer), HTTPMethods.Post);
            request.AddHeader("Content-Type", "text/plain");
            request.AddHeader("Accept", "application/json");
            request.RawData = Encoding.UTF8.GetBytes("ON");
            request.Send();
            drag_active = false;
        }
        else
        {
            drag_active = true;
            Lights_Koekken_Spots_binary_label.text = "OFF";
            HTTPRequest request = new HTTPRequest(new Uri("http://10.0.10.50:8080/rest/items/" + Lys_Koekken_Spots_dimmer), HTTPMethods.Post);
            request.AddHeader("Content-Type", "text/plain");
            request.AddHeader("Accept", "application/json");
            request.RawData = Encoding.UTF8.GetBytes("OFF");
            request.Send();
            drag_active = false;
        }
    }
    public void Lights_Koekken_Spots_Dimmer_Send()
    {
        HTTPRequest request = new HTTPRequest(new Uri("http://10.0.10.50:8080/rest/items/" + Lys_Koekken_Spots_dimmer), HTTPMethods.Post);
        request.AddHeader("Content-Type", "text/plain");
        request.AddHeader("Accept", "application/json");
        request.RawData = Encoding.UTF8.GetBytes(Lights_Koekken_Spots_dimmer_val);
        request.Send();
        drag_active = false;
    }

    //Stue 
    void ValueChanged_Lights_Stue_Spisebord_dimmer()
    {
        Lights_Stue_Spisebord_dimmer_val = Lights_Stue_Spisebord_dimmer.value.ToString();
    }
    public void ValueChange_Lights_Stue_Spisebord_toggle()
    {
        if (Lights_Stue_Spisebord_binary.isOn)
        {
            //textmeshPro.SetText("ON");
            drag_active = true;
            Lights_Stue_Spisebord_binary_label.text = "ON";
            HTTPRequest request = new HTTPRequest(new Uri("http://10.0.10.50:8080/rest/items/" + Lys_Stue_Spisebord_dimmer), HTTPMethods.Post);
            request.AddHeader("Content-Type", "text/plain");
            request.AddHeader("Accept", "application/json");
            request.RawData = Encoding.UTF8.GetBytes("ON");
            request.Send();
            drag_active = false;
        }
        else
        {
            drag_active = true;
            Lights_Stue_Spisebord_binary_label.text = "OFF";
            HTTPRequest request = new HTTPRequest(new Uri("http://10.0.10.50:8080/rest/items/" + Lys_Stue_Spisebord_dimmer), HTTPMethods.Post);
            request.AddHeader("Content-Type", "text/plain");
            request.AddHeader("Accept", "application/json");
            request.RawData = Encoding.UTF8.GetBytes("OFF");
            request.Send();
            drag_active = false;
        }
    }
    public void Lights_Stue_Spisebord_Dimmer_Send()
    {
        HTTPRequest request = new HTTPRequest(new Uri("http://10.0.10.50:8080/rest/items/" + Lys_Stue_Spisebord_dimmer), HTTPMethods.Post);
        request.AddHeader("Content-Type", "text/plain");
        request.AddHeader("Accept", "application/json");
        request.RawData = Encoding.UTF8.GetBytes(Lights_Stue_Spisebord_dimmer_val);
        request.Send();
        drag_active = false;
    }

    void ValueChanged_Lights_Stue_Endevaeg_dimmer()
    {
        Lights_Stue_Endevaeg_dimmer_val = Lights_Stue_Endevaeg_dimmer.value.ToString();
    }
    public void ValueChange_Lights_Stue_Endevaeg_toggle()
    {
        if (Lights_Stue_Endevaeg_binary.isOn)
        {
            //textmeshPro.SetText("ON");
            drag_active = true;
            Lights_Stue_Endevaeg_binary_label.text = "ON";
            HTTPRequest request = new HTTPRequest(new Uri("http://10.0.10.50:8080/rest/items/" + Lys_Stue_Endevaeg_dimmer), HTTPMethods.Post);
            request.AddHeader("Content-Type", "text/plain");
            request.AddHeader("Accept", "application/json");
            request.RawData = Encoding.UTF8.GetBytes("ON");
            request.Send();
            drag_active = false;
        }
        else
        {
            drag_active = true;
            Lights_Stue_Endevaeg_binary_label.text = "OFF";
            HTTPRequest request = new HTTPRequest(new Uri("http://10.0.10.50:8080/rest/items/" + Lys_Stue_Endevaeg_dimmer), HTTPMethods.Post);
            request.AddHeader("Content-Type", "text/plain");
            request.AddHeader("Accept", "application/json");
            request.RawData = Encoding.UTF8.GetBytes("OFF");
            request.Send();
            drag_active = false;
        }
    }
    public void Lights_Stue_Endevaeg_Dimmer_Send()
    {
        HTTPRequest request = new HTTPRequest(new Uri("http://10.0.10.50:8080/rest/items/" + Lys_Stue_Endevaeg_dimmer), HTTPMethods.Post);
        request.AddHeader("Content-Type", "text/plain");
        request.AddHeader("Accept", "application/json");
        request.RawData = Encoding.UTF8.GetBytes(Lights_Stue_Endevaeg_dimmer_val);
        request.Send();
        drag_active = false;
    }

    void ValueChanged_Lights_Stue_Indgang_dimmer()
    {
        Lights_Stue_Indgang_dimmer_val = Lights_Stue_Indgang_dimmer.value.ToString();
    }
    public void ValueChange_Lights_Stue_Indgang_toggle()
    {
        if (Lights_Stue_Indgang_binary.isOn)
        {
            //textmeshPro.SetText("ON");
            drag_active = true;
            Lights_Stue_Indgang_binary_label.text = "ON";
            HTTPRequest request = new HTTPRequest(new Uri("http://10.0.10.50:8080/rest/items/" + Lys_Stue_Indgang_dimmer), HTTPMethods.Post);
            request.AddHeader("Content-Type", "text/plain");
            request.AddHeader("Accept", "application/json");
            request.RawData = Encoding.UTF8.GetBytes("ON");
            request.Send();
            drag_active = false;
        }
        else
        {
            drag_active = true;
            Lights_Stue_Indgang_binary_label.text = "OFF";
            HTTPRequest request = new HTTPRequest(new Uri("http://10.0.10.50:8080/rest/items/" + Lys_Stue_Indgang_dimmer), HTTPMethods.Post);
            request.AddHeader("Content-Type", "text/plain");
            request.AddHeader("Accept", "application/json");
            request.RawData = Encoding.UTF8.GetBytes("OFF");
            request.Send();
            drag_active = false;
        }
    }
    public void Lights_Stue_Indgang_Dimmer_Send()
    {
        HTTPRequest request = new HTTPRequest(new Uri("http://10.0.10.50:8080/rest/items/" + Lys_Stue_Indgang_dimmer), HTTPMethods.Post);
        request.AddHeader("Content-Type", "text/plain");
        request.AddHeader("Accept", "application/json");
        request.RawData = Encoding.UTF8.GetBytes(Lights_Stue_Indgang_dimmer_val);
        request.Send();
        drag_active = false;
    }





    void UpdateEverySecond()
    {
        //Hent alle things fra Openhab

        HTTPRequest OpenhabItems_request = new HTTPRequest(new Uri("http://10.0.10.50:8080/rest/items/"), HTTPMethods.Get, OpenhabItems_OnRequestFinished);
        OpenhabItems_request.Send();

    }




    void OpenhabItems_OnRequestFinished(HTTPRequest OpenhabItems_request, HTTPResponse response)
    {

        //OpenhabItemsList = JsonConvert.DeserializeObject<OpenhabItemsList>(response.DataAsText);
        //Items = JsonConvert.DeserializeObject<List<OpenhabItemsList>(response.DataAsText);
        OpenhabItemsList = JsonConvert.DeserializeObject<List<OpenhabItems>>(response.DataAsText);
        if(drag_active!=true)
        {
            UpdateItems_Koekken();
            UpdateItems_Stue();
        }
        



    }
    void UpdateItems_Koekken()
    {
        //Køkken
        //Sætter Items til Spisebord
        Items = OpenhabItemsList.Find((x) => x.name == Lys_Koekken_Spisebord_dimmer);
        Lights_Koekken_Spisebord_dimmer.value = Items.state.ToString() != "NULL" ? float.Parse(Items.state.ToString()) : 0;

        if (Items.state.ToString() == "0")
        {
            Lights_Koekken_Spisebord_binary.isOn = false;
        }

        else
        {
            Lights_Koekken_Spisebord_binary.isOn = true;
        }

        Items = OpenhabItemsList.Find((x) => x.name == Lys_Koekken_Spisebord_watt);
        Lights_Koekken_Spisebord_watt.text = "NUVÆRENDE: " + Items.state.ToString() + " Watt";

        Items = OpenhabItemsList.Find((x) => x.name == Lys_Koekken_Spisebord_kwh);
        Lights_Koekken_Spisebord_kwh.text = "AKKUMULERET: " + Items.state.ToString() + " kWh";

        //Sætter Items til Indirekte
        Items = OpenhabItemsList.Find((x) => x.name == Lys_Koekken_Indirekte_dimmer);
        Lights_Koekken_Indirekte_dimmer.value = Items.state.ToString() != "NULL" ? float.Parse(Items.state.ToString()) : 0;

        if (Items.state.ToString() == "0")
        {
            Lights_Koekken_Indirekte_binary.isOn = false;
        }
        else
        {
            Lights_Koekken_Indirekte_binary.isOn = true;
        }

        Items = OpenhabItemsList.Find((x) => x.name == Lys_Koekken_Indirekte_watt);
        Lights_Koekken_Indirekte_watt.text = "NUVÆRENDE: " + Items.state.ToString() + " Watt";

        Items = OpenhabItemsList.Find((x) => x.name == Lys_Koekken_Indirekte_kwh);
        Lights_Koekken_Indirekte_kwh.text = "AKKUMULERET: " + Items.state.ToString() + " kWh";

        //Sætter Items til Spots
        Items = OpenhabItemsList.Find((x) => x.name == Lys_Koekken_Spots_dimmer);
        Lights_Koekken_Spots_dimmer.value = Items.state.ToString() != "NULL" ? float.Parse(Items.state.ToString()) : 0;
        if (Items.state.ToString() == "0")
        {
            Lights_Koekken_Spots_binary.isOn = false;
            Lights_Koekken_Spots_dimmer_off_counter = 0;
        }
        else
        {
            if (Lights_Koekken_Spots_watt.text == "NUVÆRENDE: 0 Watt")
            {
                Lights_Koekken_Spots_dimmer_off_counter++;
                if(Lights_Koekken_Spots_dimmer_off_counter > 5)
                {
                    Lights_Koekken_Spots_binary.isOn = false;
                    Lights_Koekken_Spots_dimmer_off_counter = 0;
                }
                    
            }
            else
            {
                Lights_Koekken_Spots_binary.isOn = true;
                Lights_Koekken_Spots_dimmer_off_counter = 0;
            }
            
        }

        Items = OpenhabItemsList.Find((x) => x.name == Lys_Koekken_Spots_watt);
        Lights_Koekken_Spots_watt.text = "NUVÆRENDE: " + Items.state.ToString() + " Watt";

        Items = OpenhabItemsList.Find((x) => x.name == Lys_Koekken_Spots_kwh);
        Lights_Koekken_Spots_kwh.text = "AKKUMULERET: " + Items.state.ToString() + " kWh";

        //Tjekker hvor mange lys er tændt
            Lights_Koekken_Units_On_Val = 0;
            if (Lights_Koekken_Spisebord_binary.isOn == true)
            {
                Lights_Koekken_Units_On_Val = Lights_Koekken_Units_On_Val + 1;
            }
            if (Lights_Koekken_Indirekte_binary.isOn == true)
            {
                Lights_Koekken_Units_On_Val = Lights_Koekken_Units_On_Val + 1;
            }
            if (Lights_Koekken_Spots_binary.isOn == true)
            {
                Lights_Koekken_Units_On_Val = Lights_Koekken_Units_On_Val + 1;
            }
            if (Lights_Koekken_Units_On_Val >= 1)
            {
                Lights_Koekken_Units_On.text = Lights_Koekken_Units_On_Val + " Enheder er tændt";
                //Debug.Log("Units on: " + Lights_Koekken_Units_On_Val);
            }
            else
            {
                Lights_Koekken_Units_On.text = "Alle Enheder er slukket";
                //Debug.Log("No units on!");
            }



    }

    void UpdateItems_Stue()
    {
        //Køkken
        //Sætter Items til Spisebord
        Items = OpenhabItemsList.Find((x) => x.name == Lys_Stue_Spisebord_dimmer);
        Lights_Stue_Spisebord_dimmer.value = Items.state.ToString() != "NULL" ? float.Parse(Items.state.ToString()) : 0;
        if (Items.state.ToString() == "0")
        {
            Lights_Stue_Spisebord_binary.isOn = false;
        }
        else
        {
            Lights_Stue_Spisebord_binary.isOn = true;
        }

        Items = OpenhabItemsList.Find((x) => x.name == Lys_Stue_Spisebord_watt);
        Lights_Stue_Spisebord_watt.text = "NUVÆRENDE: " + Items.state.ToString() + " Watt";

        Items = OpenhabItemsList.Find((x) => x.name == Lys_Stue_Spisebord_kwh);
        Lights_Stue_Spisebord_kwh.text = "AKKUMULERET: " + Items.state.ToString() + " kWh";

        //Sætter Items til Endevaeg
        Items = OpenhabItemsList.Find((x) => x.name == Lys_Stue_Endevaeg_dimmer);
        Lights_Stue_Endevaeg_dimmer.value = Items.state.ToString() != "NULL" ? float.Parse(Items.state.ToString()) : 0;
        if (Items.state.ToString() == "0")
        {
            Lights_Stue_Endevaeg_binary.isOn = false;
        }
        else
        {
            Lights_Stue_Endevaeg_binary.isOn = true;
        }

        Items = OpenhabItemsList.Find((x) => x.name == Lys_Stue_Endevaeg_watt);
        Lights_Stue_Endevaeg_watt.text = "NUVÆRENDE: " + Items.state.ToString() + " Watt";

        Items = OpenhabItemsList.Find((x) => x.name == Lys_Stue_Endevaeg_kwh);
        Lights_Stue_Endevaeg_kwh.text = "AKKUMULERET: " + Items.state.ToString() + " kWh";

        //Sætter Items til Indgang

        Items = OpenhabItemsList.Find((x) => x.name == Lys_Stue_Indgang_dimmer);
        Lights_Stue_Indgang_dimmer.value = Items.state.ToString() != "NULL" ? float.Parse(Items.state.ToString()) : 0;
        if (Items.state.ToString() == "0")
        {
            Lights_Stue_Indgang_binary.isOn = false;
        }
        else
        {
            Lights_Stue_Indgang_binary.isOn = true;
        }

        Items = OpenhabItemsList.Find((x) => x.name == Lys_Stue_Indgang_watt);
        Lights_Stue_Indgang_watt.text = "NUVÆRENDE: " + Items.state.ToString() + " Watt";

        Items = OpenhabItemsList.Find((x) => x.name == Lys_Stue_Indgang_kwh);
        Lights_Stue_Indgang_kwh.text = "AKKUMULERET: " + Items.state.ToString() + " kWh";

        //Tjekker hvor mange lys er tændt
        Lights_Stue_Units_On_Val = 0;
        if (Lights_Stue_Spisebord_binary.isOn == true)
        {
            Lights_Stue_Units_On_Val = Lights_Stue_Units_On_Val + 1;
        }
        if (Lights_Stue_Endevaeg_binary.isOn == true)
        {
            Lights_Stue_Units_On_Val = Lights_Stue_Units_On_Val + 1;
        }
        if (Lights_Stue_Indgang_binary.isOn == true)
        {
            Lights_Stue_Units_On_Val = Lights_Stue_Units_On_Val + 1;
        }
        if (Lights_Stue_Units_On_Val >= 1)
        {
            Lights_Stue_Units_On.text = Lights_Stue_Units_On_Val + " Enheder er tændt";
            //Debug.Log("Units on: " + Lights_Stue_Units_On_Val);
        }
        else
        {
            Lights_Stue_Units_On.text = "Alle Enheder er slukket";
            //Debug.Log("No units on!");
        }



    }
}
