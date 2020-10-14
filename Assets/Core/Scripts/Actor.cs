﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;

public class Actor : NetworkedBehaviour
{

    public bool IsActive = true;
    public bool IgnoreDamage = false;


    public void TakeDamage(float value)
    {
        if (IgnoreDamage)
        {
            return;
        }
        ProcessDamage(value);
    }

    public virtual void ProcessDamage(float value)
    {
        Debug.Log(gameObject.name + "Took Damage:" + value);
    }

    public virtual void ShowWho(string reason)
    {
        if (IsHost)
        {
            Debug.Log(reason + ": on Host");
        }
        if (IsClient)
        {
            Debug.Log(reason + ": on Client");
        }
        if (IsOwner)
        {
            Debug.Log(reason + ": on Owner");
        }
        if (IsLocalPlayer)
        {
            Debug.Log(reason + ": on Local Player");
        }
    }

    public void NetSpawn(GameObject prefab, Vector3 location, Quaternion rotation)
    {
        GameObject projectile = Instantiate(prefab, location, rotation);
        projectile.GetComponent<NetworkedObject>().Spawn();
    }
}

