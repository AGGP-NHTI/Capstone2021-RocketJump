﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
public abstract class Projectile : Actor
{
    public Rigidbody rb;

    float timeAlive = 0;
    public float lifeTime = 3;
    public float contactLifetime = 0;

    [Range(0f, 750f)]
    public float explosiveForce = 10;

    [Range(0f,20f)]
    public float explosiveDistance = 10;

    [Range(0f,1000f)]
    public float projectileLaunchForce = 10;


    protected virtual void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();

        if (rb)
        {
            rb.AddForce(transform.forward * projectileLaunchForce);
        }
    }
    protected virtual void Update()
    {
        timeAlive += Time.deltaTime;
        if (timeAlive >= lifeTime)
        {
            explode();
            lifeTime = 0;
        }
    }
    protected virtual void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, explosiveDistance);
    }

    protected abstract void OnCollisionEnter(Collision other);

    protected abstract void hitSomething();

    protected abstract void explode();

    protected abstract IEnumerator waitForExplode();

}
