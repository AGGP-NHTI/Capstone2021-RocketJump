using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MLAPI;
using MLAPI.Messaging;

public class PPLogic : NetworkedBehaviour
{
    public List<GameObject> players = new List<GameObject>();

    public Payload payloadprefab;
    public Payload payload;

    public NetworkingManager manager;

    public float gracetime = 15f;

    [ServerRPC(RequireOwnership = false)]
    public void BeginGame()
    {
        foreach (NewPC p in FindObjectsOfType<NewPC>())
        {
            players.Add(p.gameObject);
        }

        GameObject go = Instantiate(payloadprefab.gameObject, new Vector3(0, 0, 0), Quaternion.identity);
        NetworkedObject netObj = go.GetComponent<NetworkedObject>();

        payload = go.GetComponent<Payload>();
        payload.logic = this;
        
        StartCoroutine(GracePeriod());
    }
    
    public IEnumerator GracePeriod()
    {
        yield return new WaitForSeconds(gracetime);
        InvokeServerRpc(payload.ChoosePlayer);        
    }

    public GameObject RandomPlayer()
    {
        int p = Random.Range(0, players.Capacity);
        while (players[p] != null)
        {
            p = Random.Range(0, players.Capacity);
        }
        return players[p];
    }
}
