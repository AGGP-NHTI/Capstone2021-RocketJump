﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using MLAPI;
using MLAPI.Messaging;

public class PlayerNetworkCenter : NetworkedBehaviour
{

    private Player_Controller owner;
    public PositionManager positionManager;
    public GameObject track;
    public RaceManager raceManager;
    public bool initPlayer = false;
    public bool enabled;

    public void initPNC(Player_Controller reference)
    {
        owner = reference;
        if(IsServer && IsLocalPlayer)
        {
            initHost();
        }
        else if(IsServer && !IsLocalPlayer)
        {
            setTrack();
            positionManager = track.GetComponent<PositionManager>();
        }
        else if(IsClient && IsLocalPlayer)
        {
            initClient();
        }
    }

    public void initHost()
    {
        initPlayer = true;
        setTrack();
        setPositionManager();
        if (positionManager)
        {
            positionManager.updatePlayerList(owner.gameObject, owner.playerName);
        }

        initRaceManager();
    }

    public void initClient()
    {
        Debug.Log("initClient");
        setTrack();
        initPlayer = true;
        InvokeServerRpc(serverAddPlayer, owner.gameObject, owner.playerName);
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

    public void initRaceManager()
    {
        raceManager.hostPlayer = owner.gameObject;
        raceManager.enableLobby = true;
        raceManager.isHost = true;
    }

    /*
    public void initializePositionManager()
    {
        initPlayer = true;
        setTrack();
        setPositionManager();
        positionManager.updatePlayerList(owner.gameObject, owner.playerName);
    }
    */

    public void setTrack()
    {
        track = GameObject.Find("track");
        if(track)
        {
            raceManager = track.GetComponent<RaceManager>();
        }
        
    }

    public void updateClientLobbies(int updateType, string name, bool start, bool end)
    {
        switch(updateType)
        {
            case 0: //Update Countdown
                InvokeClientRpcOnEveryone(clientUpdateLobbyCountdown, raceManager.countdown);
                break;
            case 1: //Update Playerlist
                Debug.Log("Update PlayerList");
                InvokeClientRpcOnEveryone(clientUpdateLobby, name, start, end);
                break;
        }
        
    }

    [ServerRPC(RequireOwnership = false)]
    public void serverAddPlayer(GameObject player, string name)
    {
        Debug.Log("Client add player, " + positionManager + ", " + player.name);
        positionManager.updatePlayerList(player, name);
    }

    [ServerRPC(RequireOwnership = false)]
    public void serverUpdateNodePosition(int nodeNumber, GameObject player)
    {
        //positionManager.updatePlayerPosition(player, nodeNumber);
        var pm = GameObject.Find("track").GetComponent<PositionManager>();
        pm.updatePlayerPosition(player, nodeNumber);
    }

    [ClientRPC()]
    public void clientUpdateLobby(string name, bool start, bool end)
    {
        Debug.Log("clientUpdateLobby");
        if (!raceManager)
        {
            setTrack();
        }
        raceManager.clientPopulatePlayerList(name, start, end);
    }

    [ClientRPC()]
    public void clientUpdateLobbyCountdown(int countdown)
    {
        raceManager.updateLobbyCountdown(countdown);
    }
}
