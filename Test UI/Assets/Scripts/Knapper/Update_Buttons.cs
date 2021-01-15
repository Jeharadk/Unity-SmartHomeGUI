using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Update_Buttons : MonoBehaviour {
    public Image BG_Home;
    public Image BG_Lights;
    public Image BG_Music;
    public Image BG_CCTV;
    public Image BG_Heating;


    public void Set_BG_Color_HomeAktiv()
    {
        BG_Home.color = new Color32(1, 186, 154, 255);
        BG_Lights.color = new Color32(60, 60, 60, 255);
        BG_Music.color = new Color32(60, 60, 60, 255);
        BG_CCTV.color = new Color32(60, 60, 60, 255);
        BG_Heating.color = new Color32(60, 60, 60, 255);
    }

    public void Set_BG_Color_LightsAktiv()
    {
        BG_Lights.color = new Color32(1, 186, 154, 255);
        BG_Home.color = new Color32(60, 60, 60, 255);
        BG_Music.color = new Color32(60, 60, 60, 255);
        BG_CCTV.color = new Color32(60, 60, 60, 255);
        BG_Heating.color = new Color32(60, 60, 60, 255);
    }

    public void Set_BG_Color_MusicAktiv()
    {
        BG_Lights.color = new Color32(60, 60, 60, 255);
        BG_Home.color = new Color32(60, 60, 60, 255);
        BG_Music.color = new Color32(1, 186, 154, 255);
        BG_CCTV.color = new Color32(60, 60, 60, 255);
        BG_Heating.color = new Color32(60, 60, 60, 255);
    }
}
