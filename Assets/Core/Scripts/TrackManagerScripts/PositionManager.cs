using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*

    Alright, the way I have this set up at the moment may look kinda confusing, so Ima do my best to explain it here.
    This script keeps track of all the players positions. This way the position of all players is determined and maintained in a single location (this script, server side) rather than each player
    keeping track of their own position and each player having to communicate with the server and relay their position to the server and yatta yatta yatta.
    Basically this script keeps track of two things: the position nodes, and the players positions. Whenever a position node is triggered, it relays its values along with the player who touched it back to here.
    This script takes that information and uses it to update the respective players position and lap information, which is held in its own PlayerPositionManager element inside the playerPositions list.
    When a player connects, updatePlayerList is called from that players PlayerControllers Awake function, which creates and adds a PlayerPositionManager to the list for that player.

    Somethings aren't complete yet, but the above is a basic overview of how it WILL work when its done.

    */

public class PositionManager : MonoBehaviour
{

    public GameObject track; // Point a reference to the track
    public int lastNode;
    public List<GameObject> players;
    [SerializeField] List<Transform> positionNodes;
    [SerializeField] List<PlayerPositionManager> playerPositions;

    private void Start()
    {
        if (!track)
        {
            Debug.Log("Track not found; " + gameObject.name);
        }
        else
        {

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

    private void Awake()
    {
        
    }

    void Update()
    {
        
    }

    public void updatePlayerList(GameObject player)
    {
        playerPositions.Add(new PlayerPositionManager(player, this));
        players.Add(player);

        updatePlayerPosition(player);
    }

    public void updatePlayerPosition(GameObject player)
    {

        foreach (PlayerPositionManager p in playerPositions)
        {
            if (p.player == player)
            {
                p.updatePosition();
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

    public void comparePlayerPositions()
    {

        //positionNodes.Sort((x, y) => x.GetComponent<PositionNodeScript>().nodeNumber.CompareTo(y.GetComponent<PositionNodeScript>().nodeNumber));

        playerPositions.Sort((x, y) => x.lap.CompareTo(y.lap));

        var prevPlayer = playerPositions[0];

        foreach (PlayerPositionManager p in playerPositions)
        {
            if(p == prevPlayer) { break; }

            if(p.lap == prevPlayer.lap)
            {
                if(p.nodePosition > prevPlayer.nodePosition)
                {
                    p.position = prevPlayer.position;
                    prevPlayer.position -= 1;
                }
            }

            prevPlayer = p;
        }
    }
}
