using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Payload : MonoBehaviour
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
            timeleft = MaxTime;
        }
    }

    public void SwapPlayer()
    {

    }
}
