using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;

public class Actor : NetworkedBehaviour
{
    //
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

    public GameObject NetSpawn(GameObject prefab, Vector3 location, Quaternion rotation)
    {
        GameObject obj = Instantiate(prefab, location, rotation);

        Debug.Log(obj.name);
        if (obj && obj.TryGetComponent(out NetworkedObject netObj))
        { 
            netObj.Spawn();

            return obj;
        }

        
        return null;
    }

    public GameObject NetSpawn(GameObject prefab, Transform parent)
    {
        GameObject projectile = Instantiate(prefab, parent);

        Debug.Log(projectile.name);
        if (projectile)
        {
            projectile.GetComponent<NetworkedObject>().Spawn();

            return projectile;
        }


        return null;
    }
}

