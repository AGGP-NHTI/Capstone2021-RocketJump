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
    }

    public override bool Fire()
    {
        if (isDropped)
        {
            bool fired = base.Fire();
            Destroy(gameObject);
            return fired;
        }
        else 
        {
            dropItem();
            return true;
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
