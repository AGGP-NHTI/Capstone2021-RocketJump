using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomButton : Button
{
    public AudioSource source;
    public AudioClip clip;
    bool pressed = false;

    void FixedUpdate()
    {
        if (IsHighlighted() == true && !pressed)
        {           
            source.PlayOneShot(clip);
            pressed = true;
            
            //Debug.Log("Highlighted");
        }

        if (IsHighlighted() == false)
        {
            pressed = false;
        }
    }
}
