using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BestHTTP;
using System;
using Newtonsoft.Json;
using TMPro;

public class Update_Audio : MonoBehaviour {

    public TextMeshProUGUI Main_Audio_Volume;
    //public TextMeshProUGUI Main_Audio_Input;
    //public TextMeshProUGUI Main_Audio_SoundProgram;
    public TextMeshProUGUI Main_Audio_Artist;
    public TextMeshProUGUI Main_Audio_Album;
    public TextMeshProUGUI Main_Audio_Track;
    public Image Main_Audio_Albumart;
    public string Main_Audio_Albumart_url;
    private bool Main_Audio_Albumart_changed;
    public Toggle Main_Audio_Power;
    private int nextUpdate = 1;
    public Status Main_Status;
    public PlayInfo Main_Playinfo;
    public Image Audio_Stue_TV;
    public Image Audio_Stue_Radio;
    public Image Audio_Stue_Spotify;
    public Image Audio_Stue_Airplay;
    public Image Audio_Stue_Bluetooth;
    public TextMeshProUGUI Audio_Stue_Input_Label;


    public class ActualVolume
    {
        public string mode { get; set; }
        public double value { get; set; }
        public string unit { get; set; }
    }
    public class ToneControl
    {
        public string mode { get; set; }
        public int bass { get; set; }
        public int treble { get; set; }
    }
    public class Status
    {
        public int response_code { get; set; }
        public string power { get; set; }
        public int sleep { get; set; }
        public int volume { get; set; }
        public bool mute { get; set; }
        public int max_volume { get; set; }
        public string input { get; set; }
        public bool distribution_enable { get; set; }
        public string sound_program { get; set; }
        public string surr_decoder_type { get; set; }
        public bool pure_direct { get; set; }
        public bool enhancer { get; set; }
        public ToneControl tone_control { get; set; }
        public int dialogue_level { get; set; }
        public string link_control { get; set; }
        public string link_audio_delay { get; set; }
        public string link_audio_quality { get; set; }
        public int disable_flags { get; set; }
        public ActualVolume actual_volume { get; set; }
        public bool contents_display { get; set; }
        public bool party_enable { get; set; }      
    }

    public class PlayInfo
    {
        public int response_code { get; set; }
        public string input { get; set; }
        public string play_queue_type { get; set; }
        public string playback { get; set; }
        public string repeat { get; set; }
        public string shuffle { get; set; }
        public int play_time { get; set; }
        public int total_time { get; set; }
        public string artist { get; set; }
        public string album { get; set; }
        public string track { get; set; }
        public string albumart_url { get; set; }
        public int albumart_id { get; set; }
        public string usb_devicetype { get; set; }
        public bool auto_stopped { get; set; }
        public int attribute { get; set; }
        public List<object> repeat_available { get; set; }
        public List<object> shuffle_available { get; set; }
    }



