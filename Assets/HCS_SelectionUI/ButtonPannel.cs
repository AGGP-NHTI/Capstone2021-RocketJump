using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MLAPI;

public class ButtonPannel : MonoBehaviour
{
    public GameObject Pannel;

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
        NetworkingManager.Singleton.StartClient();
        Pannel.SetActive(false);
    }


    public void StartasServer()
    {
        NetworkingManager.Singleton.StartServer();
        Pannel.SetActive(false);
    }


   
   
}
