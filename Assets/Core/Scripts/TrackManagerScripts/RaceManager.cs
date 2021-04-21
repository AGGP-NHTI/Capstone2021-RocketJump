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
    public GameObject lobbyCanvas;
    public TextMeshProUGUI countdownText;
    public GameObject playerListPanel;
    public GameObject playerSlotPrefab;
    public GameObject startGamebutton;

    [Header("Lobby Information")]
    public List<GameObject> playerSlots = new List<GameObject>();
    public GameObject hostPlayer;
    public bool isHost = false;
    private float lastCountdownNumber = 100;
    private List<string> clientPlayerNames = new List<string>();

    void Update()
    {
        if(isHost)
        {
            if (!track.GetComponent<PositionManager>())
            {
                return;
            }
            else
            {
                positionManager = track.GetComponent<PositionManager>();
            }

            
        }

        if (enableLobby) { lobbyManager(); }
        else { lobbyCanvas.SetActive(false); }

    }

    public void lobbyManager()
    {
        if(!lobbyCanvas.activeSelf) { lobbyCanvas.SetActive(true); }

        if(isHost)
        {

            if (timer.updateTimer())
            {
                spawnPlayers();
            }
            if (timer.runTimer)
            {
                countdownText.enabled = true;
                countdownText.text = "Game starting in " + Mathf.Round(timer.time);
                countdown = (int)Mathf.Round(timer.time);

                if(Mathf.Round(timer.time) < Mathf.Round(lastCountdownNumber))
                {
                    lastCountdownNumber = Mathf.Round(timer.time);
                    updateClientLobbies(0, null, false, false);
                }
            }
            else { countdownText.enabled = false; }

            if (positionManager.playerPositions.Count != oldPlayerListCount)
            {
                oldPlayerListCount = positionManager.playerPositions.Count;

                populatePlayerList();

                
            }
        }
        else
        {
            startGamebutton.SetActive(false);
            countdownText.text = "Game starting in " + countdown;
        }

        
    }

    public void updateClientLobbies(int updateType, string name, bool start, bool end)
    {
        Debug.Log("start updateClientLobbies");
        hostPlayer.GetComponent<Player_Controller>().PNC.updateClientLobbies(updateType, name, start, end);
        Debug.Log("end updateClientLobbies");
    }

    public void updateLobbyCountdown(int c)
    {
        print("update countdown");
        if (!enableLobby) { enableLobby = true; }
        countdown = c;
        print(countdown);
    }

    public void clientPopulatePlayerList(string name, bool start, bool end)
    {
        Debug.Log("clientPopulatePlayerList");

        if (!enableLobby) { enableLobby = true; }

        if (start)
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

        if(isHost) //Is host
        {
            playerSlots.Clear();

            var playerList = positionManager.playerPositions;
            var index = 0;

            foreach (PlayerPositionManager p in playerList)
            {
                //var pawn = p.GetComponent<Player_Controller>();

                var name = p.name;

                var slot = Instantiate(playerSlotPrefab, playerListPanel.transform);

                slot.transform.Find("playerName").GetComponent<TextMeshProUGUI>().text = name;
                slot.transform.position = new Vector2(slot.transform.position.x, slot.transform.position.y + offset);
                offset -= 75;

                playerSlots.Add(slot);

                if(index == 0)
                {
                    updateClientLobbies(1, name, true, false);
                }
                else if(index == playerList.Count - 1)
                {
                    updateClientLobbies(1, name, false, true);
                }
                else
                {
                    updateClientLobbies(1, name, false, false);
                }

                index++;
            }

        }
        else //Is client
        {
            var playerList = clientPlayerNames;

            foreach(string p in playerList)
            {
                var slot = Instantiate(playerSlotPrefab, playerListPanel.transform);

                slot.transform.Find("playerName").GetComponent<TextMeshProUGUI>().text = p;
                slot.transform.position = new Vector2(slot.transform.position.x, slot.transform.position.y + offset);
                offset -= 75;
            }
        }
        
    }

    public void hostStartGame()
    {
        timer.setTimer(3);
        countdownActive = true;
    }

    public void spawnPlayers()
    {
        enableLobby = false;

        hostPlayer.GetComponent<Player_Controller>().PNC.spawnPlayerForRace();
    }
}
