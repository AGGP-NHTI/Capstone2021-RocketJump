using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flicker : MonoBehaviour
{
    public Image image;
    public Color Solid;
    public Color transp;
    bool c;
    
    void FixedUpdate()
    {
        if (c)
        {
            image.color = Solid;
            c = false;
        }
        else
        {
            image.color = transp;
            c = true;
        }
    }
}
