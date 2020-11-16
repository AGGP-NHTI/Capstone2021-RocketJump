using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionManager : MonoBehaviour
{

    public GameObject track; // Point a reference to the track
    [SerializeField] List<Transform> positionNodes;

    private void Awake()
    {
        if(!track)
        {
            Debug.Log("Track not found; " + gameObject.name);
        }
        else
        {
            foreach(Transform node in track.transform.Find("positionNodes"))
            {
                positionNodes.Add(node);
            }
        }
    }

    void Update()
    {
        
    }
}
