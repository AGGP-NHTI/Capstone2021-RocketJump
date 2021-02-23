using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{

    public GameObject itemToGive;


    public void OnTriggerEnter(Collider obj)
    {
        //Debug.Log("PLAYER ENTERED Trigger");

        NewPC player = obj.gameObject.GetComponent<NewPC>();
        if (player)
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
