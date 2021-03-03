using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Hints : MonoBehaviour
{
    public List<string> hints = new List<string>();
    public TextMeshProUGUI loadtext;
    
    void Start()
    {
        int i = Random.Range(0, hints.Capacity);
        loadtext.text = hints[i];
        //Debug.Log(hints[i]);
    }

}
