using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoExplode : MonoBehaviour
{
    public float rate = 0.1f;
    [HideInInspector]
    Vector3 endscale;
    public Vector3 startscale;
    [HideInInspector]
    public Vector3 localscale;
    public GameObject menu;
    
    
    bool done = false;
    
    void Start()
    {
        endscale = gameObject.transform.localScale;
        localscale = startscale;
        if (menu != null)
        {
            menu.SetActive(false);
        }
        
    }
    
    void FixedUpdate()
    {
        if (!done)
        {            
            localscale += new Vector3(rate, rate, 0);
            if (localscale.x >= endscale.x)
            {
                localscale.x = endscale.x;
                done = true;
                if (menu != null)
                {
                    menu.SetActive(true);
                }
            }           
        }
    }
    void Update()
    {
        if (!done)
        {
            gameObject.transform.localScale = localscale;
        }
    }
}
