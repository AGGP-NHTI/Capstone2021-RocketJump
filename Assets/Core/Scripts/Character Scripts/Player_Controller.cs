using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;

public class Player_Controller : Controller
{
    public GameObject PlayerPawn;

    private void Start()
    {
        SpawnPlayerPawn();
    }

    private void Update()
    {
        Debug.Log("IS LOCAL PLAYER ON CONTROLLER: " + IsLocalPlayer);
        if (!IsLocalPlayer)
        { return; }

        if (!ControlledPawn)
        { return; }
    }

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
        PlayerPawn.GetComponent<Player_Movement_Controller>().playerController = this;
        NetworkedObject netObj = playerPawn.GetComponent<NetworkedObject>();
        //netObj.Spawn();
        netObj.SpawnWithOwnership(OwnerClientId);
        

        PossessPawn(playerPawn, netObj.OwnerClientId, netObj.NetworkId);
    }
}
