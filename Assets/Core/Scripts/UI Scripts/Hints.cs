using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Hints : MonoBehaviour
{
    public List<string> hints = new List<string>();
    public TextMeshProUGUI loadtext;
    public bool IsSource = true;
    public Hints other;
    
    void Start()
    {
        Randomize();
       
    }

    public void Randomize()
    {
        if (IsSource)
        {
            int i = Random.Range(0, hints.Capacity);
            loadtext.text = hints[i];
        }
        else
        {
            int i = Random.Range(0, other.hints.Capacity);
            loadtext.text = other.hints[i];
        }       
    }
}
