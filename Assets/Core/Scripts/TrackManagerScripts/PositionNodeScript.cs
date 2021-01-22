using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionNodeScript : MonoBehaviour
{
    public int nodeNumber;
    public PositionManager positionManager;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "TestPlayer(Clone)") // Hacky for testing. Will be better handled later
        {

            //other.GetComponent<PlayerController>().positionManager.nodePosition = nodeNumber;

            //print(other.GetComponent<PlayerController>().positionManager.nodePosition);

        }
    }

}
