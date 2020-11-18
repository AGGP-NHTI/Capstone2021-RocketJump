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

        Pannel.SetActive(true);
    }


    public void StartasHost()
    {
        NetworkingManager.Singleton.StartHost();
        Pannel.SetActive(false);
    }

    public void StartasClient()
    {
        NetworkingManager.Singleton.gameObject.GetComponent<UnetTransport>().ConnectAddress = connectAddress.text;
        NetworkingManager.Singleton.StartClient();
        Pannel.SetActive(false);
    }


    public void StartasServer()
    {
        NetworkingManager.Singleton.StartServer();
        Pannel.SetActive(false);
    }


   
   
}
