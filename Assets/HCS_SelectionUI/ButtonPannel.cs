using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MLAPI;
using MLAPI.Transports.UNET;
using TMPro;
using System;
public class ButtonPannel : MonoBehaviour
{
    public GameObject Pannel;

    public TMP_InputField connectAddress;
    public TMP_InputField connectPort;  
	public TMP_InputField relayAddress;
	public TMP_InputField relayPort;

    void Start()
    {
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
        Pannel.SetActive(true);
    }


    public void StartasHost()
    {
		NetworkingManager.Singleton.gameObject.GetComponent<UnetTransport>().ConnectAddress = connectAddress.text;
		
		StartCoroutine(TaskStatus(NetworkingManager.Singleton.StartHost()));
        //Pannel.SetActive(false);
    }

    public void StartasClient()
    {
		//Debug.Log(Convert.ToInt32(relayPort.text));
		NetworkingManager.Singleton.gameObject.GetComponent<UnetTransport>().ConnectAddress = connectAddress.text;
		//Debug.Log("ca");
		//int i = Convert.ToInt32(connectPort.text);
		
		//NetworkingManager.Singleton.gameObject.GetComponent<UnetTransport>().ConnectPort = i;
		//Debug.Log("cp");

		NetworkingManager.Singleton.gameObject.GetComponent<UnetTransport>().MLAPIRelayAddress = relayAddress.text;
		//Debug.Log("ra");
		//NetworkingManager.Singleton.gameObject.GetComponent<UnetTransport>().MLAPIRelayPort = Convert.ToInt32(relayPort.text);
		//Debug.Log("rp");
		
		StartCoroutine(TaskStatus(NetworkingManager.Singleton.StartClient()));  
    }

	IEnumerator TaskStatus(MLAPI.Transports.Tasks.SocketTasks tasks)
	{
		Debug.Log($"Listening to {tasks.Tasks.Length} tasks . . .");
		yield return new WaitUntil(() => tasks.IsDone);

		Debug.Log(tasks.Tasks[0].SocketError);

		if (tasks.Success) Pannel.SetActive(false);
	}

    public void StartasServer()
    {
		NetworkingManager.Singleton.gameObject.GetComponent<UnetTransport>().ConnectAddress = connectAddress.text;
		StartCoroutine(TaskStatus(NetworkingManager.Singleton.StartServer()));
        //Pannel.SetActive(false);
    }


   
   
}
