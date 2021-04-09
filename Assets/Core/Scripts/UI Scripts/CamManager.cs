using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamManager : MonoBehaviour
{
    [Header("Arena")]
    public List<CamPan> cams = new List<CamPan>();

    [Header("Race Track")]
    public List<CamPan> cams2 = new List<CamPan>();
    int index = 0;
    public int map = 0;

    void Start()
    {
        //map = Random.Range(0, 2);
        map = 1;
        //
        
        foreach (CamPan i in cams)
        {
            i.gameObject.SetActive(false);
        }
        foreach (CamPan i in cams2)
        {
            i.gameObject.SetActive(false);
        }

        if (map == 0)
        {
            cams[0].gameObject.SetActive(true);
        }
        else
        {
            cams2[0].gameObject.SetActive(true);
        }

    }
   
    public void NextCam()
    {
        if (map == 0)
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

        else
        {
            cams2[index].EndCam();

            if (index + 1 < cams2.Capacity)
            {
                index++;
            }
            else
            {
                index = 0;
            }

            cams2[index].gameObject.SetActive(true);
        }
    }
}
