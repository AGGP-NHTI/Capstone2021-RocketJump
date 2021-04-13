using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
public class Pawn : Actor
{
    public Controller controller;
    //[MLAPI.NetworkedVar.SyncedVar]
    public float testVar = 0;


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
            Debug.LogWarning($"{c.name} is not a networked object or is a client.");
        }


    }

    public virtual void OnPossess()
    {


    }

    public virtual void OnUnPossess()
    {

    }



}
