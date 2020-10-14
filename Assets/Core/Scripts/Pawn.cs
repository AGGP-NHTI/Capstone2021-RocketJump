using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
public class Pawn : Actor
{
    PlayerController controller;

    public void Possesed(PlayerController c)
    {
        controller = c;

        OnPossess();

        NetworkedObject netObj = GetComponent<NetworkedObject>();
        if(netObj)
        {
            netObj.ChangeOwnership(c.NetworkId);
        }
    }

    public virtual void OnPossess()
    {

    }

    public virtual void OnUnPossess()
    {

    }
}
