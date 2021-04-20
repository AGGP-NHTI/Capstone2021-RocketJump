using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;

public class SpawnPointManager : NetworkedBehaviour
{ 
    public List<GameObject> spawnpoints = new List<GameObject>();
    public GameObject CamSpawn;
    public int currentspawn = 0;

    void Start()
    {
        foreach(Transform child in transform)
        {
            if(child.gameObject.activeSelf)
            {
                spawnpoints.Add(child.gameObject);
            }
        }
    }

    public Vector3 getSpawn()
    {
        return spawnpoints[0].transform.position;
    }

    [ServerRPC(RequireOwnership = false)]
    public Vector3 RequestSpawn()
    {
        currentspawn++;
        return spawnpoints[currentspawn - 1].transform.position;
    }
    
}
