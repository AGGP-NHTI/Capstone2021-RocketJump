using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ServerSlotScript : MonoBehaviour
{
    public serverInfo_SO info;
    public TextMeshProUGUI serverName;

    public void Start()
    {
        if(info != null)
        {
            serverName.text = info.serverName;
        }
    }
}
