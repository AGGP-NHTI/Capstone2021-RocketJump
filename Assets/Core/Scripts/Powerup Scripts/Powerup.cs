using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{

    public GameObject itemToGive;


    public void OnTriggerEnter(Collider obj)
    {
        //Debug.Log("PLAYER ENTERED Trigger");

        Inventory_Manager player = obj.gameObject.GetComponent<Inventory_Manager>();
        if (player)
        {
            player.addItem(itemToGive);
            StartCoroutine(destroyAtEndofFrame());
        }
    }


    IEnumerator destroyAtEndofFrame()
    {
        yield return new WaitForEndOfFrame();

        Destroy(gameObject);
    }

}
