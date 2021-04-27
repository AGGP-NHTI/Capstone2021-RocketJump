using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using MLAPI;
using MLAPI.Messaging;

public class PlayerNetworkCenter : NetworkedBehaviour
{

    public Player_Controller owner;
    public PositionManager positionManager;
    public GameObject track;
    public RaceManager raceManager;
    public bool initPlayer = false;
    public bool isEnabled;

    private void Update()
    {
        if(!IsLocalPlayer || !IsOwner)
        {
            //Debug.Log("not me update");
            if(!owner)
            {
                Debug.Log("owner: " + PlayerInformation.controller);
                owner = PlayerInformation.controller;
            }
        }
    }

    public void initPNC(Player_Controller reference)
    {
        if(IsLocalPlayer)
        {
            owner = reference;

            if(!PlayerInformation.controller)
            {
                PlayerInformation.controller = owner;
                Debug.Log("owner: " + PlayerInformation.controller);
            }
        }
        else
        {
            owner = PlayerInformation.controller;
            Debug.Log("owner: " + PlayerInformation.controller);
        }
        
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
            positionManager.updatePlayerList(owner.gameObject, owner.playerName, owner.OwnerClientId);
        }

        initRaceManager();
    }

    public void initClient()
    {
        Debug.Log("initClient");
        setTrack();
        initPlayer = true;
        InvokeServerRpc(serverAddPlayer, owner.gameObject, owner.playerName, owner.OwnerClientId);
    }

    public void setPositionManager()
    {
        if(!track) { return; }

        positionManager = track.AddComponent<PositionManager>();
        if (positionManager && track)
        {
            positionManager.track = track;
            positionManager.initPositionManager();
        }
    }

    public void initRaceManager()
    {
        if (raceManager && owner)
        {
            raceManager.hostPlayer = owner.gameObject;
            raceManager.enableLobby = true;
            raceManager.isHost = true;
        }
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
                Debug.Log("Update Countdown");
                InvokeClientRpcOnEveryone(clientUpdateLobbyCountdown, raceManager.countdown);
                break;
            case 1: //Update Playerlist
                Debug.Log("Update PlayerList");
                InvokeClientRpcOnEveryone(clientUpdateLobby, name, start, end);
                break;
        }
        
    }

    public void spawnPlayerForRace()
    {
        owner.SpawnPlayerPawn(PlayerInformation.playerCharacter);
        InvokeClientRpcOnEveryone(spawnPlayer);
    }

    public void updateNodePosition(PositionNodeScript node)
    {
        if (IsHost)
        {
            positionManager.updatePlayerPosition(gameObject, node.nodeNumber);
            positionManager.comparePlayerPositions(this);
        }
        else if (IsClient)
        {
            //clientUpdateNodePosition(node, gameObject);
            InvokeServerRpc(serverUpdateNodePosition, node.nodeNumber, gameObject);
        }
    }

    public void requestRespawn()
    {
        if(IsHost)
        {
            Debug.Log("request spawn");
            positionManager.respawnPlayer(gameObject);
        }
    }

    public void hostSendClientPositionUpdate(int pos, ulong id)
    {
        InvokeClientRpcOnClient(updateClientPosition, id, pos);
    }

    public void hostSendPlayerFinished()
    {

    }

    [ServerRPC(RequireOwnership = false)]
    public void serverAddPlayer(GameObject player, string name, ulong clientID)
    {
        Debug.Log("Client add player, " + positionManager + ", " + player.name);
        positionManager.updatePlayerList(player, name, clientID);
    }

    [ServerRPC(RequireOwnership = false)]
    public void serverUpdateNodePosition(int nodeNumber, GameObject player)
    {
        //var pm = GameObject.Find("track").GetComponent<PositionManager>();
        positionManager.updatePlayerPosition(player, nodeNumber);
        positionManager.comparePlayerPositions(this);
    }

    [ClientRPC()]
    public void clientUpdateLobby(string name, bool start, bool end)
    {
        Debug.Log("clientUpdateLobby");
        if(!IsHost)
        {
            if (!raceManager)
            {
                setTrack();
            }
            raceManager.clientPopulatePlayerList(name, start, end);
        }
    }

    [ClientRPC()]
    public void clientUpdateLobbyCountdown(int countdown)
    {
        Debug.Log("Client Update Countdown");
        if (!IsHost)
        {
            if (!raceManager)
            {
                setTrack();
            }
            raceManager.updateLobbyCountdown(countdown);
        }
    }

    [ClientRPC()]
    public void spawnPlayer()
    {
        if(!IsHost)
        {
            Debug.Log("Spawn player");
            raceManager.enableLobby = false;
            owner.SpawnPlayerPawn(PlayerInformation.playerCharacter);
        }
    }

    [ClientRPC()]
    public void updateClientPosition(int pos)
    {
        Debug.Log("I am in position " + pos);
    }

    [ClientRPC()]
    public void playerFinishedRace()
    {
        if(!IsHost)
        {

        }
    }
}
