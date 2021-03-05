using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomButton : Button
{   
    public AudioSource source;
    public AudioClip clip;
    bool pressed = false;

    public virtual void FixedUpdate()
    {
        if (IsHighlighted() == true && !pressed)
        {
            OnHighlight();           
        }

        if (IsHighlighted() == false)
        {
            OnHighlightEnd();
        }
    }

    public virtual void OnHighlight()
    {
        source.PlayOneShot(clip);
        pressed = true;
    }

    public virtual void OnHighlightEnd()
    {
        pressed = false;
    }
}
