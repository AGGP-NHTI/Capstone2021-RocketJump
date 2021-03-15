using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionNodeScript : MonoBehaviour
{
    public int nodeNumber;

    private void OnTriggerEnter(Collider other)
    {

        Player_Movement_Controller obj = other.gameObject.GetComponent<Player_Movement_Controller>();

        if (obj)
        {
            if (!obj.IsLocalPlayer)
            {
                return;
            }


            obj.updateNodePosition(this);
        }

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
