using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;

public class CharacterSelection : NetworkedBehaviour
{
    public List<GameObject> characters = new List<GameObject>();
    public GameObject CSMenu;
    public GameObject cam;

    public void choice(int c)
    {
        cam.SetActive(false);
        InvokeServerRpc(Selection, c);
    }

    [ServerRPC(RequireOwnership = false)]
    public void Selection(int c)
    {
        CSMenu.SetActive(false);
        GameObject go = Instantiate(characters[c], new Vector3(0, 0, 0), Quaternion.identity);
        NetworkedObject netObj = go.GetComponent<NetworkedObject>();
        netObj.SpawnWithOwnership(OwnerClientId);
    }

    public void Rand()
    {
        int i = Random.Range(0, characters.Capacity);
        
        choice(i);
    }
}
