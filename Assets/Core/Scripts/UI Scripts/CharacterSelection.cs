﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.Transports.UNET;
using UnityEngine.SceneManagement;

public class CharacterSelection : NetworkedBehaviour
{
    public List<GameObject> characters = new List<GameObject>();
    public GameObject CSMenu;
    public GameObject cam;
    public GameObject loadingScreen;
    public SpawnPointManager sm;
    public MainMenu mainMenu;
    public serverInfo_SO connectionInfo;
    public bool isStartingAsHost;

    public void choice(int c)
    {
        //cam.SetActive(false);
        //InvokeServerRpc(Selection, c);
        mainMenu.playerInformationCarrier.GetComponent<PlayerInformationCarrier>().playerInfo.playerCharacter = c;

        if(isStartingAsHost)
        {
            hostServer();
        }
        else
        {
            connectToServer();
        }
    }

    public void connectToServer()
    {

        loadingScreen.SetActive(true);

        mainMenu.playerInformationCarrier.GetComponent<PlayerInformationCarrier>().playerInfo.isHosting = false;

        NetworkingManager.Singleton.gameObject.GetComponent<UnetTransport>().ConnectAddress = connectionInfo.connectAddress;

        int i;
        int.TryParse(connectionInfo.connectPort, out i);

        NetworkingManager.Singleton.gameObject.GetComponent<UnetTransport>().ConnectPort = i;

        NetworkingManager.Singleton.gameObject.GetComponent<UnetTransport>().MLAPIRelayAddress = connectionInfo.relayAddress;

        int l;
        int.TryParse(connectionInfo.relayPort, out l);
        NetworkingManager.Singleton.gameObject.GetComponent<UnetTransport>().MLAPIRelayPort = l;


        StartCoroutine(TaskStatus(NetworkingManager.Singleton.StartClient()));
    }

    public void hostServer()
    {
        loadingScreen.SetActive(true);

        mainMenu.playerInformationCarrier.GetComponent<PlayerInformationCarrier>().playerInfo.isHosting = true;

        SceneManager.LoadScene(1);
    }


    IEnumerator TaskStatus(MLAPI.Transports.Tasks.SocketTasks tasks)
    {
        Debug.Log($"Listening to {tasks.Tasks.Length} tasks . . .");
        yield return new WaitUntil(() => tasks.IsDone);

        Debug.Log(tasks.Tasks[0].SocketError);
    }

    [ServerRPC(RequireOwnership = false)]
    public void Selection(int c)
    {
        foreach (SpawnPointManager s in FindObjectsOfType<SpawnPointManager>())
        {
            sm = s; 
        }
        if (IsOwner)
        {
            InvokeServerRpc(NetSpawn, c);
        }
    }

    [ServerRPC(RequireOwnership = false)]
    public void NetSpawn(int c)
    {
        CSMenu.SetActive(false);

        GameObject go = Instantiate(characters[c], sm.RequestSpawn(), Quaternion.identity);
        NetworkedObject netObj = go.GetComponent<NetworkedObject>();
        netObj.SpawnWithOwnership(OwnerClientId);
    }
    public void Rand()
    {
        int i = Random.Range(0, characters.Capacity);
        
        choice(i);
    }
}
