using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MLAPI;
using MLAPI.Messaging;

public class PPLogic : MonoBehaviour
{
    public List<GameObject> players = new List<GameObject>();

    public Payload payloadprefab;
    public Payload payload;

    void Start()
    {
        
    }

    void Update()
    {
        
    }
    
    public void BeginGame()
    {

    }

    public GameObject RandomPlayer()
    {
        int p = Random.Range(0, players.Capacity);
        return players[p];
    }
}
