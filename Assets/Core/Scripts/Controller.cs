using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;

public class Controller : NetworkedBehaviour
{
    protected Pawn ControlledPawn;

    public void PossessPawn(GameObject pawn)
    {
        Pawn p = pawn.GetComponent<Pawn>();
      
        if(p)
        {
           
            PossessPawn(p);
        }
        else
        {
            Debug.Log(pawn.name + " isn't a pawn to me");
        }
    }

        public void PossessPawn(Pawn p)
    {
        if(ControlledPawn)
        {
            ControlledPawn.OnUnPossess();
            Debug.Log("ControlledPawn.OnUnpossess called");
        }

        ControlledPawn = p;
      
        p.Possesed(this);
      
    }

    public void PossessPawn(GameObject p, ulong clientID, ulong NetID)
    {
        PossessPawn(p);
        InvokeClientRpcOnClient(Client_PossessPawn, clientID, NetID);
    }

    [ClientRPC]
    public void Client_PossessPawn(ulong netID)
    {
        GameObject gObj = FindByNetID(netID);
        if(gObj)
        {
            PossessPawn(gObj);
        }
    }

    GameObject FindByNetID(ulong netID)
    {
        NetworkedObject[] netObjs = GameObject.FindObjectsOfType<NetworkedObject>();
        foreach (NetworkedObject netObj in netObjs)
        {
            if(netObj.NetworkId == netID)
            {
                return netObj.gameObject;
            }
        }
        return null;
    }    
}
