using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPlayerScript : MonoBehaviour
{

    public GameObject UI;
    public PlayerController player;

    private void Awake()
    {

        UI = Instantiate(UI, gameObject.transform);
        //UI.GetComponentInChildren<SpeedometerScript>().player = this; // Fix speedometer bug. Temporary fix, could probably be implimented better.

    }
}
