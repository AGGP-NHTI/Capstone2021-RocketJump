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
    private List<string> clientPlayerNames = new List<string>();

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
                    updateClientLobbies(0, null, false, false);
                }
            }
            else { countdownText.enabled = false; }

            if (positionManager.players.Count != oldPlayerListCount)
            {
                oldPlayerListCount = positionManager.players.Count;

                populatePlayerList();

                
            }
        }
        else
        {
            countdownText.enabled = true;
            countdownText.text = "Game starting in " + Mathf.Round(timer.time);
        }

        
    }

    public void updateClientLobbies(int updateType, string name, bool start, bool end)
    {
        hostPlayer.GetComponent<Player_Pawn>().PNC.updateClientLobbies(updateType, name, start, end);
    }

    public void updateLobbyCountdown(int c)
    {
        countdown = c;
        //populatePlayerList();
    }

    public void clientPopulatePlayerList(string name, bool start, bool end)
    {

        if(start)
        {
            clientPlayerNames.Clear();
        }

        clientPlayerNames.Add(name);

        if(end)
        {
            populatePlayerList();
        }
    }

    public void populatePlayerList()
    {

        var offset = 145f;

        if(isHost)
        {
            playerSlots.Clear();

            var playerList = positionManager.players;

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
        else
        {
            var playerList = clientPlayerNames;

            foreach(string p in playerList)
            {
                var slot = Instantiate(playerSlotPrefab, playerListPanel.transform);

                slot.transform.Find("playerName").GetComponent<TextMeshProUGUI>().text = p;
                slot.transform.position = new Vector2(slot.transform.position.x, slot.transform.position.y + offset);
                offset -= 10;
            }
        }
        
    }

    public void hostStartGame()
    {
        timer.setTimer(60);
        countdownActive = true;
    }
}
