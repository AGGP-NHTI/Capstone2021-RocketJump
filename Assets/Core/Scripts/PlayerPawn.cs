using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
using System.Runtime.InteropServices;

public class PlayerPawn : Pawn
{
    public GameObject camControl;
    public GameObject projSpawn;
    public GameObject projPrefab;
   // public Camera cam;

    public float MSensitivity = 15;
    public float MoveSpeed = 5;
    public float RotateSpeed = 90;
    public float PitchRate = 90;
    public Vector2 PitchRange = new Vector2(-89, 89);
    public bool InvertCam = true;

    Rigidbody rb;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        //cam = GetComponentInChildren<Camera>();
      /*  if(!IsLocalPlayer)
        { return; }
        else { cam.enabled = false; }*/

    }
    public void SetCamPitch(float value)
    {
        if (value == 0)
        { return; }

        if (InvertCam)
        { value *= -1; }

        float nextPitch = camControl.transform.rotation.eulerAngles.x;
        if (nextPitch > 180)
        { nextPitch -= 360; }

        float delta = (value * MSensitivity * PitchRate * Time.deltaTime);
    }

    //public override void  RotatePlayer(float value)
    //{
    //    gameObject.transform.Rotate(Vector3.up * value * MSensitivity * RotateSpeed * Time.deltaTime);
    //}

    //public override void Move(float horizontal, float vertical)
    //{
    //    Vector3 Direction = (gameObject.transform.forward * vertical) + (gameObject.transform.right * horizontal);
    //    Direction = Direction.normalized;
    //    rb.velocity = Direction * MoveSpeed;
    //}

    //public override void Fire1()
    //{
    //    if (IsServer)
    //    {
    //        SpawnProjectile();
    //    }
    //    else
    //    { InvokeServerRpc(SpawnProjectile); }
    //}

    [ServerRPC(RequireOwnership = false)]
    public void SpawnProjectile()
    {
        NetSpawn(projPrefab, projSpawn.transform.position, projSpawn.transform.rotation);
    }
}
