using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionManager : MonoBehaviour
{

    public GameObject track; // Point a reference to the track
    public int lastNode;
    public List<GameObject> players;
    [SerializeField] List<Transform> positionNodes;
    [SerializeField] List<PlayerPositionManager> playerPositions;

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
