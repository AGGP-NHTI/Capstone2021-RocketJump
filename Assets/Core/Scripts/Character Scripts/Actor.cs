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
    public AudioClip objSoundEffect;

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
        if (obj && obj.TryGetComponent(out NetworkedObject netObj))
        {
            netObj.Spawn();
            return obj;
        }


        return null;
    }

    protected void playObjSoundEffect()
    {
        if (objSoundEffect)
        {
            Audio_Manager.instance.PlayAudio(objSoundEffect, transform.position);
        }
        else
        {
            Debug.LogWarning("Sound effect not populated for: " + name);
        }
    }

    protected void playSoundEffect(AudioClip clip)
    {
        if (clip)
        {
            Audio_Manager.instance.PlayAudio(clip, transform.position);
        }
        else
        {
            Debug.LogWarning("Sound effect not populated for: " + name);
        }
    }

    protected void playLocalSoundEffect(AudioClip clip, float time = -1)
    {
        if (clip)
        {
            GameObject obj = Audio_Manager.instance.PlayAudio(clip.name, transform.position);
            if (obj) { obj.transform.parent = gameObject.transform; }
            if (time != -1) { Destroy(obj,time); }
        }
        else
        {
            Debug.LogWarning("Sound effect not populated for: " + name);
        }
    }
    //public GameObject NetSpawn(GameObject prefab, Transform parent)
    //{
    //    GameObject projectile = Instantiate(prefab, parent);

    //    Debug.Log(projectile.name);
    //    if (projectile)
    //    {
    //        projectile.GetComponent<NetworkedObject>().Spawn();

    //        return projectile;
    //    }


    //    return null;
    //}
}

