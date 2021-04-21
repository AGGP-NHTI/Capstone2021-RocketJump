using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerPositionManager
{
    public int nodePosition;
    public int lap;
    public int position;
    public string name;
    public GameObject player;
    public PositionManager positionManager;

    public PlayerPositionManager(GameObject plr, PositionManager owner, string plrname)
    {
        nodePosition = 0;
        lap = 1;
        position = 1;
        player = plr;
        positionManager = owner;
        name = plrname;
    }

    public void updatePosition()
    {
        //player.GetComponent<PlayerController>().updateLap(lap);
    }

    public void updatePosition(int nodeNum)
    {
        if(nodeNum == nodePosition + 1) { nodePosition = nodeNum; }
        else if(nodeNum == 0 && nodePosition == positionManager.lastNode - 1) { nodePosition = nodeNum; lap++; }

        if(lap > 1)
        {
            Debug.Log(name + " WINS");
        }

        Debug.Log(nodePosition + ", " + lap);

        //player.GetComponent<PlayerController>().updateLap(lap);
    }
}
