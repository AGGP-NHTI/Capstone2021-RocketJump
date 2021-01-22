using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerPositionManager
{
    public int nodePosition;
    public int lap;
    public GameObject player;

    public PlayerPositionManager(GameObject plr)
    {
        nodePosition = 0;
        lap = 0;
        player = plr;
    }
}
