using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;

public class Player_Controller : Controller
{
    public GameObject PlayerPawn = null;
    public bool PlayerSpawned = false;
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
        GameObject playerPawn = Instantiate(PlayerPawn, location, Quaternion.identity);
        NetworkedObject netObj = playerPawn.GetComponent<NetworkedObject>();

        PossessPawn(playerPawn, netObj.OwnerClientId, netObj.NetworkId);
        netObj.SpawnWithOwnership(OwnerClientId);
    }
}
