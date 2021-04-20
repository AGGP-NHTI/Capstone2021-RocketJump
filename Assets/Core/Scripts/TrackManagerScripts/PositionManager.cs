﻿using System.Collections;
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

    public void updatePlayerList(GameObject player, string name)
    {
        playerPositions.Add(new PlayerPositionManager(player, this, name));
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

        playerPositions = (playerPositions.OrderByDescending(p => p.lap).ThenByDescending(p => p.nodePosition)).ToList();

    }
}
