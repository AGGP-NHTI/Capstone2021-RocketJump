﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
public class Pawn : Actor
{
    public Controller controller;
    


    public void Possessed(Controller c)
    {
        controller = c;

        OnPossess();

        NetworkedObject netObj = GetComponent<NetworkedObject>();
        if (netObj && IsServer)
        {
            netObj.ChangeOwnership(c.OwnerClientId);
        }
        else
        {
            Debug.Log($"{c.name} is not a networked object or is a client.");
        }


    }

    public virtual void OnPossess()
    {
        if (IsServer)
        {
            Debug.Log($"SERVER CONTROLLER IS {controller} for GO {name}");
        }
        else
        {
            Debug.Log($"CLIENT CONTROLLER IS {controller} for GO {name}");
        }

    }

    public virtual void OnUnPossess()
    {

    }
}
