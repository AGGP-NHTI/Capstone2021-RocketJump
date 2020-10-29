using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
public class Projectile : Actor
{
    public Rigidbody proj;
    public float Damage = 5f;
    public float bulletSpeed = 200f;
    private void Start()
    {
        proj = gameObject.GetComponent<Rigidbody>();
    }
    private void Update()
    {
        proj.velocity = transform.forward * bulletSpeed;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(!IsServer)
        {
            return;
        }

        Actor act = other.gameObject.GetComponentInParent<Actor>();
        if(act)
        {
            act.TakeDamage(Damage);
            Destroy(gameObject);

            ShowWho("TakeDamage-Projectile Aka YA GOT SHOT");
        }
    }

    public override void ProcessDamage(float value)
    {
        base.ProcessDamage(value);
    }
}
