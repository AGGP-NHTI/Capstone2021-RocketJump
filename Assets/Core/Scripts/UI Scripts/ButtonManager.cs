using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public List<CustomButton> buttons = new List<CustomButton>();
    public AudioSource source;
    public AudioClip clip;
    
    void Start()
    {
        foreach (CustomButton i in buttons)
        {
            i.source = source;
            i.clip = clip;
        }
    }

    
    void Update()
    {
        
    }
}
