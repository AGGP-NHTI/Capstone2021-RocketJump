using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;

public class PositionNodeScript : MonoBehaviour
{
    public int nodeNumber;

    private void OnTriggerEnter(Collider other)
    {

        other.gameObject.GetComponent<PlayerController>().updateNodePosition(this);

        /*

        foreach(GameObject player in positionManager.players)
        {
            if(other.gameObject.name == player.gameObject.name)
            {
                positionManager.updatePlayerPosition(player, nodeNumber);
            }
        }

        */
    }

}
