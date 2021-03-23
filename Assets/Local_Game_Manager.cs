using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Local_Game_Manager : MonoBehaviour
{
    public GameObject characterSelectPrefab;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(characterSelectPrefab);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
