using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;


public class Powerup : Actor
{

    public GameObject itemToGive;


    public void OnTriggerEnter(Collider obj)
    {
        //Debug.Log("PLAYER ENTERED Trigger");

        Inventory_Manager inventoryManager = obj.gameObject.GetComponent<Inventory_Manager>();
        if (inventoryManager)
        {
            if (IsServer)
            {
                //inventoryManager.addItem(itemToGive, true);
            }
            else
            {
                //InvokeServerRpc(inventoryManager.addItem, itemToGive, true);
            }


            //player.giveItem(itemToGive);
            //StartCoroutine(destroyAtEndofFrame());
        }
    }


    IEnumerator destroyAtEndofFrame()
    {
        yield return new WaitForEndOfFrame();

        Destroy(gameObject);
    }

}
