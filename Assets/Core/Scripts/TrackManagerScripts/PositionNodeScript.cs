using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionNodeScript : MonoBehaviour
{
    public int nodeNumber;

    private void Start()
    {
        gameObject.transform.localScale = new Vector3(35, 50, .25f);
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + (gameObject.transform.localScale.y / 2), gameObject.transform.position.z);
    }

    private void OnTriggerEnter(Collider other)
    {

        var obj = other.gameObject.GetComponent<Player_Controller>();

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
