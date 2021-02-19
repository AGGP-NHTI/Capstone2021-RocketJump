using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{

    public GameObject itemToGive;


    public void OnTriggerEnter(Collider obj)
    {
        Debug.Log("PLAYER ENTERED");

        NewPC player = obj.transform.GetComponent<NewPC>();

        if (player && !player.ownedItem)
        {
            player.giveItem(itemToGive);
            StartCoroutine(destroyAtEndofFrame());
        }
    }
    public void OnCollisionEnter(Collision obj)
    {
        Debug.Log("PLAYER ENTERED");

        NewPC player = obj.transform.GetComponent<NewPC>();

        if (player && !player.ownedItem)
        {
            player.giveItem(itemToGive);
            StartCoroutine(destroyAtEndofFrame());
        }
    }


    IEnumerator destroyAtEndofFrame()
    {
        yield return new WaitForEndOfFrame();

        Destroy(gameObject);
    }

}
