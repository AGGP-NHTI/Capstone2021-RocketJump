using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using MLAPI;
using MLAPI.Transports.UNET;

public class ServerSlotScript : MonoBehaviour
{
    public serverInfo_SO info;
    public TextMeshProUGUI serverName;
    public GameObject characterSelect;
    public GameObject lobby;

    public void Start()
    {

        if(info != null)
        {
            serverName.text = info.serverName;

            lobby = transform.GetComponentInParent<LobbyManager>().gameObject;
            characterSelect = transform.GetComponentInParent<Canvas>().gameObject.GetComponentInChildren<CharacterSelection>(true).gameObject;
        }
        
        NetworkingManager.Singleton.OnClientConnectedCallback += (obj) =>
        {
            if (NetworkingManager.Singleton.IsClient)
            {
                Debug.Log("Connected.");
            }
            else
            {
                Debug.Log("Client joined");
            }

        };

    }

    public void confirmSelection()
    {

        characterSelect.SetActive(true);
        lobby.SetActive(false);
        


        /*
        NetworkingManager.Singleton.gameObject.GetComponent<UnetTransport>().ConnectAddress = info.connectAddress;

        int i;
        int.TryParse(info.connectPort, out i);

        NetworkingManager.Singleton.gameObject.GetComponent<UnetTransport>().ConnectPort = i;

        NetworkingManager.Singleton.gameObject.GetComponent<UnetTransport>().MLAPIRelayAddress = info.relayAddress;

        int l;
        int.TryParse(info.relayPort, out l);
        NetworkingManager.Singleton.gameObject.GetComponent<UnetTransport>().MLAPIRelayPort = l;


        StartCoroutine(TaskStatus(NetworkingManager.Singleton.StartClient()));
        */
    }

    IEnumerator TaskStatus(MLAPI.Transports.Tasks.SocketTasks tasks)
    {
        Debug.Log($"Listening to {tasks.Tasks.Length} tasks . . .");
        yield return new WaitUntil(() => tasks.IsDone);

        Debug.Log(tasks.Tasks[0].SocketError);
    }
}
