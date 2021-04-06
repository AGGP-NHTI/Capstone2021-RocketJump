using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using MLAPI.Messaging;

public class PlayerNetworkCenter
{

    private Player_Pawn owner;
    public PositionManager positionManager;
    public GameObject track;
    public bool initPlayer = false;
    public bool enabled;

    public PlayerNetworkCenter(Player_Pawn reference)
    {
        owner = reference;
    }

    public void initHost()
    {
        initPlayer = true;
        setTrack();
        setPositionManager();
        if (positionManager)
        {
            positionManager.updatePlayerList(owner.gameObject);
        }
    }

    public void initClient()
    {

    }

    public void setPositionManager()
    {
        positionManager = track.AddComponent<PositionManager>();
        if (positionManager && track)
        {
            positionManager.track = track;
            positionManager.initPositionManager();
        }
    }

    public void initializePositionManager()
    {
        initPlayer = true;
        setTrack();
        setPositionManager();
        positionManager.updatePlayerList(owner.gameObject);
    }

    public void setTrack()
    {
        track = GameObject.Find("track");
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
