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

    public void updatePlayerList(GameObject player, string name, ulong clientID)
    {
        playerPositions.Add(new PlayerPositionManager(player, this, name, clientID));
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

        //comparePlayerPositions();
    }

    public void comparePlayerPositions(PlayerNetworkCenter sender)
    {

        playerPositions = (playerPositions.OrderByDescending(p => p.lap).ThenByDescending(p => p.nodePosition)).ToList();

        for(int i = 0; i < playerPositions.Count; i++)
        {
            sender.hostSendClientPositionUpdate(i + 1, playerPositions[i].clientID);
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
