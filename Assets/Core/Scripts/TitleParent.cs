using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleParent : MonoBehaviour
{
    public GameObject parent;
    public List<TitleAnim> children;


    void Update()
    {
        if (parent.activeInHierarchy == false)
        {
            foreach (TitleAnim i in children)
            {
                i.gameObject.SetActive(false);
                i.localscale = i.startscale;
                i.xreached = false;
                i.yreached = false;
                i.finished = false;
                i.delay = i.dc;
            }
        }
        if (parent.activeInHierarchy != false)
        {
            foreach (TitleAnim i in children)
            {
                i.gameObject.SetActive(true);
            }
        }
    }
}
