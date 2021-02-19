using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_Manager : MonoBehaviour
{
    public NewPC player;
    public int currentItem = 0;
    public List<GameObject> items = new List<GameObject>();

    

    private void Update()
    {
        for (int i = 1; i < 10; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0 + i))
            {
                currentItem = i-1;
                checkActiveItems();
            }
        }
    }


    public void addItem(GameObject itemToAdd, bool setToCurrentActiveItem = false)
    {
        if (!items.Contains(itemToAdd))
        {
            itemToAdd.transform.parent = player.eyes;
            itemToAdd.transform.localPosition = Vector3.zero;
            itemToAdd.transform.localRotation = Quaternion.identity;
            items.Add(itemToAdd);

            if (setToCurrentActiveItem)
            {
                currentItem = (items.Count - 1);
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
        for (int i = 0; i < items.Count-1; i++)
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

            //
            if (i != currentItem)
            {
                items[i].SetActive(false);
            }
            else
            {
                items[currentItem].SetActive(true);
            }
        }

    }
}
