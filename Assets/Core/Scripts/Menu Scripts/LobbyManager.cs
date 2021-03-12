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
    public GameObject scrollMenuContent;
    public GameObject serverSlotPrefab;
    public List<GameObject> serverSlots;

    private void Start()
    {
        serverSlots = new List<GameObject>();
        findServers();
    }

    public void findServers()
    {

        var spacing = 0;

        foreach(var connection in connectionList)
        {
            var tempServer = Instantiate(serverSlotPrefab, scrollMenuContent.transform);
            var serverInfo = tempServer.GetComponent<ServerSlotScript>();
            serverInfo.info = connection;
            tempServer.transform.position = new Vector2(tempServer.transform.position.x, tempServer.transform.position.y - spacing);
            spacing += 175;
        }
    }
}
