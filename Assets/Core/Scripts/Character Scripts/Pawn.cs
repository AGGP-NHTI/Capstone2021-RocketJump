using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
public class Pawn : Actor
{
    public Controller controller;

    public void Possesed(Controller c)
    {
        controller = c;

        OnPossess();

        NetworkedObject netObj = GetComponent<NetworkedObject>();
        if(netObj)
        {
            netObj.ChangeOwnership(c.OwnerClientId);
        }
    }

    public virtual void OnPossess()
    {

    }

    public virtual void OnUnPossess()
    {

    }
}
