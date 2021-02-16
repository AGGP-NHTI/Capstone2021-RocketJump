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

    void Start()
    {
        timeleft = MaxTime;
    }

    void FixedUpdate()
    {
        timeleft -= Time.fixedDeltaTime;
        if (timeleft <= 0)
        {
            // set player to spectate and destroy the pawn
            CurrentOwner = logic.RandomPlayer();
            NewPlayer(CurrentOwner);
            timeleft = MaxTime;
        }
    }

    public void SwapPlayer()
    {
        GameObject p;
        p = logic.RandomPlayer();
        CurrentOwner = p;
        NewPlayer(CurrentOwner);
    }

    public void NewPlayer(GameObject player)
    {
        gameObject.transform.position = player.transform.position;
        gameObject.transform.SetParent(player.transform);
        
    }
}
