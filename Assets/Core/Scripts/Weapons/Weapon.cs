using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class Weapon : Actor
{

    public GameObject projectilePrefab;
    public Transform projectileSpawn;

    [Header("Stats")]
    public float reloadSpeed;
    public float fireRate;

    public int currentClip;
    public int clipSize;

    GameObject weaponModel;

    public abstract void Fire();
    public abstract void AltFire();
}

