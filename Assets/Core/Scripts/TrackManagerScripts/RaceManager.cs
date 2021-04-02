using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RaceManager : MonoBehaviour
{
    [Header("Additional Resources")]
    Timer timer = new Timer();

    [Header("General Settings")]
    public bool enableLobby = false;
    public int countdown;
    public bool countdownActive = false;

    [Header("Canvas Elements")]
    public Canvas lobbyCanvas;
    public TextMeshProUGUI countdownText;


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

        if(timer.updateTimer())
        {
            print("countdown finished");
        }
        if (timer.runTimer)
        {
            countdownText.enabled = true;
            countdownText.text = "Game starting in " + Mathf.Round(timer.time);
        } else { countdownText.enabled = false; }
        


    }

    public void hostStartGame()
    {
        timer.setTimer(10);
    }
}
