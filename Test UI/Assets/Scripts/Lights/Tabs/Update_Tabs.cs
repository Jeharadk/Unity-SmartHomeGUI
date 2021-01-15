using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Update_Tabs : MonoBehaviour {

    public TextMeshProUGUI Label_Stuen;
    public TextMeshProUGUI Label_Koekken;
    
    public void Set_LabelColor_StuenAktiv()
    {
        Label_Stuen.color = new Color32(1, 186, 154, 255);
        Label_Koekken.color = new Color32(255, 255, 255, 255);
    }

    public void Set_LabelColor_KoekkenAktiv()
    {
        Label_Koekken.color = new Color32(1, 186, 154, 255);
        Label_Stuen.color = new Color32(255, 255, 255, 255);
    }
}
