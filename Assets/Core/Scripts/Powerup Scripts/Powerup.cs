using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{

    public GameObject itemToGive;

   

    public void OnCollisionEnter(Collision collision)
    {
        PlayerController player = collision.transform.GetComponent<PlayerController>();

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
