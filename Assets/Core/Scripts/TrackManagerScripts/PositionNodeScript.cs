using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionNodeScript : MonoBehaviour
{
    public int nodeNumber;
    public bool showNode;

    private void Start()
    {
        gameObject.transform.localScale = new Vector3(50, 500, 3f); // resize node to fill track once game has started
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + (gameObject.transform.localScale.y / 2), gameObject.transform.position.z); // adjust nodes height based on resize
        gameObject.GetComponent<Renderer>().enabled = showNode; //Disable renderer to hide node in game. Set showNode to true if visible ingame nodes are needed for debugging purposes;
    }

    private void OnTriggerEnter(Collider other)
    {
        var obj = other.gameObject.GetComponent<Player_Pawn>().controller; // get the players controller

        if (obj) // if the controller exists (mean a player made contact)
        {
            if (!obj.IsLocalPlayer) // Make sure the contacting player is the local player (preventing from local instances of other players from triggering)
            {
                return; // If it isn't the local player, break function
            }


            obj.plrCntrl.PNC.updateNodePosition(this); // If it is the local player, start the node updating process 
        }

    }

}
