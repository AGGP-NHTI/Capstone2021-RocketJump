using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MLAPI;
using MLAPI.Transports.UNET;
using TMPro;
public class ButtonPannel : MonoBehaviour
{
    public GameObject Pannel;
    public TMP_InputField connectAddress;
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
        NetworkingManager.Singleton.gameObject.GetComponent<UnetTransport>().ConnectAddress = connectAddress.text;
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
