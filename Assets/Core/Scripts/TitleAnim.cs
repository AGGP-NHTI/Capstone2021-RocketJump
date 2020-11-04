using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleAnim : MonoBehaviour
{
    public GameObject parent;
    public TitleAnim previous;
    [HideInInspector]
    public bool finished = false;
    public float rate = 0.05f;
    public float delay = 0;
    public float dc;
    [HideInInspector]
    public bool xreached = false;
    [HideInInspector]
    public bool yreached = false;
    [HideInInspector]
    public Vector3 endscale;
    public Vector3 startscale = new Vector3(0, 0.5f, 0);
    [HideInInspector]
    public Vector3 localscale;
    bool go = false;

    void Start()
    {
        dc = delay;
    }
    void Awake()
    {
        endscale = gameObject.transform.localScale;
        localscale = startscale;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (previous != null)
        {
            if (previous.finished == true)
            {
                go = true;
            }
            else
            {
                go = false;
            }
        }
        else
        {
            go = true;
        }
        if (parent.activeSelf == false)
        {
            localscale = startscale;
        }
        gameObject.transform.localScale = localscale;
        if (go)
        {
            delay -= Time.deltaTime;
            if (delay <= 0)
            {
                if (!xreached)
                {
                    localscale += new Vector3(rate, 0, 0);
                    if (localscale.x >= endscale.x)
                    {
                        localscale.x = endscale.x;
                        xreached = true;
                    }
                }
                if (xreached && !yreached)
                {
                    localscale += new Vector3(0, rate, 0);
                    if (localscale.y >= endscale.y)
                    {
                        localscale.y = endscale.y;
                        yreached = true;
                        finished = true;
                        go = false;
                    }
                }
            }
        }
    }
}
