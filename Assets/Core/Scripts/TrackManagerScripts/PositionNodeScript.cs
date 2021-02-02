using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionNodeScript : MonoBehaviour
{
    public int nodeNumber;
    public PositionManager positionManager;

    private void Awake()
    {
        positionManager = GameObject.Find("track").GetComponent<PositionManager>(); // this is bad, i know. Its temporary
    }

    private void OnTriggerEnter(Collider other)
    {
        foreach(GameObject player in positionManager.players)
        {
            if(other.gameObject.name == player.gameObject.name)
            {
                positionManager.updatePlayerPosition(player, nodeNumber);
            }
        }
    }

}
