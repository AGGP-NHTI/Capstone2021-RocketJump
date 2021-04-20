using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
public abstract class Projectile : Actor
{
    //protected Player_Pawn player;
    protected Rigidbody rb;
    [Range(0f, 10f)]
    public float lifeTime = 3;
    [Range(0f, 10f)]
    public float contactLifetime = 0;
    [Range(0f,100f)]
    public float projectileLaunchForce = 10;
    float timeAlive = 0;

    public virtual void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>() ?? gameObject.AddComponent<Rigidbody>();

        if (rb)
        {
            rb.AddForce(transform.forward * projectileLaunchForce * 100);
        }
    }

    protected virtual void hitSomething()
    {
        StartCoroutine(waitForTrigger());
    }
    protected void OnCollisionEnter(Collision other)
    {
        //Debug.Log("COLLIDED WITH: " + other.transform.name);
        hitSomething();
    }
    public virtual void Update()
    {
        timeAlive += Time.deltaTime;
        if (timeAlive >= lifeTime)
        {
            trigger();
            lifeTime = 0;
        }
    }
    protected virtual IEnumerator waitForTrigger() 
    {
        yield return new WaitForSeconds(contactLifetime);
        trigger();
    }

    //public void setPlayer(Player_Pawn controller)
    //{
    //    player = controller;
    //}

    protected virtual void OnDrawGizmos(){}

    protected abstract void trigger();
    

}
