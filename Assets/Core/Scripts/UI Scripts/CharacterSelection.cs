using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;

public class CharacterSelection : NetworkedBehaviour
{
    public List<GameObject> characters = new List<GameObject>();
    public GameObject CSMenu;

    public void Selection(int c)
    {
        //GameObject go = Instantiate(characters[c], new Vector3(0, 0, 0), Quaternion.identity);
        //NetworkedObject netObj = go.GetComponent<NetworkedObject>();
        CSMenu.SetActive(false);
    }

    public void Rand()
    {
        int i = Random.Range(0, characters.Capacity);
        Selection(i);
    }
}
