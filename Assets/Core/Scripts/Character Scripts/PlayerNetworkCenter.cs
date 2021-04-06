using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using MLAPI.Messaging;

public class PlayerNetworkCenter
{

    private Player_Pawn owner;

    public PlayerNetworkCenter(Player_Pawn reference)
    {
        owner = reference;
    }


    [ServerRPC(RequireOwnership = false)]
    public void clientAddPlayer(GameObject player)
    {
        //positionManager.updatePlayerList(player);
        var pm = GameObject.Find("track").GetComponent<PositionManager>();
        pm.updatePlayerList(player);
    }

    [ServerRPC(RequireOwnership = false)]
    public void clientUpdateNodePosition(int nodeNumber, GameObject player)
    {
        //positionManager.updatePlayerPosition(player, nodeNumber);
        var pm = GameObject.Find("track").GetComponent<PositionManager>();
        pm.updatePlayerPosition(player, nodeNumber);
    }
}
