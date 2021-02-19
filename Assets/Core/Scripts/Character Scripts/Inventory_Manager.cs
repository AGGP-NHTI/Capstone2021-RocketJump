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
        if (Input.GetKeyDown(KeyCode.Alpha1)) { currentItem = 1; }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { currentItem = 2; }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { currentItem = 3; }
        if (Input.GetKeyDown(KeyCode.Alpha4)) { currentItem = 4; }
        if (Input.GetKeyDown(KeyCode.Alpha5)) { currentItem = 5; }
        if (Input.GetKeyDown(KeyCode.Alpha6)) { currentItem = 6; }
        if (Input.GetKeyDown(KeyCode.Alpha7)) { currentItem = 7; }
        if (Input.GetKeyDown(KeyCode.Alpha8)) { currentItem = 8; }
        if (Input.GetKeyDown(KeyCode.Alpha9)) { currentItem = 9; }
    }


    public void addItem(GameObject itemToAdd, bool setToCurrentActiveItem = false)
    {
        if (!items.Contains(itemToAdd))
        {
            itemToAdd.transform.parent = player.eyes;
            itemToAdd.transform.localPosition = Vector3.zero;
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
            if (!items[i])
            {
                if (i <= currentItem)
                {
                    currentItem--;
                }
                items.RemoveAt(i);
                
            }

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
