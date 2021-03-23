using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;

public class Player_Controller : Controller
{

    public GameObject SpectatorPawn = null;

    public GameObject PlayerPawn = null;
    public bool PlayerSpawned = false;
    private void Start()
    {
        SpawnSpectatorPawn();

        
    }

    private void Update()
    {
        if (!IsLocalPlayer)
        { return; }

        if (!ControlledPawn)
        { return; }

        if (PlayerPawn != null && PlayerSpawned == false)
        {
            SpawnPlayerPawn();
            PlayerSpawned = true;

            SpectatorPawn.SetActive(false);
        }
        
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
        NetworkedObject netObj = playerPawn.GetComponent<NetworkedObject>();

        netObj.SpawnWithOwnership(OwnerClientId);


        PossessPawn(playerPawn, netObj.OwnerClientId, netObj.NetworkId);
    }

    public void SpawnSpectatorPawn()
    {
        if (IsOwner)
        {
            InvokeServerRpc(Server_SpawnSpectatorPawn);
        }
    }

    [ServerRPC(RequireOwnership = false)]
    public void Server_SpawnSpectatorPawn()
    {
        Vector3 location = Vector3.right * NetworkId;
        location += Vector3.up * 10;
        GameObject specPawn = Instantiate(SpectatorPawn, location, Quaternion.identity);
        NetworkedObject netObj = specPawn.GetComponent<NetworkedObject>();

        netObj.SpawnWithOwnership(OwnerClientId);


        PossessPawn(specPawn, netObj.OwnerClientId, netObj.NetworkId);
    }

}
