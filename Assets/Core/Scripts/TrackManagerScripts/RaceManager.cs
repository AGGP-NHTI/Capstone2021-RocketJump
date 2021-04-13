using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RaceManager : MonoBehaviour
{
    [Header("Additional Resources")]
    Timer timer = new Timer();
    PositionManager positionManager;
    public GameObject track;

    [Header("General Settings")]
    public bool enableLobby = false;
    public int countdown;
    public bool countdownActive = false;
    private int oldPlayerListCount = 0;

    [Header("Canvas Elements")]
    public Canvas lobbyCanvas;
    public TextMeshProUGUI countdownText;
    public GameObject playerListPanel;
    public GameObject playerSlotPrefab;

    [Header("Lobby Information")]
    public List<GameObject> playerSlots = new List<GameObject>();
    public GameObject hostPlayer;
    public bool isHost = false;
    private float lastCountdownNumber = 100;

    void Update()
    {
        if(!track.GetComponent<PositionManager>())
        {
            return;
        }
        else
        {
            positionManager = track.GetComponent<PositionManager>();
        }

        if(enableLobby) { lobbyManager(); }
        else { lobbyCanvas.enabled = false; }
    }

    public void lobbyManager()
    {
        if(!lobbyCanvas.enabled) { lobbyCanvas.enabled = true; }

        if(isHost)
        {
            if (!countdownActive)
            {
                hostStartGame();
            }

            if (timer.updateTimer())
            {
                print("countdown finished");
            }
            if (timer.runTimer)
            {
                countdownText.enabled = true;
                countdownText.text = "Game starting in " + Mathf.Round(timer.time);

                if(Mathf.Round(timer.time) < Mathf.Round(lastCountdownNumber))
                {
                    lastCountdownNumber = Mathf.Round(timer.time);
                    hostPlayer.GetComponent<Player_Pawn>().PNC.updateClientLobbies();
                }
            }
            else { countdownText.enabled = false; }

            if (positionManager.players.Count != oldPlayerListCount)
            {
                oldPlayerListCount = positionManager.players.Count;

                populatePlayerList();

                hostPlayer.GetComponent<Player_Pawn>().PNC.updateClientLobbies();
            }
        }
        

        
    }

    public void updateLobby(List<GameObject> ps, int c)
    {
        playerSlots = ps;
        countdown = c;
        populatePlayerList();
    }

    public void populatePlayerList()
    {

        playerSlots.Clear();

        var playerList = positionManager.players;
        var offset = 145f;

        foreach (GameObject p in playerList)
        {
            var pawn = p.GetComponent<Player_Pawn>();

            var slot = Instantiate(playerSlotPrefab, playerListPanel.transform);

            slot.transform.Find("playerName").GetComponent<TextMeshProUGUI>().text = pawn.playerName;
            slot.transform.position = new Vector2(slot.transform.position.x, slot.transform.position.y + offset);
            offset -= 10;

            playerSlots.Add(slot);
        }
    }

    public void hostStartGame()
    {
        timer.setTimer(60);
        countdownActive = true;
    }
}
