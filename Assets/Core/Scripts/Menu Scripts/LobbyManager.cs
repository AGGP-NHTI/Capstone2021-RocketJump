using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using MLAPI;
using MLAPI.Transports.UNET;
using TMPro;
using System;

public class LobbyManager : MonoBehaviour
{

    public List<serverInfo_SO> connectionList;

    void Start()
    {
        connectionList = new List<serverInfo_SO>();
    }

    
    void Update()
    {
        
    }
}
