using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using MLAPI;
using MLAPI.Messaging;
using UnityEngine.SceneManagement;

public class PlayerNetworkCenter : NetworkedBehaviour
{

    public Player_Controller owner;
    public PositionManager positionManager;
    public GameObject track;
    public RaceManager raceManager;
    public UIManager UI_manager;
    public bool initPlayer = false;
    public bool isEnabled;
    public bool checkForClientsDisconnect;

    private void Update()
    {
        if(!IsLocalPlayer || !IsOwner)
        {
            if(!owner)
            {
                owner = PlayerInformation.controller;
            }
            if(!UI_manager && PlayerInformation.controller.PNC.UI_manager)
            {
                UI_manager = PlayerInformation.controller.PNC.UI_manager;
            }
        }

        if(IsHost)
        {
            if(checkForClientsDisconnect)
            {
                if(positionManager.playerPositions.Count == 1)
                {
                    NetworkingManager.Singleton.StopHost();
                    SceneManager.LoadScene("MainMenu");
                }
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
        else if(IsClient && !IsLocalPlayer)
        {
            print("is client and not is local player");
            //setUIManager();
        }
    }

    public void initHost()
    {
        initPlayer = true;
        setTrack();
        setPositionManager();
        if (positionManager)
        {
            print(owner.playerCharacter);
            positionManager.updatePlayerList(owner.gameObject, owner.playerName, owner.OwnerClientId, owner.playerCharacter);
        }

        initRaceManager();
    }

    public void initClient()
    {
        Debug.Log("initClient");
        setTrack();
        initPlayer = true;
        InvokeServerRpc(serverAddPlayer, owner.gameObject, owner.playerName, owner.OwnerClientId, PlayerInformation.playerCharacter);
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

    public void setTrack()
    {
        track = GameObject.Find("track");
        if(track)
        {
            raceManager = track.GetComponent<RaceManager>();
        }
        
    }

    public void failsafeDisconnectHost()
    {
        NetworkingManager.Singleton.StopHost();
        SceneManager.LoadScene("MainMenu");
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

    public void updateClientLobbiesNew(int updateType, string[] playerNames, int[] playerCharacters)
    {
        switch(updateType)
        {
            case 0:
                InvokeClientRpcOnEveryone(clientUpdateLobbyCountdown, raceManager.countdown);
                break;
            case 1:
                InvokeClientRpcOnEveryone(clientUpdateLobbyNew, playerNames, playerCharacters);
                break;
        }
    }

    public void updateFinishCountdown(int c)
    {
        InvokeClientRpcOnEveryone(clientUpdateFinishCountdown, c);
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
            //positionManager.comparePlayerPositions(this);
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

    public void hostSendPlayerFinished(string[] name)
    {
        InvokeClientRpcOnEveryone(playerFinishedRace, name);
    }

    public void hostSendPlayerLap(int lap, int maxLap, ulong id)
    {
        InvokeClientRpcOnClient(clientUpdateLap, id, lap, maxLap);
    }

    public void shutdownServer()
    {
        InvokeClientRpcOnEveryone(disconnectClientsFromServer);
        checkForClientsDisconnect = true;
    }

    [ServerRPC(RequireOwnership = false)]
    public void serverAddPlayer(GameObject player, string name, ulong clientID, int character)
    {
        Debug.Log("Client add player, " + positionManager + ", " + player.name);
        positionManager.updatePlayerList(player, name, clientID, character);
    }

    [ServerRPC(RequireOwnership = false)]
    public void serverUpdateNodePosition(int nodeNumber, GameObject player)
    {
        //var pm = GameObject.Find("track").GetComponent<PositionManager>();
        positionManager.updatePlayerPosition(player, nodeNumber);
        //positionManager.comparePlayerPositions(this);
    }

    [ServerRPC(RequireOwnership = false)]
    public void serverRemoveClient(ulong clientID)
    {
        positionManager.removeClient(clientID);
    }

    [ClientRPC()]
    public void clientUpdateLobby(string name, bool start, bool end)
    {
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
    public void clientUpdateLobbyNew(string[] playerNames, int[] playerCharacters)
    {
        if (!IsHost)
        {
            if (!raceManager)
            {
                setTrack();
            }
            raceManager.clientPopulatePlayerListNew(playerNames, playerCharacters);
        }
    }

    [ClientRPC()]
    public void clientUpdateLobbyCountdown(int countdown)
    {
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
    public void clientUpdateFinishCountdown(int countdown)
    {
        if (!IsHost)
        {
            if (!UI_manager)
            {
                return;
            }
            UI_manager.updateFinishCountdown(countdown);
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
        if(UI_manager)
        {
            UI_manager.updatePositionText(pos);
        }
    }

    [ClientRPC()]
    public void playerFinishedRace(string[] name)
    {
        for(int i = 0; i < name.Length; i++)
        {
            print(name[i]);
        }

        UI_manager.displayFinishScreen(name);
    }

    [ClientRPC()]
    public void clientUpdateLap(int lap, int maxLap)
    {
        if(UI_manager)
        {
            UI_manager.setLapText(lap, maxLap);
        }
    }

    [ClientRPC()]
    public void disconnectClientsFromServer()
    {

        if(!IsHost)
        {
            InvokeServerRpc(serverRemoveClient, PlayerInformation.controller.OwnerClientId);
            NetworkingManager.Singleton.StopClient();
            SceneManager.LoadScene("MainMenu");
        }
        
    }

    
}