    // Use this for initialization
    void Start () {
        HTTPRequest Main_Audio_Status_request = new HTTPRequest(new Uri("http://10.0.10.60/YamahaExtendedControl/v1/main/getStatus"), HTTPMethods.Get, Main_Audio_Status_OnRequestFinished);
        Main_Audio_Status_request.Send();

        //Playinfo på Main
        HTTPRequest Main_Audio_Playinfo_request = new HTTPRequest(new Uri("http://10.0.10.60/YamahaExtendedControl/v1/netusb/getPlayInfo"), HTTPMethods.Get, Main_Audio_Playinfo_OnRequestFinished);
        Main_Audio_Playinfo_request.Send();
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

    void UpdateEverySecond()
    {

        
       
        //Status på Main
        HTTPRequest Main_Audio_Status_request = new HTTPRequest(new Uri("http://10.0.10.60/YamahaExtendedControl/v1/main/getStatus"), HTTPMethods.Get, Main_Audio_Status_OnRequestFinished);
        Main_Audio_Status_request.Send();

        //Playinfo på Main
        HTTPRequest Main_Audio_Playinfo_request = new HTTPRequest(new Uri("http://10.0.10.60/YamahaExtendedControl/v1/netusb/getPlayInfo"), HTTPMethods.Get, Main_Audio_Playinfo_OnRequestFinished);
        Main_Audio_Playinfo_request.Send();
        
        // Hent Albumart
                if (Main_Playinfo.albumart_url != "")
                { 
                    if(Main_Audio_Albumart_changed == true)
                    { 
                    HTTPRequest Main_Audio_Albumart_request = new HTTPRequest(new Uri(Main_Audio_Albumart_url), HTTPMethods.Get, Main_Audio_Albumart_OnRequestFinished);
                    Main_Audio_Albumart_request.Send();
                    }
                }
                else
                {
                    Main_Audio_Albumart.enabled = false;

                }

        // Debug.Log(Main_Playinfo.playback);
        Active_Input();
    }

    public void Volume_Up()
    {
        HTTPRequest request = new HTTPRequest(new Uri("http://10.0.10.60/YamahaExtendedControl/v1/main/setVolume?volume=up&step=4"), HTTPMethods.Get);
        request.AddHeader("Content-Type", "text/plain");
        request.AddHeader("Accept", "application/json");
        //request.RawData = Encoding.UTF8.GetBytes("ON");
        request.Send();
        Main_Status.volume = Main_Status.volume + 4;
        Main_Audio_Volume.text = ((float)Main_Status.volume / (float)161 * (float)100).ToString("F0") + "%";

    }

    public void Volume_Down()
    {
        HTTPRequest request = new HTTPRequest(new Uri("http://10.0.10.60/YamahaExtendedControl/v1/main/setVolume?volume=down&step=4"), HTTPMethods.Get);
        request.AddHeader("Content-Type", "text/plain");
        request.AddHeader("Accept", "application/json");
        //request.RawData = Encoding.UTF8.GetBytes("ON");
        request.Send();
        Main_Status.volume = Main_Status.volume - 4;
        Main_Audio_Volume.text = ((float)Main_Status.volume / (float)161 * (float)100).ToString("F0") + "%";

    }

    void Active_Input()
    {
        if (Main_Status.input == "audio1")
        {
            Audio_Stue_TV.color = new Color32(1, 186, 154, 255);
            Audio_Stue_Spotify.color = new Color32(60, 60, 60, 255);
            Audio_Stue_Radio.color = new Color32(60, 60, 60, 255);
            Audio_Stue_Bluetooth.color = new Color32(60, 60, 60, 255);
            Audio_Stue_Airplay.color = new Color32(60, 60, 60, 255);
            Audio_Stue_Input_Label.text = "TV";
        }

        if (Main_Status.input == "net_radio")
        {
            Audio_Stue_TV.color = new Color32(60, 60, 60, 255);
            Audio_Stue_Spotify.color = new Color32(60, 60, 60, 255);
            Audio_Stue_Radio.color = new Color32(1, 186, 154, 255);
            Audio_Stue_Bluetooth.color = new Color32(60, 60, 60, 255);
            Audio_Stue_Airplay.color = new Color32(60, 60, 60, 255);
            Audio_Stue_Input_Label.text = "Radio";
        }

        if (Main_Status.input == "spotify")
        {
            Audio_Stue_TV.color = new Color32(60, 60, 60, 255);
            Audio_Stue_Spotify.color = new Color32(1, 186, 154, 255);
            Audio_Stue_Radio.color = new Color32(60, 60, 60, 255);
            Audio_Stue_Bluetooth.color = new Color32(60, 60, 60, 255);
            Audio_Stue_Airplay.color = new Color32(60, 60, 60, 255);
            Audio_Stue_Input_Label.text = "Spotify";
        }

        if (Main_Status.input == "airplay")
        {
            Audio_Stue_TV.color = new Color32(60, 60, 60, 255);
            Audio_Stue_Spotify.color = new Color32(60, 60, 60, 255);
            Audio_Stue_Radio.color = new Color32(60, 60, 60, 255);
            Audio_Stue_Bluetooth.color = new Color32(60, 60, 60, 255);
            Audio_Stue_Airplay.color = new Color32(1, 186, 154, 255);
            Audio_Stue_Input_Label.text = "Airplay";
        }

        if (Main_Status.input == "bluetooth")
        {
            Audio_Stue_TV.color = new Color32(60, 60, 60, 255);
            Audio_Stue_Spotify.color = new Color32(60, 60, 60, 255);
            Audio_Stue_Radio.color = new Color32(60, 60, 60, 255);
            Audio_Stue_Bluetooth.color = new Color32(1, 186, 154, 255);
            Audio_Stue_Airplay.color = new Color32(60, 60, 60, 255);
            Audio_Stue_Input_Label.text = "Bluetooth";
        }
    }

    void Main_Audio_Status_OnRequestFinished(HTTPRequest Main_Audio_Status_request, HTTPResponse response)
    {

        Main_Status = JsonConvert.DeserializeObject<Status>(response.DataAsText);
        Main_Audio_Volume.text = ((float)Main_Status.volume / (float)161 * (float)100).ToString("F0") + "%";
        //Main_Audio_Input.text = Main_Status.input;
        //Main_Audio_SoundProgram.text = Main_Status.sound_program;
        if (Main_Status.power == "standby")
        {
            Main_Audio_Power.isOn = false;
        }
        else
        {
            Main_Audio_Power.isOn = true;
        }
    }

    void Main_Audio_Playinfo_OnRequestFinished(HTTPRequest Main_Audio_Playinfo_request, HTTPResponse response)
    {

        Main_Playinfo = JsonConvert.DeserializeObject<PlayInfo>(response.DataAsText);

        if (Main_Playinfo.playback != "play")
        {
            Main_Audio_Album.text = "";
            Main_Audio_Artist.text = "";
            Main_Audio_Track.text = "";
            Main_Audio_Albumart.enabled = false;

        }
        else
        {
            Main_Audio_Album.text = Main_Playinfo.album;
            Main_Audio_Artist.text = Main_Playinfo.artist;
            Main_Audio_Track.text = Main_Playinfo.track;
            Main_Audio_Albumart.enabled = true;
            if (Main_Audio_Albumart_url != "http://10.0.10.60" + Main_Playinfo.albumart_url)
            {
                Main_Audio_Albumart_url = "http://10.0.10.60" + Main_Playinfo.albumart_url;
                Main_Audio_Albumart_changed = true;
            }
        }
        
        
        

    }
    void Main_Audio_Albumart_OnRequestFinished(HTTPRequest Main_Audio_Albumart_request, HTTPResponse response)
    {
        if(response.DataAsTexture2D != null)
        {
            
         //   Debug.Log(Main_Audio_Albumart_request.Downloaded + " / " + Main_Audio_Albumart_request.DownloadLength);
            Main_Audio_Albumart.sprite = Sprite.Create(response.DataAsTexture2D, new Rect(0, 0, response.DataAsTexture2D.width, response.DataAsTexture2D.height), new Vector2(0, 0));
            Main_Audio_Albumart.enabled = true;
            Main_Audio_Albumart_changed = false;
        }
        else
        {
            Main_Audio_Albumart.enabled = false;
        }
    }


        //Debug.Log(Main_Audio_Albumart_url);
        //Main_Audio_Volume.text = Main_Status.volume.ToString();
        //Main_Audio_Input.text = Main_Status.input;
        //Main_Audio_SoundProgram.text = Main_Status.sound_program;

    }

