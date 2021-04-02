﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;

public class Player_Controller : Controller
{
    public GameObject player = null;
    public Player_Pawn playerPawn = null;
    public bool PlayerSpawned = false;
    public PlayerInformation_SO playerInfo;

    [Header("Characters")]
    public GameObject defaultPawn;
    public GameObject sashaPawn;
    public GameObject chappiePawn;
    public GameObject dictatorPawn;

    private void Awake()
    {
        //var fetchPlayerInfo = GameObject.Find("PlayerInformation");
        //if (fetchPlayerInfo)
        //{

        //    print("Player Information Fetched");

        //    playerInfo = fetchPlayerInfo.GetComponent<PlayerInformationCarrier>().playerInfo;

        //    switch(playerInfo.playerCharacter)
        //    {
        //        case 0:
        //            print("spectator spawned");
        //            PlayerPawn = defaultPawn;
        //            break;
        //        case 1:
        //            print("dictator spawned");
        //            PlayerPawn = dictatorPawn;
        //            break;
        //        case 2:
        //            PlayerPawn = chappiePawn;
        //            print("chappie spawned");
        //            break;
        //        case 3:
        //            PlayerPawn = sashaPawn;
        //            print("sasha spawned");
        //            break;
        //    }

        //}
        //else
        //{
        //    print("Player Information Not Found");
        //}
    }

    private void Start()
    {
        SpawnPlayerPawn();
    }

    //private void Update()
    //{
    //    if (!IsLocalPlayer)
    //    { return; }

    //    if (!ControlledPawn)
    //    { return; }
    //}

    public void SpawnPlayerPawn()
    {
        if (IsOwner)
        {
            InvokeServerRpc(Server_SpawnPlayerPawn);
        }
    }

    [ServerRPC(RequireOwnership = false)]
    public void Server_SpawnPlayerPawn()
    {
        Vector3 location = Vector3.right * NetworkId;
        location += Vector3.up * 10;
        player = Instantiate(defaultPawn, location, Quaternion.identity);

        playerPawn = player.GetComponent<Player_Pawn>();

        NetworkedObject netObj = player.GetComponent<NetworkedObject>();
        Debug.Log($"[1] {netObj.name}'s client ID is {netObj.OwnerClientId}");

        //client rpc to set the controller before
        netObj.SpawnWithOwnership(netObj.OwnerClientId);

        

        PossessPawn(player, netObj.OwnerClientId, netObj.NetworkId);

        Debug.Log($"[2] {netObj.name}'s client ID is {netObj.OwnerClientId}");

        InvokeClientRpcOnEveryone(setFields, playerPawn.controller, playerPawn, player);
    }

    [ClientRPC]
    void setFields(Controller c, Player_Pawn p, GameObject g)
    {
        player = g;
        playerPawn = p;
        playerPawn.controller = c;
    }




}
