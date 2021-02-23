using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;

public class Inventory_Manager : Actor
{
    public List<GameObject> items = new List<GameObject>();

    [HideInInspector]
    public NewPC player;
    public int currentItem = 0;

    private void Update()
    {
        for (int i = 1; i < 10; i++)
        {

            if (Input.GetKeyDown(KeyCode.Alpha0 + i) && IsLocalPlayer)
            {
                int select = i - 1;


                if (select >= 0 && select < items.Count)
                {
                    currentItem = select;
                    checkActiveItems();
                }
            }
        }
    }

    [ServerRPC(RequireOwnership  = false)]
    public void addItem(GameObject itemToAdd, bool setToCurrentActiveItem = false)
    {


        if (!items.Contains(itemToAdd))
        {
            itemToAdd = Instantiate(itemToAdd, player.eyes);
            itemToAdd.transform.parent = player.eyes;
            itemToAdd.transform.localPosition = Vector3.zero;
            itemToAdd.transform.localRotation = Quaternion.identity;

            items.Add(itemToAdd);

            if (setToCurrentActiveItem)
            {
                currentItem = (items.Count - 1);
            }
            else
            {
                itemToAdd.SetActive(false);
            }

            checkActiveItems();
        }
    }

    public void dropItem(int whichIndex)
    {
        if (whichIndex >= items.Count) { return; }

        if (whichIndex <= currentItem)
        {
            currentItem--;
        }
        items.RemoveAt(whichIndex);

        checkActiveItems();
    }

    public void checkActiveItems()
    {
        if (items.Count == 1)
        {
            items[0].SetActive(true);
        }

        for (int i = 0; i < items.Count; i++)
        {

            //Check if the item exists
            if (!items[i])
            {
                if (i <= currentItem)
                {
                    currentItem--;
                }
                items.RemoveAt(i);
            }


            items[i].SetActive(false);
        }
        items[currentItem].SetActive(true);

    }
}
