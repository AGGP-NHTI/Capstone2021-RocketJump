using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerPositionManager
{
    public int nodePosition;
    public int lap;
    public int position;
    public GameObject player;
    public PositionManager positionManager;

    public PlayerPositionManager(GameObject plr, PositionManager owner)
    {
        nodePosition = 0;
        lap = 1;
        position = 0;
        player = plr;
        positionManager = owner;
    }

    public void updatePosition()
    {
        //player.GetComponent<PlayerController>().updateLap(lap);
    }

    public void updatePosition(int nodeNum)
    {
        if(nodeNum > nodePosition) { nodePosition = nodeNum; }
        else if(nodeNum == 0 && nodePosition == positionManager.lastNode - 1) { nodePosition = nodeNum; lap++; }

        Debug.Log(nodePosition + ", " + lap);

        player.GetComponent<PlayerController>().updateLap(lap);
    }
}
