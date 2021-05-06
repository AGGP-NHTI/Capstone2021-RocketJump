using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PositionManager : MonoBehaviour
{

    public GameObject track; // Point a reference to the track
    public int lastNode;
    public List<GameObject> players;
    [SerializeField] List<Transform> positionNodes;
    public List<PlayerPositionManager> playerPositions;
    public int maxLap = 3;

    public void initPositionManager()
    {
        if (!track)
        {
            Debug.Log("Track not found; " + gameObject.name);
        }
        else
        {

            Debug.Log("Track found");

            positionNodes = new List<Transform>();

            foreach (Transform node in track.transform.Find("positionNodes"))
            {
                positionNodes.Add(node);
            }

            positionNodes.Sort((x, y) => x.GetComponent<PositionNodeScript>().nodeNumber.CompareTo(y.GetComponent<PositionNodeScript>().nodeNumber));

            playerPositions = new List<PlayerPositionManager>();
            players = new List<GameObject>();

            lastNode = positionNodes.Count;

        }
    }

    public void updatePlayerList(GameObject player, string name, ulong clientID, int character)
    {
        playerPositions.Add(new PlayerPositionManager(player, this, name, clientID, character));
        players.Add(player);

        updatePlayerPosition(player);
    }

    public void updatePlayerPosition(GameObject player)
    {

        foreach (PlayerPositionManager p in playerPositions)
        {
            if (p.player == player)
            {
               // p.updatePosition();
            }
        }
    }

    public void updatePlayerPosition(GameObject player, int nodeNum)
    {

        foreach (PlayerPositionManager p in playerPositions)
        {
            if(p.player == player)
            {
                p.updatePosition(nodeNum);
            }
        }

        comparePlayerPositions();
    }

    public void comparePlayerPositions()
    {

        playerPositions = (playerPositions.OrderByDescending(p => p.lap).ThenByDescending(p => p.nodePosition)).ToList();

        for(int i = 0; i < playerPositions.Count; i++)
        {
            PlayerInformation.controller.PNC.hostSendClientPositionUpdate(i + 1, playerPositions[i].clientID);
        }

    }

    public void removeClient(ulong id)
    {
        for(int i = 0; i < playerPositions.Count; i++)
        {
            if(playerPositions[i].clientID == id)
            {
                playerPositions.RemoveAt(i);
                print("player removed");
            }
        }
    }

    public void playerFinishedRace()
    {
        comparePlayerPositions();

        string[] playerNames = new string[playerPositions.Count];

        for(int i = 0; i < playerPositions.Count; i++)
        {
            playerNames[i] = playerPositions[i].name;
        }

        PlayerInformation.controller.PNC.hostSendPlayerFinished(playerNames);
    }

    public void updatePlayerLaps(int lap, ulong id)
    {

        for (int i = 0; i < playerPositions.Count; i++)
        {
            PlayerInformation.controller.PNC.hostSendPlayerLap(lap, maxLap, id); ;
        }
    }

    public void respawnPlayer(GameObject player)
    {
        Debug.Log("respawn player");

        var node = 0;

        foreach(PlayerPositionManager p in playerPositions)
        {
            if(p.player == player)
            {
                Debug.Log("player node");
                node = p.nodePosition;
                print(positionNodes[node]);
                player.GetComponent<Player_Controller>().ControlledPawn.gameObject.transform.position = positionNodes[node].position;
            }
        }

        
    }

}
