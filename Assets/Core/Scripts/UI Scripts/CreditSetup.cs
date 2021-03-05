using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditSetup : MonoBehaviour
{

    public CreditHover ch;
    public GameObject fact;
   
    void Start()
    {
        ch.Facts = fact;
    }

   
}
