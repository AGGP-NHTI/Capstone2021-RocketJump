using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RaceManager : MonoBehaviour
{

    [Header("General Settings")]
    public bool enableLobby = false;

    [Header("Canvas Elements")]
    public Canvas lobbyCanvas;


    void Start()
    {
        
    }

    void Update()
    {
        if(enableLobby) { lobbyManager(); }
        else { lobbyCanvas.enabled = false; }
    }

    public void lobbyManager()
    {
        if(!lobbyCanvas.enabled) { lobbyCanvas.enabled = true; }


    }
}
