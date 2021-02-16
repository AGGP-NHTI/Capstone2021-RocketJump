using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamManager : MonoBehaviour
{
    public List<CamPan> cams = new List<CamPan>();
    int index = 0;

    void Start()
    {
        foreach (CamPan i in cams)
        {
            i.gameObject.SetActive(false);
        }
        cams[0].gameObject.SetActive(true);
    }
   
    public void NextCam()
    {        
        cams[index].EndCam();

        if (index + 1 < cams.Capacity)
        {
            index++;
        }
        else
        {
            index = 0;
        }

        cams[index].gameObject.SetActive(true);
    }
}
