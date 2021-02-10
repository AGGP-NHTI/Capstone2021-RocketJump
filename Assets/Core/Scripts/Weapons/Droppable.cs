using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Droppable : Weapon
{
    KeyCode UseDropBinding = KeyCode.E;
    bool isDropped = false;
    protected override void Update()
    {
        if (!isDropped)
        {
            base.Update();
        }
        else if(Input.GetKeyDown(UseDropBinding))// && IsLocalPlayer
        {
            Debug.Log("EXPLODE THE: " + transform.name);
            Fire();
            Destroy(gameObject);
        }
        
    }

    public override void Fire()
    {
        if (isDropped)
        {
            base.Fire();
        }
        else 
        {
            dropItem();
        }
    }

    protected void dropItem()
    {
        isDropped = true;

        //drop on ground animation by setting the parent to world
        transform.parent = null;


        //give a rb
        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
    }

}
