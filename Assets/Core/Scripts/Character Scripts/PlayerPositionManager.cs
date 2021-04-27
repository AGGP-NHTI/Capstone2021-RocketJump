﻿using System.Collections;
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
    public ulong clientID;

    public PlayerPositionManager(GameObject plr, PositionManager owner, string plrname, ulong id)
    {
        nodePosition = 0;
        lap = 1;
        position = 1;
        player = plr;
        positionManager = owner;
        name = plrname;
        clientID = id;
    }

    public void updatePosition(int nodeNum)
    {
        if(nodeNum == nodePosition + 1) { nodePosition = nodeNum; }
        else if(nodeNum == 0 && nodePosition == positionManager.lastNode - 1) { nodePosition = nodeNum; lap++; }

        if(lap > positionManager.maxLap)
        {
            PlayerInformation.controller.PNC.hostSendPlayerFinished(name);
        }
        else
        {
            PlayerInformation.controller.PNC.hostSendPlayerLap(lap, positionManager.maxLap);
        }
    }
}
