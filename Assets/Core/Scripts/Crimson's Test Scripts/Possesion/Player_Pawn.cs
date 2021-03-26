using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Pawn : Pawn
{
    Pawn[] pawns;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnPossess()
    {
        base.OnPossess();

        pawns = GetComponents<Pawn>();

        foreach (Pawn p in pawns)
        {
            p.controller = controller;
        }
    }

    private void OnDisable()
    {
        foreach (Pawn p in pawns)
        {
            p.enabled = false;   
        }
    }
}
