using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MLAPI;
using MLAPI.Messaging;

public class Payload : NetworkedBehaviour
{
    public GameObject CurrentOwner;
    public PPLogic logic;
    public float MaxTime = 30f;
    public float timeleft;
    public bool IsOn = false;

    void Start()
    {
        timeleft = MaxTime;
    }

    void FixedUpdate()
    {
        if (IsOn)
        {
            timeleft -= Time.fixedDeltaTime;
            if (timeleft <= 0)
            {
                // set player to spectate and destroy the pawn

                InvokeServerRpc(ChoosePlayer);
            }
        }
    }

    //chooses a random player for the payload to swap to, used when the payload runs out of time and when the round first starts
    [ServerRPC(RequireOwnership = false)]
    public void ChoosePlayer()
    {
        IsOn = true;
        InvokeServerRpc(NewPlayer, logic.RandomPlayer());
        timeleft = MaxTime;
    }

    //changes the player the payload is attached to, also called by the projectile to swap it to the hit player
    [ServerRPC(RequireOwnership = false)]
    public void NewPlayer(GameObject player)
    {      
        gameObject.transform.position = player.transform.position;
        gameObject.transform.SetParent(player.transform);
        CurrentOwner = player;
    }
}