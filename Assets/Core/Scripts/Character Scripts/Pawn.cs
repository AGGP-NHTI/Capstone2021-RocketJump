using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
public class Pawn : Actor
{
    public Controller controller;
    public Transform eyes;


    public void Possessed(Controller c)
    {
        controller = c;

        OnPossess();

        NetworkedObject netObj = GetComponent<NetworkedObject>();
        if (netObj)
        {
            netObj.ChangeOwnership(c.OwnerClientId);
        }
        else
        {
            Debug.Log($"{c.name} is not a networked object.");
        }


    }

    public virtual void OnPossess()
    {

    }

    public virtual void OnUnPossess()
    {

    }
}
