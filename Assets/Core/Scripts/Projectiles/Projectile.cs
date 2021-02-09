using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
public abstract class Projectile : Actor
{
    protected Rigidbody rb;
    public float lifeTime = 3;
  


    [Range(0f,100f)]
    public float projectileLaunchForce = 10;


    public virtual void Start(){}
    public virtual void Update(){}
    protected virtual void OnDrawGizmos(){}
    protected abstract void hitSomething();

}
